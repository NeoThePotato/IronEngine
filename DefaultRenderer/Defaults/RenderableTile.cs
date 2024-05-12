using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultRenderer.Defaults
{
	/// <summary>
	/// A <see cref="Tile"/> which implements a default <see cref="IRenderAble"/> using the <see cref="Console"/>.
	/// </summary>
	public class RenderableTile : Tile, IRenderAble
	{
		private TileRenderer _renderer;

		/// <summary>
		/// Background color in ANSI.
		/// </summary>
		public byte BgColor { get => _renderer.BgColor; set => _renderer.BgColor = value; }

		public RenderableTile()
		{
			_renderer = new TileRenderer(this);
		}

		public IRenderer GetRenderer() => _renderer;
	}
}
