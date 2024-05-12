using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	/// <summary>
	/// Represents an engine-managed 2D array of <see cref="Tile"/>s.
	/// </summary>
	public class TileMap : IEnumerable<Tile>
	{
		[NotNull]
		private Tile?[,] _tileMap;

		/// <summary>
		/// Horizontal size of the <see cref="TileMap"/>.
		/// </summary>
		public int SizeX => _tileMap.GetLength(0);

		/// <summary>
		/// Vertical size of the <see cref="TileMap"/>.
		/// </summary>
		public int SizeY => _tileMap.GetLength(1);

		/// <param name="sizeX">Horizontal size.</param>
		/// <param name="sizeY">Vertical size.</param>
		/// <param name="fillWith"><see cref="Tile"/> to fill the <see cref="TileMap"/> with.</param>
		public TileMap(uint sizeX, uint sizeY, Tile? fillWith = null)
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

		/// <summary>
		/// Note that unlike regular 2D arrays, X is the first index, and Y is the second.
		/// </summary>
		/// <returns>Returns <see cref="Tile"/> at index.</returns>
		public Tile? this[int posX, int posY]
		{
			get => this[new(posX, posY)];
			set => this[new(posX, posY)] = value;
		}

		/// <returns>Returns <see cref="Tile"/> at index <see cref="Position"/>.</returns>
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

		/// <returns>Returns all <see cref="TileObject"/>s on this <see cref="TileMap"/>.</returns>
		public IEnumerable<TileObject> GetTileObjects() => this.Where(t => t.HasObject).Select(t => t.Object);

		internal void Destroy()
		{
			foreach (var tile in this.Where(t => t != null))
				tile.Destroy();
		}
	}
}
