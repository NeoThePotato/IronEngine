using System.Collections;

namespace IronEngine
{
	public sealed class TileMap : IEnumerable<Tile>
	{
		private Tile[,] _tileMap;

		public int SizeX => _tileMap.GetLength(0);

		public int SizeY => _tileMap.GetLength(1);

		public TileMap(int sizeJ, int sizeI)
		{
			_tileMap = new Tile[sizeJ, sizeI];
		}

		public bool WithinBounds(Position position)
		{
			return position.x >= 0 && position.x < SizeX && position.y >= 0 && position.y < SizeY;
		}

		public IEnumerator<Tile> GetEnumerator() => (IEnumerator<Tile>)_tileMap.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _tileMap.GetEnumerator();
	}
}
