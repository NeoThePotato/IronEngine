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

		public RenderableTileObject(Actor? actor = null) : base(actor)
		{
			_renderer = new TileObjectRenderer(this);
		}

		public IRenderer GetRenderer() => _renderer;
	}
}
