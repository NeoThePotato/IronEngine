using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;

namespace DefaultRenderer.Defaults
{
	/// <summary>
	/// A <see cref="TileObject"/> which implements a default <see cref="IRenderAble"/> using the <see cref="Console"/>.
	/// </summary>
	public class RenderableTileObject : TileObject, IRenderAble
	{
		private TileObjectRenderer _renderer;

		/// <summary>
		/// Foreground color in ANSI.
		/// </summary>
		public byte FgColor { get => _renderer.FgColor; set => _renderer.FgColor = value; }

		/// <summary>
		/// Strings to print in place of this <see cref="TileObject"/>.
		/// </summary>
		public string[]? Chars { get => _renderer.Chars; set => _renderer.Chars = value; }

		public RenderableTileObject(Actor? actor = null) : base(actor)
		{
			_renderer = new TileObjectRenderer(this);
		}

		public IRenderer GetRenderer() => _renderer;
	}
}
