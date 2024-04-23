using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class TileMap : IEnumerable<Tile>
	{
		[NotNull]
		private Tile[,] _tileMap;

		public int SizeX => _tileMap.GetLength(0);

		public int SizeY => _tileMap.GetLength(1);

		public TileMap(int sizeX, int sizeY, Tile? fillWith = null)
		{
			_tileMap = new Tile[sizeY, sizeX];
			fillWith ??= new Tile();
			for (int y = 0; y < SizeY; y++)
			{
				for (int x = 0; x < SizeX; x++)
					this[x, y] = fillWith!;
			}
		}

		public Tile this[int posX, int posY]
		{
			get => this[new(posX, posY)];
			set => this[new(posX, posY)] = value;
		}

		public Tile this[Position position]
		{
			get
			{
				if (WithinBounds(position))
					return _tileMap[position.y, position.x];
				return null;
			}

			internal set
			{
				if (WithinBounds(position))
				{
					if (_tileMap[position.y, position.x] != null)
						_tileMap[position.y, position.x].Destroy();
					_tileMap[position.y, position.x] = value;
				}
			}
		}

		public IEnumerator<Tile> GetEnumerator() => (IEnumerator<Tile>)_tileMap.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _tileMap.GetEnumerator();

		public IEnumerable<TileObject> GetTileObjects() => this.Where(t => t.HasObject).Select(t => t.Object);

		public bool WithinBounds(Position position) => position.x >= 0 && position.x < SizeX && position.y >= 0 && position.y < SizeY;
	}
}
