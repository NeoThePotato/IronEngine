using IO.UI.Menus;

namespace IO.Render
{
	class SelectionMenuRenderer : Renderer
	{
		private const byte HIGHLIGHTED_BG_COLOR = 240;
		private const int SPACE_BETWEEN_COLS = 1;

		private SelectionMenu Menu
		{  get; set; }
		public override int SizeJ
		{ get => Menu.DimJ; }
		public override int SizeI
		{ get => Menu.DimI * LengthPerString; }
		public int LengthPerString
		{ get => Menu.LongestString + SPACE_BETWEEN_COLS; }

		public SelectionMenuRenderer(SelectionMenu menuManager)
		{
			Menu = menuManager;
		}

		public override void Render(FrameBuffer frameBuffer)
		{
			RenderBaseMenu(frameBuffer);

			if (Menu.CursorValidPosition)
				HighlightSelection(frameBuffer);
		}

		private void RenderBaseMenu(FrameBuffer frameBuffer)
		{
			for (int col = 0; col < Menu.DimI; col++)
				RenderCol(frameBuffer, col);
		}

		private void RenderCol(FrameBuffer frameBuffer, int col)
		{
			var rowEnum = Enumerable.Range(0, Menu.DimJ).Select(row => Menu.Options[row, col] != null ? Menu.Options[row, col] : "");
			RenderText(new FrameBuffer(frameBuffer, 0, LengthPerString * col), rowEnum, LengthPerString);
		}

		private void HighlightSelection(FrameBuffer frameBuffer)
		{
			RenderText(new FrameBuffer(frameBuffer, Menu.CursorJ, LengthPerString * Menu.CursorI), Menu.GetOptionAtCursor(), LengthPerString, COLOR_WHITE, HIGHLIGHTED_BG_COLOR);
		}
	}
}
