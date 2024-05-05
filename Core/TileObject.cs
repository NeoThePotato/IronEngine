using MovementStrategy = System.Func<IronEngine.IMoveable, IronEngine.Tile, System.Collections.Generic.IEnumerable<IronEngine.Tile>>;

namespace IronEngine
{
	public abstract class TileObject : ICloneable, IHasActor, IMoveable, IDestroyable
	{
		private Tile _currentTile;
		private Actor? _actor;

		public Tile CurrentTile { get => _currentTile; set => this.Move(value); }

		public Actor? Actor
		{
			get => _actor;

			set
			{
				if (_actor != value)
				{
					Actor?.RemoveChild(this);
					_actor = value;
					Actor?.AddChild(this);
				}
			}
		}

		public TileMap? TileMap => CurrentTile?.TileMap;

		public Position Position { get => TileMap != null ? CurrentTile.Position : Position.OutOfBounds; set => Move(value); }

		protected TileObject(Tile tile, Actor? actor = null)
		{
			CurrentTile = tile;
			Actor = actor;
		}

		#region EVENTS
		public event Action<IMoveable, Position>? OnObjectMoved;
		#endregion

		#region MOVEMENT
		public virtual MovementStrategy DefaultMovementStrategy => (this as IMoveable).DefaultMovementStrategy;

		public void Move(Tile to, MovementStrategy strategy) => (this as IMoveable).Move(to, strategy);

		public void Move(Tile to) => (this as IMoveable).Move(to);

		public void Move(Position to, MovementStrategy strategy) => (this as IMoveable).Move(to, strategy);

		public void Move(Position to) => (this as IMoveable).Move(to);
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
			Actor?.RemoveChild(this);
			if (CurrentTile != null)
				CurrentTile.Object = null;
		}
	}
}
