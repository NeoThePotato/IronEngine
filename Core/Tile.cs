namespace IronEngine
{
	public class Tile
	{
		public Position Position { get; private set; }

		#region EVENTS
		public event Action<Tile, TileObject> OnObjectEnter;
		public event Action<Tile, TileObject> OnObjectExit;
		#endregion
	}
}
