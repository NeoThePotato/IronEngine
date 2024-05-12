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
	public class RenderableTileMap : TileMap, IRenderAble
	{
		private TileMapRenderer _renderer;

		public RenderableTileMap(uint sizeX, uint sizeY, Tile? fillWith = null) : base(sizeX, sizeY, fillWith)
		{ }

		public IRenderer GetRenderer() => _renderer;
	}
}
