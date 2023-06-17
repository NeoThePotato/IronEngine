using Game;
using Game.World;
using System.Diagnostics;

namespace IO.UI
{
	class ContainerMenuManager
	{
		public PlayerInputManager InputManager
		{ get => MenuManager.InputManager; }
		public MenuManager MenuManager
		{ get; private set; }
		public Container[] Containers
		{ get; private set; }
		public int SelectedItemJ
		{ get; private set; }
		public int SelectedItemI
		{ get; private set; }
		public Item? SelectedItem
		{ get; private set; }
		public bool ItemSelected
		{ get => SelectedItem != null; }
		public bool Exit
		{ get; private set; }

		public ContainerMenuManager(PlayerInputManager inputManager, params Container[] containers)
		{
			Containers = containers;
			MenuManager = new MenuManager(inputManager, GetStrings());
		}

		public void Update()
		{
			bool itemSelected = MenuManager.Update() != null;
			Debug.Assert(itemSelected && !MenuManager.Exit);

			if (MenuManager.Exit && !ItemSelected && !itemSelected)
			{
				Exit = true;
				return;
			}
			else if (MenuManager.Exit && ItemSelected)
			{
				SelectedItem = null;
				MenuManager.Continue();
			}
			else if (!ItemSelected && itemSelected)
			{
				SelectedCurrentItem();
			}
			else if (ItemSelected && itemSelected)
			{
				SwapSelectedWithCursor();
				RefreshMenuStrings();
				SelectedItem = null;
			}
		}

		private void SelectedCurrentItem()
		{
			SelectedItemJ = MenuManager.CursorJ;
			SelectedItemI = MenuManager.CursorI;
			SelectedItem = GetItemAtSelection();

			if (SelectedItem != null)
				Debug.Assert(SelectedItem.ToString() == MenuManager.GetOptionAtCursor());
		}

		private Item? GetItemAtCursor()
		{
			return Containers[MenuManager.CursorJ].Items[MenuManager.CursorI];
		}

		private Item? GetItemAtSelection()
		{
			return Containers[SelectedItemJ].Items[SelectedItemI];
		}

		private void SwapSelectedWithCursor()
		{
			var item1 = Containers[SelectedItemJ].RemoveItem(SelectedItemI);
			var item2 = Containers[MenuManager.CursorJ].RemoveItem(MenuManager.CursorI);
			Containers[SelectedItemJ].TryAddItem(item2, SelectedItemI);
			Containers[MenuManager.CursorJ].TryAddItem(item1, MenuManager.CursorI);
		}

		private void RefreshMenuStrings()
		{
			throw new NotImplementedException(); // TODO Implement
		}

		private int GetBiggestContainerSize()
		{
			int size = 0;

			foreach (var container in Containers)
				size = Math.Max(size, container.Capacity);

			return size;
		}

		private string?[,] GetStrings()
		{
			var strings = new string?[GetBiggestContainerSize(), Containers.Length];

			for (int j = 0; j < strings.GetLength(0); j++)
			{
				for (int i = 0; i < strings.GetLength(1); i++)
				{
					var item = Containers[j].Items[i];
					strings[j, i] = item != null? item.ToString() : "Empty";
				}
			}

			return strings;
		}
	}
}
