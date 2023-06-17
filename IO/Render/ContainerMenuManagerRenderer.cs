using IO.UI;

namespace IO.Render
{
	class ContainerMenuManagerRenderer : Renderer
	{
		private const byte HIGHLIGHTED_BG_COLOR = 240;
		private const int SPACE_BETWEEN_COLS = 1;
		private ContainerMenuManager ContainerMenuManager
		{ get; set; }
		private MenuManager MenuManager
		{ get => ContainerMenuManager.MenuManager; }
		private MenuManagerRenderer MenuManagerRenderer
		{ get; set; }
		public override int SizeJ
		{ get => MenuManager.DimJ; }
		public override int SizeI
		{ get => MenuManager.DimI * LengthPerString; }
		public int LengthPerString
		{ get => MenuManager.LongestString + SPACE_BETWEEN_COLS; }

		public ContainerMenuManagerRenderer(ContainerMenuManager containerMenuManager)
		{
			ContainerMenuManager = containerMenuManager;
			MenuManagerRenderer = new MenuManagerRenderer(MenuManager);

		}

		public override void Render(FrameBuffer frameBuffer)
		{
			MenuManagerRenderer.Render(frameBuffer);

			if (ContainerMenuManager.ItemSelected)
				HighlightSelection(frameBuffer);
		}
		
		private void HighlightSelection(FrameBuffer frameBuffer)
		{
			RenderText(new FrameBuffer(frameBuffer, ContainerMenuManager.SelectedItemIndex, LengthPerString * ContainerMenuManager.SelectedContainerIndex), MenuManager.GetOptionAtCursor(), LengthPerString, COLOR_WHITE, HIGHLIGHTED_BG_COLOR);
		}
	}
}
