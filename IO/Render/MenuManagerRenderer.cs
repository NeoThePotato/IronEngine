using IO.UI;

namespace IO.Render
{
	class MenuManagerRenderer : Renderer
	{
		private const byte HIGHLIGHTED_BG_COLOR = 240;
		private const int SPACE_BETWEEN_COLS = 1;

		private MenuManager MenuManager
		{  get; set; }
		public override int SizeJ
		{ get => MenuManager.DimJ; }
		public override int SizeI
		{ get => MenuManager.DimI * LengthPerString; }
		public int LengthPerString
		{ get => MenuManager.LongestString + SPACE_BETWEEN_COLS; }

		public MenuManagerRenderer(MenuManager menuManager)
		{
			MenuManager = menuManager;
		}

		public override void Render(FrameBuffer frameBuffer)
		{
			RenderBaseMenu(frameBuffer);

			if (MenuManager.CursorValidPosition)
				HighlightSelection(frameBuffer);
		}

		private void RenderBaseMenu(FrameBuffer frameBuffer)
		{
			for (int col = 0; col < MenuManager.DimI; col++)
				RenderCol(frameBuffer, col);
		}

		private void RenderCol(FrameBuffer frameBuffer, int col)
		{
			var rowEnum = Enumerable.Range(0, MenuManager.DimJ).Select(row => MenuManager.Options[row, col] != null ? MenuManager.Options[row, col] : "");
			RenderText(new FrameBuffer(frameBuffer, 0, LengthPerString * col), rowEnum, LengthPerString);
		}

		private void HighlightSelection(FrameBuffer frameBuffer)
		{
			RenderText(new FrameBuffer(frameBuffer, MenuManager.CursorJ, LengthPerString * MenuManager.CursorI), MenuManager.GetOptionAtCursor(), LengthPerString, COLOR_WHITE, HIGHLIGHTED_BG_COLOR);
		}
	}
}
