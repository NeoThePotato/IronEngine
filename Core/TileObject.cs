using System.Diagnostics;

namespace IronEngine
{
	public abstract class TileObject : ICloneable, IMoveable<TileObject>, IActionable, IPositionable, IDestroyable
	{
		private Tile _currentTile;

		public Tile Tile { get => _currentTile; protected set => Move(value); }

		public Actor? Actor { get; protected set; }

		public TileMap? TileMap => Tile?.TileMap;

		public Position Position => TileMap != null ? Tile.Position : Position.OutOfBounds;

		protected TileObject(Tile tile, Actor? actor = null)
		{
			Tile = tile;
			Actor = actor;
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

		public void Destroy()
		{
			if (Tile != null)
				Tile.Object = null;
		}

		public void Move(Tile to)
		{
			if (!Tile.SameTileMap(to))
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

		public abstract IEnumerable<Func<bool>>? GetAvailableActions();
	}
}
