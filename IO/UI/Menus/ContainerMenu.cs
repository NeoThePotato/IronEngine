using System.Diagnostics;
using System.Text;
using Game.Combat;
using Game.Items;
using Game.Items.Equipment;
using IO.Render;

namespace IO.UI.Menus
{
    class ContainerMenu : Menu
	{
		public SelectionMenu Menu
		{ get; private set; }
		public Container[] Containers
		{ get; private set; }
		public Unit PlayerUnit
		{ get; private set; }
		public int SelectedItemIndex
		{ get; private set; }
		public int SelectedContainerIndex
		{ get; private set; }
		public Item SelectedItem
		{ get; private set; }
		public bool ItemSelected
		{ get => SelectedItem != null; }
		public override int LengthJ
		{ get => Menu.LengthJ; }
		public override int LengthI
		{ get => Menu.LengthI; }
		public override bool Exit
		{ get; set; }

		public ContainerMenu(PlayerInputManager inputManager, GameUIManager parentUIManager, Unit playerUnit, params Container[] containers) : base(inputManager, parentUIManager)
		{
			PlayerUnit = playerUnit;
			var sb = new StringBuilder(50);

			foreach (var container in containers)
				sb.Append($"{container.Name} | ");
			Containers = containers;
			Menu = new SelectionMenu(inputManager, parentUIManager, null, GetStrings(), sb.ToString());
		}

		public override void Start()
		{
			Menu.Start();
		}

		public override string? Update()
		{
			bool itemSelected = Menu.Update() != null;
			
			Debug.Assert(!(itemSelected && Menu.Exit));

			if (Menu.Exit && !ItemSelected && !itemSelected) // Exit menu
			{
				Exit = true;
				return null;
			}
			else if (Menu.Exit && ItemSelected) // Release item #1 selection
			{
				SelectedItem = null;
				Menu.Continue();
			}
			else if (!ItemSelected && itemSelected) // Select item #1
			{
				SelectedCurrentItem();
			}
			else if (ItemSelected && itemSelected) // Select item #2
			{
				if (GetItemAtCursor() == SelectedItem) // Item #1 is #2
					ParentUIManager.StackNewMenu(MenuFactory.GetEquipmentMenu(InputManager, ParentUIManager, this, (Equipment)SelectedItem, PlayerUnit));
				else // Item #1 is not #2
					SwapSelectedWithCursor();
				SelectedItem = null;
			}
			RefreshMenuStrings();

			return null;
		}

		public Item? GetItemAtCursor()
		{
			return Containers[Menu.CursorI].Items[Menu.CursorJ];
		}

		public Item? GetItemAtSelection()
		{
			return Containers[SelectedContainerIndex].Items[SelectedItemIndex];
		}

		public Item? RemoveItemAtSelection()
		{
			Item? item = Containers[SelectedContainerIndex].RemoveItem(SelectedItemIndex);
			if (item != null)
				ParentUIManager.DataLog.WriteLine($"{item} was discarded");

			return item;
		}

		public void EquipSelectedOnUnit(Unit unit)
		{
			Debug.Assert(GetItemAtSelection() is Equipment);
			var equipment = (Equipment?)Containers[SelectedContainerIndex].RemoveItem(SelectedItemIndex);

			if (equipment != null)
				ParentUIManager.DataLog.WriteLine($"{unit} equipped the {equipment}");
			unit.Equip(ref equipment);
			Containers[SelectedContainerIndex].TryAddItem(equipment, SelectedItemIndex);
		}

		public override Renderer GetRenderer()
		{
			return new ContainerMenuRenderer(this);
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
