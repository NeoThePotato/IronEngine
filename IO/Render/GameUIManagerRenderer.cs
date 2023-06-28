using Game;
using Game.Combat;
using IO.UI;

namespace IO.Render
{
	class GameUIManagerRenderer : Renderer
	{
		public const int SIDEBAR_WIDTH = 10;
		public static readonly byte HP_COLOR = 1;
		public static readonly byte BLANK_COLOR = 0;

		private GameUIManager UIManager
		{ get; set; }
		private GameManagerRenderer GameManagerRenderer
		{ get; set; }
		private Renderer? CurrentMenuRenderer
		{ get; set; }
		public override int SizeJ
		{ get => GameManagerRenderer.SizeJ; }
		public override int SizeI
		{ get => GameManagerRenderer.SizeI; }
		public int SideBarSizeJ
		{ get => GameManagerRenderer.LevelManagerRenderer.SizeJ; }
		public int SideBarSizeI
		{ get => SIDEBAR_WIDTH; }
		public GameManager GameManager
		{ get => UIManager.GameManager; }

		public GameUIManagerRenderer(GameUIManager uiManager, GameManagerRenderer gameManagerRenderer)
		{
			UIManager = uiManager;
			GameManagerRenderer = gameManagerRenderer;
		}

		public override void Render(FrameBuffer buffer)
		{
			RenderSideBar(buffer);
			RenderDataLog(buffer);

			if (UIManager.InMenu)
			{
				CurrentMenuRenderer = UIManager.GetCurrentMenu()!.GetRenderer();
				CurrentMenuRenderer.Render(buffer);
			}
		}

		private void RenderDataLog(FrameBuffer buffer)
		{
			var dataLogBuffer = new FrameBuffer(buffer, GameManagerRenderer.LevelManagerRenderer.SizeJ + 2, 1);
			RenderText(dataLogBuffer, UIManager.DataLog, GameManagerRenderer.LevelManagerRenderer.SizeI);
		}

		private void RenderSideBar(FrameBuffer buffer)
		{
			var sideBarBuffer = new FrameBuffer(buffer, 1, GameManagerRenderer.LevelManagerRenderer.SizeI + 2);
			RenderPlayerHP(sideBarBuffer);
		}

		private void RenderPlayerHP(FrameBuffer buffer)
		{
			Unit player = (Unit)GameManager.LevelManager.PlayerEntity.Entity;
			float playerHPPercent = (float)player.CurrentHP / player.MaxHP;
			RenderMeter(buffer, playerHPPercent, HP_COLOR, BLANK_COLOR, "HP", 2);
		}

		private void RenderMeter(FrameBuffer buffer, float percentage, byte fullColor, byte emptyColor, string text, int length = 1)
		{
			percentage = 1 - percentage;
			byte color;

			for (int j = 0; j < SideBarSizeJ; j++)
			{
				color = ((float)j / SideBarSizeJ >= percentage) ? fullColor : emptyColor;

				for (int i = 0; i < length; i++)
					buffer.Background[j, i] = color;
			}
			RenderText(new FrameBuffer(buffer, SideBarSizeJ-1), "HP");
		}
	}
}
