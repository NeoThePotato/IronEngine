namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	public class TileObjectRenderer : IConsoleRenderer
	{
		protected TileObject _tileObject;

		public FrameBuffer Buffer => ConsoleRenderer.Buffer;
		
		public int SizeX => TileRenderer.sizeX;
		public int SizeY => TileRenderer.sizeY;
		public (int, int) Size => (SizeX, SizeY);

		public TileObjectRenderer(TileObject tileObject)
		{
			_tileObject = tileObject;
		}

		public virtual void UpdateFrame()
		{
			Render();
		}

		public void Render()
		{
			var buffer = TileMapRenderer.GetFrameBufferAtPosition(Buffer, _tileObject.Position);
			for (int y = 0; y < SizeY; y++)
			{
				for (int x = 0; x < SizeX; x++)
				{
					buffer[x, y] = EMPTY_CHAR;
				}
			}
		}
	}
}
