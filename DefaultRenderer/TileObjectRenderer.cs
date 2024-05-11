namespace IronEngine.DefaultRenderer
{
	using static ConsoleRenderer;

	internal class TileObjectRenderer : IConsoleRenderer
	{
		protected TileObject _tileObject;

		public FrameBuffer Buffer => ConsoleRenderer.Buffer;
		
		public int SizeJ => TileRenderer.sizeX;
		public int SizeI => TileRenderer.sizeY;
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
