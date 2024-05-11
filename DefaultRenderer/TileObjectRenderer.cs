namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	public class TileObjectRenderer : IConsoleRenderer
	{
		private static readonly string[] DEFAULT_CHARS = ["TO"];
		protected TileObject _tileObject;
		private string[] _chars;
		public byte FgColor;

		public string[] Chars
		{
			get => _chars;
			set => SetChars(value);
		}

		private int charSizeX;
		private int charSizeY;

		public FrameBuffer Buffer => ConsoleRenderer.Buffer;
		
		public int SizeX => TileSizeX;
		public int SizeY => TileSizeY;
		public (int, int) Size => (SizeX, SizeY);

		public TileObjectRenderer(TileObject tileObject, string[]? chars = null, byte fgColor = COLOR_WHITE)
		{
			_tileObject = tileObject;
			FgColor = fgColor;
			if (chars != null)
				Chars = chars;
			else
				Chars = DEFAULT_CHARS;
		}

		public virtual void UpdateFrame()
		{
			Render();
		}

		public void Render()
		{
			var buffer = new FrameBuffer(
				other: TileMapRenderer.GetFrameBufferAtPosition(Buffer, _tileObject.Position),
				offsetX: (int)Math.ClampMin(0, (SizeX - charSizeX) * .5f),
				offsetY: (int)Math.ClampMin(0, (SizeY - charSizeY) * .5f)
				);
			int sizeX = Math.ClampMax(SizeX, charSizeX);
			int sizeY = Math.ClampMax(SizeY, charSizeY);
			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					buffer.Char[x, y] = Chars[y][x];
					buffer.Foreground[x, y] = FgColor;
				}
			}
		}

		private void SetChars(string[]? chars)
		{
			if (chars != null)
			{
				charSizeX = chars.Max(s => s.Length);
				charSizeY = chars.GetLength(0);
				_chars = chars;
			}
			else
				Chars = DEFAULT_CHARS;
		}
	}
}
