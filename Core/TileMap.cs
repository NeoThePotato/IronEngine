using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public sealed class TileMap : IEnumerable<Tile>
	{
		[NotNull]
		private Tile[,] _tileMap;

		public int SizeX => _tileMap.GetLength(0);

		public int SizeY => _tileMap.GetLength(1);

		public TileMap(int sizeJ, int sizeI)
		{
			_tileMap = new Tile[sizeJ, sizeI];
		}

		public Tile this[int posX, int posY]
		{
			get => this[new(posX, posY)];
		}

		public Tile this[Position position]
		{
			get => _tileMap[position.y, position.x];
			internal set => _tileMap[position.y, position.x] = value;
		}

		public IEnumerator<Tile> GetEnumerator() => (IEnumerator<Tile>)_tileMap.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _tileMap.GetEnumerator();

		public IEnumerable<TileObject> GetTileObjects() => this.Where(t => t.HasObject).Select(t => t.Object);

		public bool WithinBounds(Position position) => position.x >= 0 && position.x < SizeX && position.y >= 0 && position.y < SizeY;
	}
}
