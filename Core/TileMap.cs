using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class TileMap : IEnumerable<Tile>
	{
		[NotNull]
		private Tile?[,] _tileMap;

		public int SizeX => _tileMap.GetLength(0);

		public int SizeY => _tileMap.GetLength(1);

		public TileMap(int sizeX, int sizeY, Tile? fillWith = null)
		{
			_tileMap = new Tile[sizeY, sizeX];
			if (fillWith != null)
			{
				for (int y = 0; y < SizeY; y++)
				{
					for (int x = 0; x < SizeX; x++)
						fillWith.CloneDeep().BindToTileMapInternal(this, new(x, y));
				}
			}
		}

		public Tile? this[int posX, int posY]
		{
			get => this[new(posX, posY)];
			set => this[new(posX, posY)] = value;
		}

		public Tile? this[Position position]
		{
			get => this.WithinBounds(position) ? _tileMap[position.y, position.x] : null;
			internal set
			{
				if (this.WithinBounds(position))
				{
					Tile? tile = _tileMap[position.y, position.x];
					if (tile != null)
					{
						tile.UnBindFromTileMapInternal();
						tile.Destroy();
					}
					_tileMap[position.y, position.x] = value;
					if (value != null)
					{
						value.TileMap = this;
						value.Position = position;
					}
				}
			}
		}

		public IEnumerator<Tile> GetEnumerator()
		{
			foreach (var tile in _tileMap)
				yield return tile;
		}

		IEnumerator IEnumerable.GetEnumerator() => _tileMap.GetEnumerator();

		public IEnumerable<TileObject> GetTileObjects() => this.Where(t => t.HasObject).Select(t => t.Object);

		internal void Destroy()
		{
			foreach (var tile in this.Where(t => t != null))
				tile.Destroy();
		}
	}
}
