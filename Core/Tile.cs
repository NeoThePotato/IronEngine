using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class Tile
	{
		[NotNull]
		public TileMap TileMap { get; private init; }

		public Position Position { get; private init; }

		public TileObject? Object { get; private set; }

		public bool HasObject => Object != null;

		#region EVENTS
		public event Action<Tile, TileObject> OnObjectEnter;
		public event Action<Tile, TileObject> OnObjectExit;
		#endregion

		public bool SameTileMap(Tile other) => TileMap == other.TileMap;
	}
}
