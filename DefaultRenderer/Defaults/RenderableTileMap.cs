using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;

namespace DefaultRenderer.Defaults
{
	/// <summary>
	/// A <see cref="TileMap"/> which implements a default <see cref="IRenderAble"/> using the <see cref="Console"/>.
	/// </summary>
	public class RenderableTileMap : TileMap, IRenderAble
	{
		private TileMapRenderer _renderer;

		public RenderableTileMap(uint sizeX, uint sizeY, Tile? fillWith = null) : base(sizeX, sizeY, fillWith)
		{ }

		public IRenderer GetRenderer() => _renderer;
	}
}
