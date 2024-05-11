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

		public int SizeX => sizeX;
		public int SizeY => sizeY;
		public (int, int) Size => (SizeX, SizeY);

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
			for (int y = 0; y < SizeY; y++)
			{
				for (int x = 0; x < SizeX; x++)
				{
					buffer.Background[x, y] = BgColor;
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
