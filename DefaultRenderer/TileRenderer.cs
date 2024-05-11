using IronEngine.IO;

namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	public class TileRenderer : IConsoleRenderer
	{
		public static int sizeX = 6;
		public static int sizeY = 3;

		protected Tile _tile;
		public byte BgColor;

		public static FrameBuffer Buffer => ConsoleRenderer.Buffer;

		public int SizeJ => sizeY;
		public int SizeI => sizeX;
		public (int, int) Size => (SizeJ, SizeI);

		public TileRenderer(Tile tile, byte bgColor = COLOR_BLACK)
		{
			_tile = tile;
			BgColor = bgColor;
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
					buffer.Background[j, i] = BgColor;
				}
			}
		}

		public bool TryRenderTileObject()
		{
			if (_tile.HasObject && _tile.Object is IRenderAble renderable)
			{
				renderable.GetRenderer().UpdateFrame();
				return true;
			}
			return false;
		}
	}
}
