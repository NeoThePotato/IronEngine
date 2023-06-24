using Game.Items;

namespace Assets.Generators
{
	static class ItemGenerator
	{
		private static readonly Dictionary<Item, SpawnProfile> SPAWNABLE_ITEMS = new Dictionary<Item, SpawnProfile>()
		{
			{EquipmentTemplates.Weapons.rustedBlade,		new SpawnProfile(1, 5)},
			{EquipmentTemplates.Weapons.steelSword,			new SpawnProfile(2, 4)},
			{EquipmentTemplates.Weapons.umbraSword,			new SpawnProfile(5, 3)},
			{EquipmentTemplates.Weapons.swordExcalibur,		new SpawnProfile(10, 2)},
			{EquipmentTemplates.Armors.rustedBuckler,		new SpawnProfile(1, 5)},
			{EquipmentTemplates.Armors.steelBuckler,		new SpawnProfile(2, 4)},
			{EquipmentTemplates.Armors.towerShield,			new SpawnProfile(5, 3)},
			{EquipmentTemplates.Armors.heroShield,			new SpawnProfile(10, 2)},
			{EquipmentTemplates.Armors.rustedChestplate,    new SpawnProfile(1, 5)},
			{EquipmentTemplates.Armors.tatteredRags,		new SpawnProfile(1, 5)},
			{EquipmentTemplates.Armors.blackRobes,			new SpawnProfile(2, 4)},
			{EquipmentTemplates.Armors.leatherArmor,		new SpawnProfile(3, 3)},
			{EquipmentTemplates.Armors.moltenArmor,			new SpawnProfile(5, 2)},
			{EquipmentTemplates.Armors.mithrilChainmail,	new SpawnProfile(10, 1)},
		};

		public static Item? MakeItem(int level)
		{
			return EntityGenerator<Item>.MakeEntity(SPAWNABLE_ITEMS, level);
		}
	}
}
