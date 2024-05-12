namespace IronEngine
{
	using MovementStrategy = Func<IMoveable, Tile, IEnumerable<Tile>>;

	/// <summary>
	/// Represents an object which lives on a <see cref="Tile"/>.
	/// </summary>
	public abstract class TileObject : ICloneable, IHasActor, IMoveable, IDestroyable
	{
		internal Tile _currentTile;
		private Actor? _actor;

		/// <summary>
		/// The <see cref="Tile"/> on which this <see cref="TileObject"/> lives.
		/// </summary>
		public Tile CurrentTile { get => _currentTile; set => (this as IMoveable).TeleportInternal(value); }

		/// <summary>
		/// The <see cref="Actor"/> to which this <see cref="TileObject"/> belongs.
		/// </summary>
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

		/// <summary>
		/// The <see cref="TileMap"/> to which this <see cref="TileObject"/> belongs.
		/// </summary>
		public TileMap? TileMap => CurrentTile?.TileMap;

		/// <summary>
		/// The <see cref="Position"/> of this <see cref="TileObject"/> on the <see cref="TileMap"/>.
		/// </summary>
		public Position Position { get => TileMap != null ? CurrentTile.Position : Position.OutOfBounds; set => Move(value); }

		protected TileObject(Actor? actor = null)
		{
			Actor = actor;
		}

		#region CALLBACKS
		/// <summary>
		/// Callback function for when a <see cref="TileObject"/> enters <see langword="this"/>'s <see cref="Tile"/> via movement.
		/// </summary>
		/// <param name="other">The entering <see cref="TileObject"/>.</param>
		public virtual void OnObjectEnter(TileObject other) { }

		/// <summary>
		/// Callback function for when a <see cref="TileObject"/> temporarily passes <see langword="this"/>'s <see cref="Tile"/> via movement.
		/// </summary>
		/// <param name="other">The passing <see cref="TileObject"/>.</param>
		public virtual void OnObjectPass(TileObject other) { }

		/// <summary>
		/// Callback function for when a <see cref="TileObject"/> exists <see langword="this"/>'s <see cref="Tile"/> via movement.
		/// </summary>
		/// <param name="other">The exiting <see cref="TileObject"/>.</param>
		public virtual void OnObjectExit(TileObject other) { }
		#endregion

		#region MOVEMENT
		public virtual MovementStrategy DefaultMovementStrategy => (this as IMoveable).DefaultMovementStrategy;

		/// <summary>
		/// Move to <paramref name="to"/> using <paramref name="strategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Tile"/>.</param>
		/// <param name="strategy">Function of type <see cref="MovementStrategy"/> to use.</param>
		public void Move(Tile to, MovementStrategy strategy) => (this as IMoveable).Move(to, strategy);

		/// <summary>
		/// Move to <paramref name="to"/> using <see cref="DefaultMovementStrategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Tile"/>.</param>
		public void Move(Tile to) => (this as IMoveable).Move(to);

		/// <summary>
		/// Move to <see cref="Tile"/> at <see cref="Position"/> <paramref name="to"/> using <paramref name="strategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Position"/>.</param>
		/// <param name="strategy">Function of type <see cref="MovementStrategy"/> to use.</param>
		public void Move(Position to, MovementStrategy strategy) => (this as IMoveable).Move(to, strategy);

		/// <summary>
		/// Move to <see cref="Tile"/> at <see cref="Position"/> <paramref name="to"/> using <see cref="DefaultMovementStrategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Position"/>.</param>
		/// <param name="strategy">Function of type <see cref="MovementStrategy"/> to use.</param>
		public void Move(Position to) => (this as IMoveable).Move(to);
		#endregion

		public object Clone() => CloneDeep();

		/// <returns>A clone of this <see cref="TileObject"/> with no <see cref="Tile"/> association.</returns>
		public virtual TileObject CloneDeep()
		{
			var clone = Utilities.CloneShallow(this);
			clone._currentTile = null;
			return clone;
		}

		/// <summary>
		/// Destroys this <see cref="TileObject"/>.
		/// </summary>
		public void Destroy()
		{
			CurrentTile?.OverrideObjectInternal(null);
			DestroyInternal();
		}

		internal void DestroyInternal()
		{
			_currentTile = null;
			Actor?.RemoveChild(this);
		}
	}
}
