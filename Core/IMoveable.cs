using MovementStrategy = System.Func<IronEngine.IMoveable, IronEngine.Tile, System.Collections.Generic.IEnumerable<IronEngine.Tile>>;

namespace IronEngine
{
	public interface IMoveable : IPositionable
	{
		#region MEMBERS
		MovementStrategy DefaultMovementStrategy => Teleport;

		sealed void Move(Tile to, MovementStrategy strategy) => MoveInternal(this, to, strategy);

		sealed void Move(Tile to) => Move(to, DefaultMovementStrategy);

		sealed void Move(Position to, MovementStrategy strategy) => MoveInternal(this, to, strategy);

		sealed void Move(Position to) => Move(to, DefaultMovementStrategy);

		event Action<IMoveable, Position> OnObjectMoved;
		#endregion

		#region MOVEMENT_STRATEGIES
		/// <summary>
		/// Teleports the moveable to the target tile.
		/// </summary>
		public static IEnumerable<Tile> Teleport(IMoveable moveable, Tile to)
		{
			yield return to;
		}

		/// <summary>
		/// Moves the moveable to the target tile across the tilemap, taking the shortest direct path.
		/// </summary>
		public static IEnumerable<Tile> ShortestDirect(IMoveable moveable, Tile to)
		{
			if (!moveable.CheckSameTileMap(to)) yield break;
			Position current = moveable.Position;
			Position destination = to.Position;
			if (current == destination) yield break;
			do
			{
				current += new Position(
					x: Math.ClampRange(destination.x - current.x, -1, 1),
					y: Math.ClampRange(destination.y - current.y, -1, 1)
				);
			}
			while (current != destination);
		}
		#endregion

		#region INTERNAL_MOVEMENT_LOGIC
		internal void MoveInternal(IMoveable moveable, Position to, MovementStrategy movementStrategy)
		{
            if (!CheckHasTileMap() || !CheckWithinTileMap(to)) return;
            MoveInternal(moveable, moveable.TileMap[to], movementStrategy);
		}

		internal void MoveInternal(IMoveable moveable, Tile to, MovementStrategy movementStrategy)
		{
			IEnumerable<Tile> path = movementStrategy.Invoke(moveable, to);
			if (!path.Any()) return;
			Tile previous = moveable.CurrentTile;
			foreach (Tile current in path)
			{
				TeleportInternal(moveable, to);
				previous.OnObjectExitInternal(moveable);
				current.OnObjectEnterInternal(moveable);
				if (current != to)
					current.OnObjectPassInternal(moveable);
				previous = current;
			}
		}

		internal void TeleportInternal(IMoveable moveable, Tile to)
		{
			if (moveable is TileObject tileObject && !to.HasObject)
			{
				moveable.CurrentTile.Object = null;
				to.Object = tileObject;
			}
		}
		#endregion
	}
}
