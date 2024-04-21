﻿using System.Diagnostics;
using static Game.World.Position;
using Assets;
using Assets.Templates;

namespace Game.World
{
	class Map
	{
		private char[,] _tileMap;
		public int TileSizeJ
		{
			get => _tileMap.GetLength(0);
		}
		public int TileSizeI
		{
			get => _tileMap.GetLength(1);
		}
		public int TileSize
		{
			get => _tileMap.Length;
		}
		public int PointSizeJ
		{
			get => TileToPoint(TileSizeJ);
		}
		public int PointSizeI
		{
			get => TileToPoint(TileSizeI);
		}
		public char[,] TileMap
		{ get => _tileMap; private set => _tileMap = value; }

		public Map(char[,] tileMap)
		{
			_tileMap = tileMap;
		}

		public Map(int sizeJ, int sizeI)
		{
			_tileMap = new char[sizeJ, sizeI];
		}

		public TileInfo GetTileInfo(Position tile)
		{
			return TileInfo.GetTileInfo(TileMap[tile.TileJ, tile.TileI]);
		}

		public TileInfo GetTileInfo(int tileJ, int tileI)
		{
			return TileInfo.GetTileInfo(TileMap[tileJ, tileI]);
		}

		public Position GetRandomPoint()
		{
			return new Position(Random.Shared.Next(0, PointSizeJ), Random.Shared.Next(0, PointSizeI));
		}

		public Position GetRandomTile()
		{
			return Tile(Random.Shared.Next(0, TileSizeJ), Random.Shared.Next(0, TileSizeI));
		}

		public struct TileInfo
		{
			public string name;
			public bool passable;

			public TileInfo(string name, bool passable)
			{
				this.name = name;
				this.passable = passable;
			}

			public static TileInfo GetTileInfo(char c)
			{
				if (Tiles.TILE_INFO.TryGetValue(c, out var info))
				{
					return info;
				}
				else
				{
					Debug.WriteLine($"No TILE_INFO found for character '{c}'.");
					return Tiles.TILE_INFO['?'];
				}
			}
		}
	}

	struct MapMetadata
	{
		public string name;
		public string filePath;
		public Position entryTile;
		public Position exitTile;

		public MapMetadata(string name, string filePath, Position entryTile, Position exitTile)
		{
			this.name = name;
			this.filePath = filePath;
			this.entryTile = entryTile;
			this.exitTile = exitTile;
		}

		public static MapMetadata GetMetadata(string levelName)
		{
			return MapTemplates.MAPS_DICTIONARY[levelName];
		}

		public override readonly string ToString()
		{
			return name;
		}
	}
}
