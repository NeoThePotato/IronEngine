using Game;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private readonly (char, byte, byte) BORDER_INFO_V = ('║', 15, 0);
		private readonly (char, byte, byte) BORDER_INFO_H = ('═', 15, 0);
		private int[] _borderLinesJ;
		private int[] _borderLinesI;

		private GameManager GameManager
		{ get; set; }
		private LevelManagerRenderer LevelManagerRenderer
		{ get; set; }
		private MenuManagerRenderer InGameMenuRenderer
		{ get; set; }
		public override int SizeJ
		{ get => _borderLinesJ.Length + LevelManagerRenderer.SizeJ + GameManager.DATALOG_LENGTH; }
		public override int SizeI
		{ get => _borderLinesI.Length + LevelManagerRenderer.SizeI; }

		public GameManagerRenderer(GameManager gameManager)
		{
			GameManager = gameManager;
			InGameMenuRenderer = new MenuManagerRenderer (GameManager.InGameMenu);
			LevelManagerRenderer = new LevelManagerRenderer(GameManager.LevelManager);
			_borderLinesJ = new int[] {0, LevelManagerRenderer.SizeJ + 1, LevelManagerRenderer.SizeJ + GameManager.DATALOG_LENGTH + 2 };
			_borderLinesI = new int[] {0, LevelManagerRenderer.SizeI + 1};
		}

		public override void Render(FrameBuffer buffer)
		{
			if (GameManager.StateMenu)
			{
				InGameMenuRenderer.Render(buffer);
			}
			else
			{
				RenderBorders(buffer); // TODO Probably cache this
				var levelBuffer = new FrameBuffer(buffer, 1, 1);
				RenderLevelAndEntities(levelBuffer);
				var dataLogBuffer = new FrameBuffer(levelBuffer, _borderLinesJ[1], 0);
				RenderDataLog(dataLogBuffer);
			}
		}

		private void RenderLevelAndEntities(FrameBuffer buffer)
		{
			LevelManagerRenderer.Render(buffer);
			RenderPlayer(buffer);
		}

		private void RenderPlayer(FrameBuffer buffer)
		{
			LevelManagerRenderer.RenderEntity(buffer, GameManager.PlayerEntity, '@', 15);
		}

		private void RenderDataLog(FrameBuffer buffer)
		{
			RenderText(buffer, GameManager.DataLog, LevelManagerRenderer.SizeI);
		}

		private void RenderBorders(FrameBuffer buffer)
		{
			// Render vertical borders
			foreach (var border in _borderLinesI)
			{
				for (int j = 0; j < SizeJ; j++)
				{
					buffer[j, border] = BORDER_INFO_V;
				}
			}

			// Render horizontal borders
			foreach (var border in _borderLinesJ)
			{
				for (int i = 0; i < SizeI; i++)
				{
					buffer[border, i] = BORDER_INFO_H;
				}
			}

			// Corners
			buffer.Char[_borderLinesJ[0], _borderLinesI[0]] = '╔';
			buffer.Char[_borderLinesJ[1], _borderLinesI[0]] = '╠';
			buffer.Char[_borderLinesJ[2], _borderLinesI[0]] = '╚';
			buffer.Char[_borderLinesJ[0], _borderLinesI[1]] = '╗';
			buffer.Char[_borderLinesJ[1], _borderLinesI[1]] = '╣';
			buffer.Char[_borderLinesJ[2], _borderLinesI[1]] = '╝';
		}
	}
}
