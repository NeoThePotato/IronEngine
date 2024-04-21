using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class Tile : IPositionable
	{
		[NotNull]
		public TileMap TileMap { get; internal init; }

		public Position Position { get; internal init; }

		public TileObject? Object { get; internal set; }

		public bool HasObject => Object != null;

		#region EVENTS
		public event Action<Tile, TileObject> OnObjectEnter;
		public event Action<Tile, TileObject> OnObjectExit;
		#endregion

		public bool SameTileMap(Tile other) => TileMap == other.TileMap;
	}
}
