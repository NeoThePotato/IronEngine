using Game.Items;

namespace Assets.Generators
{
	static class ItemGenerator
	{
		private static readonly Dictionary<Item, SpawnProfile> SPAWNABLE_ITEMS = new Dictionary<Item, SpawnProfile>()
		{
			{EquipmentTemplates.Weapons.rustedBlade,	new SpawnProfile(1, 5)},
/*			{Item.bandit,			new SpawnProfile(2, 5)},
			{Item.imp,				new SpawnProfile(2, 6)},
			{Item.fae,				new SpawnProfile(2, 6)},
			{Item.spawnOfTwilight,	new SpawnProfile(4, 2)},
			{Item.antiHero,			new SpawnProfile(10, 2)},*/
		};

		public static Item MakeItem(int level)
		{
			var item = EntityGenerator<Item>.MakeEntity(SPAWNABLE_ITEMS, level);

			return item;
		}
	}
}
