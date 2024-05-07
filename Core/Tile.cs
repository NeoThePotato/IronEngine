using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	public class Tile : ICloneable, IPositionable, IHasActor, IDestroyable
	{
		private TileObject? _tileObject;
		private Actor? _actor;

		[NotNull]
		public TileMap TileMap { get; internal set; }

		public Tile CurrentTile => this;

		public Position Position { get; internal set; }

		public TileObject? Object
		{
			get => _tileObject;
			set
			{
				if (value != null)
					value.CurrentTile = this;
				else
					OverrideObjectInternal(null);
			}
		}

		public bool HasObject => Object != null;

		public Actor? Actor
		{
			get => _actor;
			set
			{
				Actor?.RemoveChild(this);
				_actor = value;
				Actor?.AddChild(this);
			}
		}

		#region CALLBACKS
		internal void OnObjectEnterInternal(IMoveable other)
		{
			if (other is TileObject tileObject)
			{
				OnObjectEnter(tileObject);
				if (HasObject)
					Object!.OnObjectEnter(tileObject);
			}
		}

		internal void OnObjectPassInternal(IMoveable other)
		{
			if (other is TileObject tileObject)
			{
				OnObjectPass(tileObject);
				if (HasObject)
					Object!.OnObjectPass(tileObject);
			}
		}

		internal void OnObjectExitInternal(IMoveable other)
		{
			if (other is TileObject tileObject)
			{
				OnObjectExit(tileObject);
				if (HasObject)
					Object!.OnObjectExit(tileObject);
			}
		}

		public virtual void OnObjectEnter(TileObject other) { }

		public virtual void OnObjectPass(TileObject other) { }

		public virtual void OnObjectExit(TileObject other) { }
		#endregion

		public object Clone() => CloneDeep();

		public virtual Tile CloneDeep()
		{
			var clone = Utilities.CloneShallow(this);
			clone.Position = Position.OutOfBounds;
			clone.OverrideObjectInternal(Object?.CloneDeep());
			return clone;
		}

		public void Destroy()
		{
			var tileObject = UnbindObjectInternal();
			tileObject?.Destroy();
			Actor?.RemoveChild(this);
			if (TileMap != null)
				TileMap[Position] = null;
		}

		internal void OverrideObjectInternal(TileObject? obj)
		{
			UnbindObjectInternal()?.DestroyInternal();
			_tileObject = obj;
			if (_tileObject != null)
				_tileObject._currentTile = this;
		}

		internal TileObject? UnbindObjectInternal()
		{
			if (!HasObject)
				return null;
			_tileObject!._currentTile = null;
			_tileObject = null;
			return _tileObject;
		}

		internal void BindToTileMapInternal(TileMap tileMap, Position position)
		{
			TileMap = tileMap;
			Position = position;
			tileMap[position] = this;
		}

		public bool SameTileMap(Tile other) => TileMap != null && TileMap == other.TileMap;
	}
}
