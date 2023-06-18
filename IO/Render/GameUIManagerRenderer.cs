using Game;
using IO.UI;
using System;

namespace IO.Render
{
	class GameUIManagerRenderer : Renderer
	{
		private GameUIManager UIManager
		{ get; set; }
		private GameManagerRenderer GameManagerRenderer
		{ get; set; }
		private MenuManagerRenderer InGameMenuRenderer
		{ get; set; }
		public ContainerMenuManagerRenderer? ContainerMenuManagerRenderer
		{ get; private set; }
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
			InGameMenuRenderer = new MenuManagerRenderer(UIManager.InGameMenu);
		}

		public override void Render(FrameBuffer buffer)
		{
			RenderDataLog(buffer);

			if (UIManager.StateMenu)
				RenderInGameMenu(buffer);
		}

		private void RenderInGameMenu(FrameBuffer buffer)
		{
			if (UIManager.StateInventoryMenu)
				RenderInventoryMenu(buffer);
			else
				InGameMenuRenderer.Render(buffer);
		}

		private void RenderInventoryMenu(FrameBuffer buffer)
		{
			ContainerMenuManagerRenderer = new ContainerMenuManagerRenderer(UIManager.ContainerMenuManager);
			ContainerMenuManagerRenderer.Render(buffer);
		}

		private void RenderDataLog(FrameBuffer buffer)
		{
			var dataLogBuffer = new FrameBuffer(buffer, GameManagerRenderer.LevelManagerRenderer.SizeJ + 2, 1);
			RenderText(dataLogBuffer, UIManager.DataLog, GameManagerRenderer.LevelManagerRenderer.SizeI);
		}
	}

}
