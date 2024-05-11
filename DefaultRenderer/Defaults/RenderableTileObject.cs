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
	public class RenderableTileObject : TileObject, IRenderAble
	{
		private TileObjectRenderer _renderer;

		public byte FgColor { get => _renderer.FgColor; set => _renderer.FgColor = value; }
		public char Char { get => _renderer.Char; set => _renderer.Char = value; }

		public RenderableTileObject(Actor? actor = null) : base(actor)
		{
			_renderer = new TileObjectRenderer(this);
		}

		public IRenderer GetRenderer() => _renderer;
	}
}
