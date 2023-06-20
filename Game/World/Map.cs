using System.Diagnostics;
using Assets;
using static Game.World.Point2D;

namespace Game.World
{
    class Map
    {
        private char[,] _tileMap;
        public int SizeJ
        {
            get => _tileMap.GetLength(0);
        }
        public int SizeI
        {
            get => _tileMap.GetLength(1);
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

		public TileInfo GetTileInfo(Point2D tile)
		{
			return TileInfo.GetTileInfo(TileMap[tile.TileJ, tile.TileI]);
		}

		public TileInfo GetTileInfo(int tileJ, int tileI)
        {
            return TileInfo.GetTileInfo(TileMap[tileJ, tileI]);
        }

        public Point2D GetRandomTile()
        {
            return Tile(Random.Shared.Next(0, SizeJ), Random.Shared.Next(0, SizeI));
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
}
