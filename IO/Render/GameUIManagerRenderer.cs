using Game;
using Game.Combat;
using Game.Progression;
using IO.UI;

namespace IO.Render
{
	class GameUIManagerRenderer : Renderer
	{
		public const int SIDEBAR_WIDTH = 10;
		public const int METER_WIDTH = 2;
		public static readonly byte HP_COLOR = 1;
		public static readonly byte XP_COLOR = 11;
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
			RenderPlayerExp(new FrameBuffer(sideBarBuffer, 0, METER_WIDTH));
		}

		private void RenderPlayerHP(FrameBuffer buffer)
		{
			Unit player = (Unit)GameManager.LevelManager.PlayerEntity.Entity;
			float playerHPPercent = (float)player.CurrentHP / player.MaxHP;
			RenderMeter(buffer, playerHPPercent, HP_COLOR, BLANK_COLOR, "HP", METER_WIDTH);
		}

		private void RenderPlayerExp(FrameBuffer buffer)
		{
			Unit player = (Unit)GameManager.LevelManager.PlayerEntity.Entity;
			float playerXPPercent = 1f - ((float)player.ExpToNextLevel / Leveling.GetExpAtLevel(player.Level+1));
			RenderMeter(buffer, playerXPPercent, XP_COLOR, BLANK_COLOR, "XP", METER_WIDTH);
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
			RenderText(new FrameBuffer(buffer, SideBarSizeJ-1), text);
		}
	}
}
