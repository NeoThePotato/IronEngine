namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	public class TileObjectRenderer : IConsoleRenderer
	{
		protected TileObject _tileObject;
		public char Char;
		public byte FgColor;

		public FrameBuffer Buffer => ConsoleRenderer.Buffer;
		
		public int SizeX => TileSizeX;
		public int SizeY => TileSizeY;
		public (int, int) Size => (SizeX, SizeY);

		public TileObjectRenderer(TileObject tileObject, char ch = ' ', byte fgColor = COLOR_BLACK)
		{
			_tileObject = tileObject;
			Char = ch;
			FgColor = fgColor;
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
					buffer.Char[x, y] = Char;
					buffer.Foreground[x, y] = FgColor;
				}
			}
		}
	}
}
