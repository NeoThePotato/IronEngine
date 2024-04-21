using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class TileObject : ICloneable, IMoveable<TileObject>, IPositionable
	{
		[DisallowNull]
		public Tile CurrentTile { get; private set; }

		public TileMap TileMap => CurrentTile.TileMap;

		public Position Position => CurrentTile.Position;

		#region EVENTS
		public event Action<TileObject, Position>? OnObjectMoved;
		#endregion

		private TileObject([DisallowNull] Tile tile)
		{
			CurrentTile = tile;
			tile.Object = this;
		}

		public static TileObject Create([DisallowNull] Tile tile, bool overrideTile)
		{
			if (!overrideTile && tile.HasObject)
			{
				Debug.WriteLine($"{tile} already contains object {tile.Object}.");
				return null;
			}
			return new TileObject(tile);
		}

		public object Clone()
		{
			return Create(CurrentTile, true);
		}

		public void Move(Tile to)
		{
			throw new NotImplementedException(); // TODO Think hard about how to implement it
			OnObjectMoved?.Invoke(this, to.Position);
		}

		public void Move(Position to) => Move(TileMap[to]);
	}
}
