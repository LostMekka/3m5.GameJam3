﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
	public class Level : MonoBehaviour
	{
		public Tile TilePrefab;
		public Wall WallPrefab;
		public int Width;
		public int Height;

		private List<Tile> tiles;

		// Use this for initialization
		private void Start()
		{
			Debug.Log(Environment.Version);
			tiles = new List<Tile>(Width * Height);
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Tile tile = Instantiate(TilePrefab);
					InitTile(tile.gameObject, "Tile", x, y);
					tiles.Add(tile);
				}
			}
			for (int x = 0; x < Width; x++)
			{
				InitTile(Instantiate(WallPrefab).gameObject, "Wall", x, -1);
				InitTile(Instantiate(WallPrefab).gameObject, "Wall", x, Height);
			}
		}

		private void InitTile(GameObject tile, string objectName, int x, int y)
		{
			tile.transform.position = new Vector3(x - Width * 0.5f, y - Height * 0.5f);
			tile.transform.parent = transform;
			tile.name = string.Format("{2} at ({0}, {1})", x, y, objectName);
		}

		private void CheckRange(int x, int y)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
			{
				throw new IndexOutOfRangeException(string.Format("point ({0}, {1}) is not in level tile set (width {2}, height {3})", x, y, Width, Height));
			}
		}

		public Tile GetTileAt(int x, int y)
		{
			CheckRange(x, y);
			return tiles[x + y * Width];
		}

		// Update is called once per frame
		private void Update()
		{
		}
	}
}