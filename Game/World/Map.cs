using System.Diagnostics;

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

        public TileInfo GetTileInfo(int posJ, int posI)
        {
            return TileInfo.GetTileInfo(TileMap[posJ, posI]);
        }

        public (int, int) GetRandomTileCoordinates()
        {
            return (Random.Shared.Next(0, SizeJ), Random.Shared.Next(0, SizeI));
        }

        public struct TileInfo
        {
            public static readonly Dictionary<char, TileInfo> TILE_INFO = new Dictionary<char, TileInfo>(){
			{'?', new TileInfo("Missing TILE_INFO", false)},
			{' ', new TileInfo("Ground", true)},
			{'s', new TileInfo("Stone Brick Floor", true)},
			{'S', new TileInfo("Stone Brick Wall", false)},
			{'g', new TileInfo("Grass", true)},
			{'r', new TileInfo("Rock Ground", true)},
			{'R', new TileInfo("Rock Wall", false)},
			{'w', new TileInfo("Water (Shallow)", true)},
			{'W', new TileInfo("Water (Deep)", false)},
			{'p', new TileInfo("Wooden Plank", true)},
			{'P', new TileInfo("Wooden Plank Wall", false)}
			};

            public string name;
            public bool passable;

            public TileInfo(string name, bool passable)
            {
                this.name = name;
                this.passable = passable;
            }

            public static TileInfo GetTileInfo(char c)
			{
				if (TILE_INFO.TryGetValue(c, out var info))
					return info;
                else
				{
					Debug.WriteLine($"No TILE_INFO found for character '{c}'.");
					return TILE_INFO['?'];
				}
            }
        }
    }
}
