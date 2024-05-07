using MovementStrategy = System.Func<IronEngine.IMoveable, IronEngine.Tile, System.Collections.Generic.IEnumerable<IronEngine.Tile>>;

namespace IronEngine
{
	public interface IMoveable : IPositionable
	{
		#region MEMBERS
		MovementStrategy DefaultMovementStrategy => Teleport;

		sealed void Move(Tile to, MovementStrategy strategy) => MoveInternal(to, strategy);

		sealed void Move(Tile to) => Move(to, DefaultMovementStrategy);

		sealed void Move(Position to, MovementStrategy strategy) => MoveInternal(to, strategy);

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
				TeleportInternal(to);
				previous.OnObjectExitInternal(this);
				current.OnObjectEnterInternal(this);
				if (current != to)
					current.OnObjectPassInternal(this);
				previous = current;
			}
		}

		internal sealed void TeleportInternal(Tile to)
		{
			if (this is TileObject tileObject && !to.HasObject)
			{
				CurrentTile?.OverrideObjectInternal(null);
				to.OverrideObjectInternal(tileObject);
			}
		}
		#endregion
	}
}
