namespace IronEngine
{
	public class TileObject : IPositionable
	{
		public Tile CurrentTile { get; private set; }

		public Position Position => CurrentTile.Position;
	}
}
