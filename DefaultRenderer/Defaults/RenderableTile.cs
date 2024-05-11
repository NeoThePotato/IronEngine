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
	public class RenderableTile : Tile, IRenderAble
	{
		private TileRenderer _renderer;
		public byte BgColor { get => _renderer.BgColor; set => _renderer.BgColor = value; }

		public RenderableTile()
		{
			_renderer = new TileRenderer(this);
		}

		public IRenderer GetRenderer() => _renderer;
	}
}
