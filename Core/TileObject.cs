using System.Diagnostics;

namespace IronEngine
{
	public abstract class TileObject : ICloneable, IMoveable<TileObject>, IHasActor, IPositionable
	{
		private Tile _currentTile;

		public Tile CurrentTile { get => _currentTile; protected set => Move(value); }

		public Actor? Actor { get; protected set; }

		public TileMap? TileMap => CurrentTile?.TileMap;

		public Position Position => TileMap != null ? CurrentTile.Position : Position.OutOfBounds;

		protected TileObject(Tile tile)
		{
			CurrentTile = tile;
		}

		#region EVENTS
		public event Action<TileObject, Position>? OnObjectMoved;
		#endregion

		public object Clone() => CloneDeep();

		public virtual TileObject CloneDeep()
		{
			var clone = Utilities.CloneShallow(this);
			clone._currentTile = null;
			return clone;
		}

		public void Move(Tile to)
		{
			if (!CurrentTile.SameTileMap(to))
			{
				Debug.WriteLine($"{to} is not on the same TileMap as {this}.");
				return;
			}
			throw new NotImplementedException(); // TODO Think hard about how to implement it
			OnObjectMoved?.Invoke(this, to.Position);
		}

		public void Move(Position to)
		{
			if (TileMap == null)
			{
				Debug.WriteLine($"{this} is not on a TileMap.");
				return;
			}
			if (!TileMap.WithinBounds(to))
			{
				Debug.WriteLine($"{to} is not within the bounds of TileMap.");
				return;
			}
			Move(TileMap[to]);
		}
	}
}
