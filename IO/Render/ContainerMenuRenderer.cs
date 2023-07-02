using IO.UI.Menus;

namespace IO.Render
{
	class ContainerMenuRenderer : Renderer
	{
		private const byte HIGHLIGHTED_BG_COLOR = 240;
		private const int SPACE_BETWEEN_COLS = 1;

		private ContainerMenu ContainerMenu
		{ get; set; }
		private SelectionMenu Menu
		{ get => ContainerMenu.Menu; }
		private SelectionMenuRenderer MenuManagerRenderer
		{ get; set; }
		public override int SizeJ
		{ get => Menu.DimJ; }
		public override int SizeI
		{ get => Menu.DimI * LengthPerString; }
		public int LengthPerString
		{ get => Menu.LongestString + SPACE_BETWEEN_COLS; }

		public ContainerMenuRenderer(ContainerMenu containerMenu)
		{
			ContainerMenu = containerMenu;
			MenuManagerRenderer = new SelectionMenuRenderer(Menu);
		}

		public override void Render(FrameBuffer buffer)
		{
			MenuManagerRenderer.Render(buffer);
			buffer = new FrameBuffer(buffer, MenuManagerRenderer.SelectionMenuOffsetJ, 0);

			if (ContainerMenu.ItemSelected)
				HighlightSelection(buffer);
		}
		
		private void HighlightSelection(FrameBuffer buffer)
		{
			RenderTextSingleLine(new FrameBuffer(buffer, ContainerMenu.SelectedItemIndex, LengthPerString * ContainerMenu.SelectedContainerIndex), ContainerMenu.GetItemAtSelection()!.ToString(), LengthPerString, COLOR_WHITE, HIGHLIGHTED_BG_COLOR);
		}
	}
}
