using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class Tile : ICloneable, IPositionable, IActionable, IDestroyable
	{
		private TileObject? _tileObject;

		[NotNull]
		public TileMap TileMap { get; internal set; }

		public Position Position { get; internal set; }

		public TileObject? Object
		{
			get => _tileObject;
			internal set
			{
				if (HasObject)
					Object!.Destroy();
				_tileObject = value;
			}
		}

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

		public void Destroy()
		{
			Object?.Destroy();
			if (TileMap != null)
				TileMap[Position] = null;
		}

		public bool SameTileMap(Tile other) => TileMap != null && TileMap == other.TileMap;

		public virtual IEnumerable<Func<bool>>? GetAvailableActions() => null;
	}
}
