namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	public class TileObjectRenderer : IConsoleRenderer
	{
		protected TileObject _tileObject;

		public FrameBuffer Buffer => ConsoleRenderer.Buffer;
		
		public int SizeJ => TileRenderer.sizeY;
		public int SizeI => TileRenderer.sizeX;
		public (int, int) Size => (SizeJ, SizeI);

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
			for (int i = 0; i < SizeI; i++)
			{
				for (int j = 0; j < SizeJ; j++)
				{
					buffer[j, i] = EMPTY_CHAR;
				}
			}
		}
	}
}
