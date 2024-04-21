using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class Tile
	{
		public Position Position { get; private set; }

		public TileObject? Object { get; private set; }

		public bool HasObject => Object != null;

		#region EVENTS
		public event Action<Tile, TileObject> OnObjectEnter;
		public event Action<Tile, TileObject> OnObjectExit;
		#endregion
	}
}
