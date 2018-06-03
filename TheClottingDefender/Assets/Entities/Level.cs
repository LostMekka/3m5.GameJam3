using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

		private List<Tile> tiles;

		// Use this for initialization
		private void Start()
		{
			tiles = new List<Tile>(Width * Height);
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					Tile tile = Instantiate(TilePrefab);
					tile.X = x;
					tile.Y = y;
					tile.IsBlockingChanged.AddListener(UpdateFlowField);
					InitTile(tile.gameObject, "Tile", x, y);
					tiles.Add(tile);
				}
			}

			for (var x = 0; x < Width; x++)
			{
				InitTile(Instantiate(WallPrefab).gameObject, "Wall", x, -1);
				InitTile(Instantiate(WallPrefab).gameObject, "Wall", x, Height);
			}

			UpdateFlowField();
		}

		private void InitTile(GameObject tile, string objectName, int x, int y)
		{
			tile.transform.localPosition = new Vector3(x - (Width - 1) * 0.5f, y - (Height - 1) * 0.5f);
			tile.transform.parent = transform;
			tile.name = string.Format("{2} at ({0}, {1})", x, y, objectName);
		}

		private bool IsInRange(int x, int y)
		{
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}

		public Tile GetTileAt(int x, int y)
		{
			return IsInRange(x, y) ? tiles[x + y * Width] : null;
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
				Tile t = GetTileAt(Width - 1, y);
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