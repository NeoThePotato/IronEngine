using System.Xml.Linq;

namespace World
{
	class Map
	{
		private static readonly Dictionary<char, TileInfo> TILE_INFO = new Dictionary<char, TileInfo>(){
			{' ', new TileInfo("Ground", true)},
			{'w', new TileInfo("Wall", false)}
		};

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
			return GetTileInfo(TileMap[posJ, posI]);
		}

		public static TileInfo GetTileInfo(char c)
		{
			return TILE_INFO[c];
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
		}
	}
}
