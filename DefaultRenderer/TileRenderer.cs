using IronEngine.IO;

namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	internal class TileRenderer : IConsoleRenderer
	{
		public static int sizeX = 12;
		public static int sizeY = 6;

		protected Tile _tile;

		public static FrameBuffer Buffer => ConsoleRenderer.Buffer;

		public int SizeJ => sizeX;
		public int SizeI => sizeY;
		public (int, int) Size => (SizeJ, SizeI);

		public TileRenderer(Tile tile)
		{
			_tile = tile;
		}

		public virtual void UpdateFrame()
		{
			RenderTile();
			TryRenderTileObject();
		}

		public virtual void RenderTile()
		{
			var buffer = TileMapRenderer.GetFrameBufferAtPosition(Buffer, _tile.Position);
			for (int i = 0; i < SizeI; i++)
			{
				for (int j = 0; j < SizeJ; j++)
				{
					buffer[j, i] = EMPTY_CHAR;
				}
			}
		}

		public bool TryRenderTileObject()
		{
			if (_tile.HasObject)
			{
				if (_tile.Object is IRenderAble renderable)
				{
					renderable.GetRenderer().UpdateFrame();
					return true;
				}
			}
			return false;
		}
	}
}
