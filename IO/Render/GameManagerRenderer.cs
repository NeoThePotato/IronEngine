using Game;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private readonly (char, byte, byte) BORDER_INFO_V = ('║', 15, 0);
		private readonly (char, byte, byte) BORDER_INFO_H = ('═', 15, 0);

		public GameManager GameManager
		{ get; private set; }
		public LevelManagerRenderer LevelManagerRenderer
		{ get; private set; }
		public GameUIManagerRenderer UIManagerRenderer
		{ get; private set; }
		public override int SizeJ
		{ get => BorderLinesJ.Length + LevelManagerRenderer.SizeJ + GameManager.DataLog.MAX_SIZE; }
		public override int SizeI
		{ get => BorderLinesI.Length + LevelManagerRenderer.SizeI + GameUIManagerRenderer.SIDEBAR_WIDTH; }
		private int[] BorderLinesJ
		{ get; }
		private int[] BorderLinesI
		{ get; }

		public GameManagerRenderer(GameManager gameManager)
		{
			GameManager = gameManager;
			UIManagerRenderer = new GameUIManagerRenderer(GameManager.UIManager, this);
			LevelManagerRenderer = new LevelManagerRenderer(GameManager);
			BorderLinesJ = new int[3];
			BorderLinesI = new int[3];
		}

		public override void Render(FrameBuffer buffer)
		{
			LevelManagerRenderer.Render(new FrameBuffer(buffer, 1, 1));
			UIManagerRenderer.Render(buffer);
		}

		public override void RenderToCache(FrameBuffer buffer)
		{
			UpdateBorderSizes();
			RenderBorders(buffer);
			buffer = new FrameBuffer(buffer, 1, 1);
			LevelManagerRenderer.RenderToCache(buffer);
		}

		public override bool Validate()
		{
			return LevelManagerRenderer.Validate();
		}

		private void RenderBorders(FrameBuffer buffer)
		{
			// Render vertical borders
			foreach (var border in BorderLinesI)
			{
				for (int j = 0; j < SizeJ; j++)
				{
					buffer[j, border] = BORDER_INFO_V;
				}
			}

			// Render horizontal borders
			foreach (var border in BorderLinesJ)
			{
				for (int i = 0; i < SizeI; i++)
				{
					buffer[border, i] = BORDER_INFO_H;
				}
			}

			// Corners
			buffer.Char[BorderLinesJ[0], BorderLinesI[0]] = '╔';
			buffer.Char[BorderLinesJ[1], BorderLinesI[0]] = '╠';
			buffer.Char[BorderLinesJ[2], BorderLinesI[0]] = '╚';
			buffer.Char[BorderLinesJ[0], BorderLinesI[1]] = '╦';
			buffer.Char[BorderLinesJ[1], BorderLinesI[1]] = '╬';
			buffer.Char[BorderLinesJ[2], BorderLinesI[1]] = '╩';
			buffer.Char[BorderLinesJ[0], BorderLinesI[2]] = '╗';
			buffer.Char[BorderLinesJ[1], BorderLinesI[2]] = '╣';
			buffer.Char[BorderLinesJ[2], BorderLinesI[2]] = '╝';
		}

		private void UpdateBorderSizes()
		{
			BorderLinesJ[0] = 0;
			BorderLinesJ[1] = LevelManagerRenderer.SizeJ + 1;
			BorderLinesJ[2] = LevelManagerRenderer.SizeJ + GameManager.UIManager.DataLog.MAX_SIZE + 2;
			BorderLinesI[0] = 0;
			BorderLinesI[1] = LevelManagerRenderer.SizeI + 1;
			BorderLinesI[2] = LevelManagerRenderer.SizeI + GameUIManagerRenderer.SIDEBAR_WIDTH + 2;
		}
	}
}
