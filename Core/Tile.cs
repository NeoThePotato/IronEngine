using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class Tile : ICloneable, IPositionable, IHasActor
	{
		[NotNull]
		public TileMap TileMap { get; internal set; }

		public Position Position { get; internal set; }

		public TileObject? Object { get; internal set; }

		public bool HasObject => Object != null;

		public Actor? Actor { get; internal set; }

		#region EVENTS
		public event Action<Tile, TileObject> OnObjectEnter;
		public event Action<Tile, TileObject> OnObjectExit;
		#endregion

		public object Clone() => CloneDeep();

		public virtual Tile CloneDeep()
		{
			var clone = Utilities.CloneShallow(this);
			clone.Position = Position.OutOfBounds;
			clone.TileMap = null;
			clone.Object = Object?.CloneDeep();
			return clone;
		}

		public bool SameTileMap(Tile other) => TileMap != null && TileMap == other.TileMap;
	}
}
