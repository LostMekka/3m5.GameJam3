using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
	public class Level : MonoBehaviour
	{
		public Enemy EnemyPrefab;
		public Tile TilePrefab;
		public Wall WallPrefab;
		public int Width;
		public int Height;
		public int AdditionalWidth = 10;

		public Sprite BloodyWallSprite;
		public Sprite ChangingWallSprite;
		public Sprite SyntheticWallSprite;
		public Sprite BloodyBgSprite;
		public Sprite ChangingBgSprite;
		public Sprite SyntheticBgSprite;

		public GameController GameController;

		public bool DebugFlowField;

		private List<Tile> tiles;
		private BoxCollider2D levelEndCollider;

		// Use this for initialization
		private void Start()
		{
			int middle = Width / 2;
			tiles = new List<Tile>(Width * Height);
			for (var y = 0; y < Height; y++)
			{
				for (int x = -AdditionalWidth; x < Width + AdditionalWidth; x++)
				{
					Tile tile = Instantiate(TilePrefab);
					tile.X = x;
					tile.Y = y;
					tile.IsInteractable = x >= 0 && x < Width;
					tile.IsBlockingChanged.AddListener(UpdateFlowField);
					tile.ParentLevel = this;
					InitTile(tile.gameObject, "Tile", x, y, false);
					tiles.Add(tile);
				}
			}

			for (int x = -AdditionalWidth; x < Width + AdditionalWidth; x++)
			{
				InitTile(Instantiate(WallPrefab).gameObject, "Wall", x, -1, true);
				InitTile(Instantiate(WallPrefab).gameObject, "Wall", x, Height, true);
			}

			UpdateFlowField();

			levelEndCollider = gameObject.AddComponent<BoxCollider2D>();
			levelEndCollider.isTrigger = true;
			// ReSharper disable once PossibleLossOfFraction
			levelEndCollider.offset = new Vector2(Width / 2 + 3, 0);
			levelEndCollider.size = new Vector2(4, Height + 1);
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (!coll.gameObject.CompareTag("Enemy")) return;
			var e = coll.gameObject.GetComponent<Enemy>();
			if (e != null) GameController.OnEnemyPasses(e);
		}

		private void InitTile(GameObject tile, string objectName, int x, int y, bool isWall)
		{
			tile.transform.localPosition = new Vector3(x - (Width - 1) * 0.5f, y - (Height - 1) * 0.5f);
			tile.transform.parent = transform;
			tile.name = string.Format("{2} at ({0}, {1})", x, y, objectName);
			Sprite sprite = null;
			if (x < Width / 2) sprite = isWall ? BloodyWallSprite : BloodyBgSprite;
			if (x == Width / 2) sprite = isWall ? ChangingWallSprite : ChangingBgSprite;
			if (x > Width / 2) sprite = isWall ? SyntheticWallSprite : SyntheticBgSprite;
			tile.GetComponent<SpriteRenderer>().sprite = sprite;
		}

		private bool IsInRange(int x, int y)
		{
			return x >= -AdditionalWidth && x < Width + AdditionalWidth && y >= 0 && y < Height;
		}

		public Tile GetTileAt(int x, int y)
		{
			return IsInRange(x, y) ? tiles[x + AdditionalWidth + y * (Width + 2 * AdditionalWidth)] : null;
		}

		public Tile GetTileForGlobalPosition(float x, float y)
		{
			//TODO: make performant
			foreach (Tile tile in tiles)
			{
				float tx = tile.transform.position.x;
				float ty = tile.transform.position.y;
				if (tx - 0.5f <= x && x <= tx + 0.5f && ty - 0.5f <= y && y <= ty + 0.5f) return tile;
			}

			return null;
		}

		public Enemy SpawnEnemy()
		{
			Enemy e = Instantiate(EnemyPrefab);
			e.transform.parent = transform;
			e.transform.localPosition = new Vector2(-Width * 0.5f - 5, Random.value * (Height - 1) - Height * 0.5f + 0.5f);
			e.GetComponent<Rigidbody2D>().AddTorque((Random.value - 0.5f) * 100);
			e.ParentLevel = this;
			return e;
		}

		// Update is called once per frame
		private void Update()
		{
			if (DebugFlowField)
			{
				foreach (Tile tile in tiles)
				{
					foreach (Tile target in tile.FlowFieldTargets)
					{
						Debug.DrawLine(tile.transform.position, target.transform.position, Color.green);
					}
				}
			}
		}

		private List<Tile> GetNeighbours(Tile tile)
		{
			var neighbours = new List<Tile>(8);
			Tile t = GetTileAt(tile.X + 1, tile.Y + 1);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X + 1, tile.Y);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X + 1, tile.Y - 1);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X, tile.Y + 1);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X, tile.Y - 1);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X - 1, tile.Y + 1);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X - 1, tile.Y);
			if (t != null) neighbours.Add(t);
			t = GetTileAt(tile.X - 1, tile.Y - 1);
			if (t != null) neighbours.Add(t);
			return neighbours;
		}

		public void UpdateFlowField()
		{
			foreach (Tile tile in tiles)
			{
				tile.FlowFieldTargets.Clear();
				tile.FlowFieldDistance = null;
			}

			var toExpand = new List<Tile>();
			for (var y = 0; y < Height; y++)
			{
				Tile t = GetTileAt(Width + AdditionalWidth - 1, y);
				if (t.IsBlocking) continue;
				toExpand.Add(t);
				t.FlowFieldDistance = 0;
			}

			while (toExpand.Count > 0)
			{
				Tile currTile = toExpand[0];
				toExpand.RemoveAt(0);
				int distanceToWrite = (currTile.FlowFieldDistance ?? 0) + 1;
				foreach (Tile neighbour in GetNeighbours(currTile))
				{
					if (neighbour.IsBlocking) continue;
					int dx = neighbour.X - currTile.X;
					int dy = neighbour.Y - currTile.Y;
					if (dx != 0 && dy != 0)
					{
						Tile t1 = GetTileAt(currTile.X + dx, currTile.Y);
						Tile t2 = GetTileAt(currTile.X, currTile.Y + dy);
						if ((t1 == null || t1.IsBlocking) && (t2 == null || t2.IsBlocking)) continue;
					}

					if (neighbour.FlowFieldDistance != null)
					{
						if (neighbour.FlowFieldDistance < distanceToWrite) continue;
					}
					else
					{
						toExpand.Add(neighbour);
					}

					neighbour.FlowFieldDistance = distanceToWrite;
					neighbour.FlowFieldTargets.Add(currTile);
				}
			}
		}

		public Vector2[] GetFlowAt(int x, int y)
		{
			Tile tile = GetTileAt(x, y);
			if (tile == null || tile.FlowFieldTargets.Count == 0)
			{
				if (y < 0) return new[] {(Vector2.up + Vector2.right).normalized};
				if (y >= Height) return new[] {(Vector2.down + Vector2.right).normalized};
				return new[] {Vector2.right};
			}

			var pos = new Vector2(tile.X, tile.Y);
			return tile.FlowFieldTargets
				.Select(t => (new Vector2(t.X, t.Y) - pos).normalized)
				.ToArray();
		}
	}
}