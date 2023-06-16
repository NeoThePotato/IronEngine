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

		public override void Render(ref FrameBuffer frameBuffer)
		{
			RenderBaseMenu(ref frameBuffer);

			if (MenuManager.CursorValidPosition)
				HighlightSelection(ref frameBuffer);
		}

		private void RenderBaseMenu(ref FrameBuffer frameBuffer)
		{
			for (int col = 0; col < MenuManager.DimI; col++)
				RenderCol(ref frameBuffer, col);
		}

		private void RenderCol(ref FrameBuffer frameBuffer, int col)
		{
			var fb = new FrameBuffer(frameBuffer, 0, LengthPerString * col);
			var rowEnum = Enumerable.Range(0, MenuManager.DimJ).Select(row => MenuManager.Options[row, col] != null ? MenuManager.Options[row, col] : "");
			RenderText(ref fb, rowEnum, LengthPerString);
		}

		private void HighlightSelection(ref FrameBuffer frameBuffer)
		{
			var fb = new FrameBuffer(frameBuffer, MenuManager.CursorJ, LengthPerString * MenuManager.CursorI);
			RenderText(ref fb, MenuManager.GetOptionAtCursor(), LengthPerString, COLOR_WHITE, HIGHLIGHTED_BG_COLOR);
		}
	}
}
