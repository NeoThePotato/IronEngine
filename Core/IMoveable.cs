using System.Diagnostics;
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
		public static IEnumerable<Tile> Teleport(IMoveable moveable, Tile to)
		{
			yield return to;
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
			// TODO Implement
		}
		#endregion
	}
}
