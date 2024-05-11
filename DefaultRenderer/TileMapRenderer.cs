using IronEngine.IO;

namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	public class TileMapRenderer : IConsoleRenderer
	{
		public static FrameBuffer Buffer => ConsoleRenderer.Buffer;
		private TileMap TileMap { get; set; }
		public int SizeJ => TileMap.SizeX * TileRenderer.sizeY;
		public int SizeI => TileMap.SizeY * TileRenderer.sizeX;
		public (int, int) Size => (SizeJ, SizeI);

		public TileMapRenderer(TileMap tileMap)
		{
			TileMap = tileMap;
		}

		public void UpdateFrame()
		{
			foreach (Tile tile in TileMap)
			{
				if (tile != null && tile is IRenderAble renderable)
					renderable.GetRenderer().UpdateFrame();
			}
		}

		public static FrameBuffer GetFrameBufferAtPosition(FrameBuffer buffer, Position position)
		{
			(int bufferX, int bufferY) = TileMapPositionToFrameBufferPosition(position);
			return new FrameBuffer(buffer, bufferX, bufferY);
		}

		public static (int, int) TileMapPositionToFrameBufferPosition(Position position) => (position.x * TileRenderer.sizeY, position.y * TileRenderer.sizeX);
	}
}
