namespace IronEngine
{
	internal interface IMoveable<T> where T : TileObject
	{
		void Move(Position to);

		event Action<T, Position> OnObjectMoved;
	}
}
