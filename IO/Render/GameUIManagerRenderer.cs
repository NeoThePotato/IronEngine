using Game;
using IO.UI;

namespace IO.Render
{
	class GameUIManagerRenderer : Renderer
	{
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
		public GameManager GameManager
		{ get => UIManager.GameManager; }

		public GameUIManagerRenderer(GameUIManager uiManager, GameManagerRenderer gameManagerRenderer)
		{
			UIManager = uiManager;
			GameManagerRenderer = gameManagerRenderer;
		}

		public override void Render(FrameBuffer buffer)
		{
			RenderDataLog(buffer);

			if (UIManager.InMenu)
			{
				CurrentMenuRenderer = UIManager.GetCurrentMenu().GetRenderer();
				CurrentMenuRenderer.Render(buffer);
			}
		}

		private void RenderDataLog(FrameBuffer buffer)
		{
			var dataLogBuffer = new FrameBuffer(buffer, GameManagerRenderer.LevelManagerRenderer.SizeJ + 2, 1);
			RenderText(dataLogBuffer, UIManager.DataLog, GameManagerRenderer.LevelManagerRenderer.SizeI);
		}
	}

}
