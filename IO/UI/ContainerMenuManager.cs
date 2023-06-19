using System.Diagnostics;
using Game.Items;

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
		public int SelectedItemIndex
		{ get; private set; }
		public int SelectedContainerIndex
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
			
			Debug.Assert(!(itemSelected && MenuManager.Exit));

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

		public Item? GetItemAtCursor()
		{
			return Containers[MenuManager.CursorJ].Items[MenuManager.CursorI];
		}

		public Item? GetItemAtSelection()
		{
			return Containers[SelectedContainerIndex].Items[SelectedItemIndex];
		}

		private void SelectedCurrentItem()
		{
			SelectedItemIndex = MenuManager.CursorJ;
			SelectedContainerIndex = MenuManager.CursorI;
			SelectedItem = GetItemAtSelection();

			if (SelectedItem != null)
				Debug.Assert(SelectedItem.ToString() == MenuManager.GetOptionAtCursor());
		}

		private void SwapSelectedWithCursor()
		{
			var item1 = Containers[SelectedContainerIndex].RemoveItem(SelectedItemIndex);
			var item2 = Containers[MenuManager.CursorI].RemoveItem(MenuManager.CursorJ);
			Containers[SelectedContainerIndex].TryAddItem(item2, SelectedItemIndex);
			Containers[MenuManager.CursorI].TryAddItem(item1, MenuManager.CursorJ);
		}

		private void RefreshMenuStrings()
		{
			MenuManager.SetOptions(GetStrings());
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

			for (int containerIndex = 0; containerIndex < Containers.Length; containerIndex++)
			{
				for (int itemIndex = 0; itemIndex < Containers[containerIndex].Capacity; itemIndex++)
				{
					var item = Containers[containerIndex].Items[itemIndex];
					strings[itemIndex, containerIndex] = item != null? item.ToString() : "Empty";
				}
			}

			return strings;
		}
	}
}
