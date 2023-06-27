using Game.World;
using static IO.Render.EntityRenderer;
using static Assets.EntitiesVisualInfo;

namespace Game.Items
{
    class Container : Entity
    {
        public override string Name
        { get; }
		public override int Level
		{ get; }
		public int Capacity
        { get; private set; }
        public int NumberOfItems
        { get; private set; }
        public Item?[] Items
        { get; private set; }
        public bool Empty
        { get => NumberOfItems == 0; }
        public bool Full
        { get => NumberOfItems == Capacity; }
        public override bool Passable
        { get => true; }
        public override bool Moveable
        { get => false; }
        public override bool MarkForDelete
        { get => Empty; }
        public override EncounterManager.EncounterType EncounterType
        { get => EncounterManager.EncounterType.Container; }
		public override VisualEntityInfo VisualInfo
		{ get => CHEST; }

		public Container(string name, int capacity = 1, int level = 1)
        {
            Name = name;
			Level = level;
			Capacity = capacity;
            Items = new Item[Capacity];
            NumberOfItems = 0;
		}

        public Container(string name, Item item, int level = 1)
        {
            Name = name;
			Level = level;
			Capacity = 1;
            Items = new Item[Capacity];
            TryAddItem(item);
        }

        public Container(string name, Item[] items, int level = 1)
        {
            Name = name;
			Level = level;
			Capacity = items.Length;
            Items = new Item[Capacity];
            TryAddItems(items);
        }

        public bool TryAddItem(Item item)
        {
            return TryAddItem(item, FindEmptySlot());
        }

        public bool TryAddItem(Item item, int index)
        {
            if (item == null || Items[index] != null || IndexOutOfBounds(index))
            {
                return false;
            }
            else
            {
                AddItem(item, index);

                return true;
            }
        }

        public int TryAddItems(Item[] items)
        {
            int i;

            for (i = 0; i < items.Length; i++)
            {
                if (!TryAddItem(items[i]))
                {
                    break;
                }
            }

            return i;
        }

        public Item? RemoveItem(int index)
        {
            if (IndexOutOfBounds(index) || Items[index] == null)
            {
                return null;
            }
            else
            {
                var item = Items[index];
                Items[index] = null;
                NumberOfItems--;

                return item;
            }
        }

        private void AddItem(Item item, int index)
        {
            Items[index] = item;
            NumberOfItems++;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IndexOutOfBounds(int index)
        {
            return 0 > index || Items.Length <= index;
        }

        private void UpdateNumberOfItems()
        {
            NumberOfItems = CountItemsInContainer();
		}

        private int CountItemsInContainer()
        {
            int itemsCount = 0;

            for (int i = 0; i < Capacity; i++)
            {
                if (Items[i] != null)
                    itemsCount++;
            }

            return itemsCount;
        }
    }
}
