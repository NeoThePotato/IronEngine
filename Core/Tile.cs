using System.Diagnostics.CodeAnalysis;

namespace IronEngine
{
	/// <summary>
	/// Represents a tile on a <see cref="TileMap"/>.
	/// </summary>
	public class Tile : ICloneable, IPositionable, IHasActor, IDestroyable
	{
		private TileObject? _tileObject;
		private Actor? _actor;

		/// <summary>
		/// The <see cref="TileMap"/> to which this <see cref="Tile"/> belongs.
		/// </summary>
		[NotNull]
		public TileMap TileMap { get; internal set; }

		/// <summary>
		/// Returns <see langword="this"/>.
		/// </summary>
		public Tile CurrentTile => this;

		/// <summary>
		/// The <see cref="Position"/> of this <see cref="Tile"/> on the <see cref="TileMap"/>.
		/// </summary>
		public Position Position { get; internal set; }

		/// <summary>
		/// The <see cref="TileObject"/> which is positioned on this <see cref="Tile"/>.
		/// <see langword="null"/> if empty.
		/// </summary>
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

		/// <summary>
		/// Whether this <see cref="Tile"/> has an <see cref="Object"/> (<see cref="Object"/> != <see langword="null"/>).
		/// </summary>
		public bool HasObject => Object != null;

		/// <summary>
		/// The <see cref="Actor"/> to which this <see cref="Tile"/> belongs.
		/// <see langword="null"/> if doesn't belong to an <see cref="Actor"/>.
		/// </summary>
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

		/// <summary>
		/// Callback function for when a <see cref="TileObject"/> enters this tile via movement.
		/// </summary>
		/// <param name="other">The entering <see cref="TileObject"/>.</param>
		public virtual void OnObjectEnter(TileObject other) { }

		/// <summary>
		/// Callback function for when a <see cref="TileObject"/> temporarily passes this tile via movement.
		/// </summary>
		/// <param name="other">The passing <see cref="TileObject"/>.</param>
		public virtual void OnObjectPass(TileObject other) { }

		/// <summary>
		/// Callback function for when a <see cref="TileObject"/> exists this tile via movement.
		/// </summary>
		/// <param name="other">The exiting <see cref="TileObject"/>.</param>
		public virtual void OnObjectExit(TileObject other) { }
		#endregion

		public object Clone() => CloneDeep();

		/// <returns>A clone of this <see cref="Tile"/>. With no <see cref="TileMap"/> association.</returns>
		public virtual Tile CloneDeep()
		{
			var clone = Utilities.CloneShallow(this);
			clone.Position = Position.OutOfBounds;
			clone.OverrideObjectInternal(Object?.CloneDeep());
			return clone;
		}

		/// <summary>
		/// Destroys this <see cref="Tile"/> and its child <see cref="Object"/>.
		/// </summary>
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

		internal void UnBindFromTileMapInternal()
		{
			var tileMap = TileMap;
			if (tileMap == null)
				return;
			var position = Position;
			TileMap = null;
			tileMap[position] = null;
		}

		/// <returns>Whether <see langword="this"/> and <paramref name="other"/> share the same <see cref="TileMap"/>.</returns>
		public bool SameTileMap(Tile other) => TileMap != null && TileMap == other.TileMap;
	}
}
