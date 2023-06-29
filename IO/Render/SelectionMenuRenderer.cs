using IO.UI.Menus;

namespace IO.Render
{
	class SelectionMenuRenderer : Renderer
	{
		private const byte HIGHLIGHTED_BG_COLOR = 240;
		private const int SPACE_BETWEEN_COLS = 1;

		private SelectionMenu Menu
		{ get; set; }
		public override int SizeJ
		{ get => Menu.DimJ; }
		public override int SizeI
		{ get => Menu.DimI * LengthPerString; }
		public int BodyOffsetJ
		{ get => Menu.HasTitle ? 1 : 0; }
		public int SelectionMenuOffsetJ
		{ get => BodyOffsetJ + (Menu.HasBody ? Body.Length : 0); }
		public int LengthPerString
		{ get => Menu.LongestString + SPACE_BETWEEN_COLS; }
		public string[] Body
		{ get; private set; }

		public SelectionMenuRenderer(SelectionMenu selectionMenu)
		{
			Menu = selectionMenu;
			Body = Menu.Body.Split('\n');
		}

		public override void Render(FrameBuffer buffer)
		{
			RenderTitle(buffer);
			RenderBody(new FrameBuffer(buffer, BodyOffsetJ, 0));
			RenderBaseMenu(new FrameBuffer(buffer, SelectionMenuOffsetJ, 0));

			if (Menu.CursorValidPosition)
				HighlightSelection(buffer);
		}

		private void RenderTitle(FrameBuffer buffer)
		{
			RenderTextSingleLine(buffer, Menu.Title, Menu.Title.Length);
		}

		private void RenderBody(FrameBuffer buffer)
		{
			var length = Body.Max()!;
			RenderText(buffer, Body, length.Length);
		}

		private void RenderBaseMenu(FrameBuffer buffer)
		{
			for (int col = 0; col < Menu.DimI; col++)
				RenderCol(buffer, col);
		}

		private void RenderCol(FrameBuffer buffer, int col)
		{
			var rowEnum = Enumerable.Range(0, Menu.DimJ).Select(row => Menu.Strings[row, col] != null ? Menu.Strings[row, col] : "");
			RenderText(new FrameBuffer(buffer, 0, LengthPerString * col), rowEnum, LengthPerString);
		}

		private void HighlightSelection(FrameBuffer buffer)
		{
			RenderTextSingleLine(new FrameBuffer(buffer, SelectionMenuOffsetJ+Menu.CursorJ, LengthPerString * Menu.CursorI), Menu.GetOptionAtCursor(), LengthPerString, COLOR_WHITE, HIGHLIGHTED_BG_COLOR);
		}
	}
}
