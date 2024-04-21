namespace IronEngine
{
	public abstract class TileObject
	{
		public Tile CurrentTile { get; private set; }

		public Position Position => CurrentTile.Position;
	}
}
