using System.Diagnostics;
using Game.Items;

namespace IO.UI.Menus
{
    class ContainerMenu : Menu
	{
		public SelectionMenu Menu
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
		public override int LengthJ
		{ get => Menu.LengthJ; }
		public override int LengthI
		{ get => Menu.LengthI; }
		public override bool Exit
		{ get; set; }

		public ContainerMenu(PlayerInputManager inputManager, params Container[] containers) : base(inputManager)
		{
			Containers = containers;
			Menu = new SelectionMenu(inputManager, null, GetStrings());
		}

		public override void Start()
		{
			Menu.Start();
		}

		public override string? Update()
		{
			bool itemSelected = Menu.Update() != null;
			
			Debug.Assert(!(itemSelected && Menu.Exit));

			if (Menu.Exit && !ItemSelected && !itemSelected)
			{
				Exit = true;
				return null;
			}
			else if (Menu.Exit && ItemSelected)
			{
				SelectedItem = null;
				Menu.Continue();
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

			return null;
		}

		public Item? GetItemAtCursor()
		{
			return Containers[Menu.CursorJ].Items[Menu.CursorI];
		}

		public Item? GetItemAtSelection()
		{
			return Containers[SelectedContainerIndex].Items[SelectedItemIndex];
		}

		private void SelectedCurrentItem()
		{
			SelectedItemIndex = Menu.CursorJ;
			SelectedContainerIndex = Menu.CursorI;
			SelectedItem = GetItemAtSelection();

			if (SelectedItem != null)
				Debug.Assert(SelectedItem.ToString() == Menu.GetOptionAtCursor());
		}

		private void SwapSelectedWithCursor()
		{
			var item1 = Containers[SelectedContainerIndex].RemoveItem(SelectedItemIndex);
			var item2 = Containers[Menu.CursorI].RemoveItem(Menu.CursorJ);
			Containers[SelectedContainerIndex].TryAddItem(item2, SelectedItemIndex);
			Containers[Menu.CursorI].TryAddItem(item1, Menu.CursorJ);
		}

		private void RefreshMenuStrings()
		{
			Menu.SetOptions(GetStrings());
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
