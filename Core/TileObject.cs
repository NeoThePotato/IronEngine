namespace IronEngine
{
	using MovementStrategy = Func<IMoveable, Tile, IEnumerable<Tile>>;

	public abstract class TileObject : ICloneable, IHasActor, IMoveable, IDestroyable
	{
		private Tile _currentTile;
		private Actor? _actor;

		public Tile CurrentTile { get => _currentTile; set => (this as IMoveable).TeleportInternal(value); }

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

		protected TileObject(Actor? actor = null)
		{
			Actor = actor;
		}

		#region CALLBACKS
		public virtual void OnObjectEnter(TileObject other) { }

		public virtual void OnObjectPass(TileObject other) { }

		public virtual void OnObjectExit(TileObject other) { }
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
				CurrentTile.SetObjectInternal(null);
		}
	}
}
