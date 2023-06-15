namespace Game.World
{
	class Container : Entity
	{
		public override string Name
		{ get; }
		public int Capacity
		{ get; private set; }
		public int LockLevel
		{ get; private set; }
		public int NumberOfItems
		{ get; private set; }
		public Item?[] Items
		{ get; private set; }
		public bool Full
		{ get => NumberOfItems == Capacity; }
		public override EncounterManager.EncounterType EncounterType
		{ get => EncounterManager.EncounterType.Container; }

		public Container(string name, int capacity = 1, int lockLevel = 0)
		{
			Name = name;
			Capacity = capacity;
			LockLevel = lockLevel;
			Items = new Item[Capacity];
		}

		public Container(string name, Item item, int lockLevel = 0)
		{
			Name = name;
			Capacity = 1;
			LockLevel = lockLevel;
			Items = new Item[Capacity];
			TryAddItem(item);
		}

		public Container(string name, Item[] items, int lockLevel = 0)
		{
			Name = name;
			Capacity = items.Length;
			LockLevel = lockLevel;
			Items = new Item[Capacity];
			TryAddItems(items);
		}

		public bool TryAddItem(Item item)
		{
			if (item == null || Full)
			{
				return false;
			}
			else
			{
				AddItem(item);
				return true;
			}
		}

		public int TryAddItems(Item[] items)
		{
			int i;

			for (i = 0; i < items.Length;  i++)
			{
				if (!TryAddItem(items[i]))
					break;
			}

			return i;
		}

		public Item? RemoveItem(int index)
		{
			--NumberOfItems;
			return Items[index];
		}

		private void AddItem(Item item)
		{
			Items.Append(item);
			++NumberOfItems;
		}
	}
}
