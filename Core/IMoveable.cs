namespace IronEngine
{
	using MovementStrategy = Func<IMoveable, Tile, IEnumerable<Tile>>;

	/// <summary>
	/// Interface for all instances of <see cref="IPositionable"/> that can also move on the <see cref="TileMap"/>.
	/// </summary>
	public interface IMoveable : IPositionable
	{
		#region MEMBERS
		/// <summary>
		/// The default MovementStrategy of this instance.
		/// MovementStrategy is a function of type <see cref="MovementStrategy"/> that returns an <see cref="IEnumerable"/> of <see cref="Tile"/>s this instance move across, in order..
		/// </summary>
		MovementStrategy DefaultMovementStrategy => Teleport;

		/// <summary>
		/// Move to <paramref name="to"/> using <paramref name="strategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Tile"/>.</param>
		/// <param name="strategy">Function of type <see cref="MovementStrategy"/> to use.</param>
		sealed void Move(Tile to, MovementStrategy strategy) => MoveInternal(to, strategy);

		/// <summary>
		/// Move to <paramref name="to"/> using <see cref="DefaultMovementStrategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Tile"/>.</param>
		sealed void Move(Tile to) => Move(to, DefaultMovementStrategy);

		/// <summary>
		/// Move to <see cref="Tile"/> at <see cref="Position"/> <paramref name="to"/> using <paramref name="strategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Position"/>.</param>
		/// <param name="strategy">Function of type <see cref="MovementStrategy"/> to use.</param>
		sealed void Move(Position to, MovementStrategy strategy) => MoveInternal(to, strategy);

		/// <summary>
		/// Move to <see cref="Tile"/> at <see cref="Position"/> <paramref name="to"/> using <see cref="DefaultMovementStrategy"/>.
		/// </summary>
		/// <param name="to">Target <see cref="Position"/>.</param>
		/// <param name="strategy">Function of type <see cref="MovementStrategy"/> to use.</param>
		sealed void Move(Position to) => Move(to, DefaultMovementStrategy);
		#endregion

		#region MOVEMENT_STRATEGIES
		/// <summary>
		/// Teleports the <paramref name="moveable"/> to the target <see cref="Tile"/>.
		/// </summary>
		public static IEnumerable<Tile> Teleport(IMoveable moveable, Tile to)
		{
			yield return to;
		}

		/// <summary>
		/// Moves the <paramref name="moveable"/> to the target tile across the <see cref="TileMap"/>, taking the shortest direct path.
		/// </summary>
		public static IEnumerable<Tile> ShortestDirect(IMoveable moveable, Tile to)
		{
			if (!moveable.CheckSameTileMap(to)) yield break;
			TileMap tileMap = moveable.TileMap;
			Position current = moveable.Position;
			Position destination = to.Position;
			if (current == destination) yield break;
			do
			{
				current += new Position(
					x: Math.ClampRange(destination.x - current.x, -1, 1),
					y: Math.ClampRange(destination.y - current.y, -1, 1)
				);
				yield return tileMap[current];
			}
			while (current != destination);
		}
		#endregion

		#region INTERNAL_MOVEMENT_LOGIC
		internal sealed void MoveInternal(Position to, MovementStrategy movementStrategy)
		{
			if (!this.CheckHasTileMap() || !this.CheckWithinTileMap(to)) return;
			MoveInternal(TileMap[to], movementStrategy);
		}

		internal sealed void MoveInternal(Tile to, MovementStrategy movementStrategy)
		{
			IEnumerable<Tile> path = movementStrategy.Invoke(this, to);
			if (!path.Any()) return;
			Tile previous = CurrentTile;
			foreach (Tile current in path)
			{
				previous.OnObjectExitInternal(this);
				TeleportInternal(to, false);
				current.OnObjectEnterInternal(this);
				if (current != to)
					current.OnObjectPassInternal(this);
				previous = current;
			}
		}

		internal sealed void TeleportInternal(Tile to, bool overrideExisting = false)
		{
			if (this is TileObject tileObject && (!to.HasObject || overrideExisting))
			{
				CurrentTile?.OverrideObjectInternal(null);
				to.OverrideObjectInternal(tileObject);
			}
		}
		#endregion
	}
}
