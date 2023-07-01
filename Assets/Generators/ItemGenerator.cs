using Game.Items;

namespace Assets.Generators
{
	static class ItemGenerator
	{
		private static readonly Dictionary<Item, SpawnProfile> SPAWNABLE_ITEMS = new Dictionary<Item, SpawnProfile>()
		{
			{EquipmentTemplates.Weapons.rustedBlade,			new SpawnProfile(1, 5)},
			{EquipmentTemplates.Weapons.steelSword,				new SpawnProfile(2, 4)},
			{EquipmentTemplates.Weapons.scimitar,				new SpawnProfile(4, 3)},
			{EquipmentTemplates.Weapons.bootlegExcalibur,		new SpawnProfile(5, 3)},
			{EquipmentTemplates.Weapons.realExcalibur,			new SpawnProfile(8, 2)},
			{EquipmentTemplates.Weapons.bootlegTyrantGauntlets,	new SpawnProfile(6, 3)},
			{EquipmentTemplates.Weapons.realTyrantGauntlets,	new SpawnProfile(10, 2)},
			{EquipmentTemplates.Weapons.theArk,					new SpawnProfile(13, 1)},
			{EquipmentTemplates.Shields.rustedBuckler,			new SpawnProfile(1, 5)},
			{EquipmentTemplates.Shields.steelBuckler,			new SpawnProfile(2, 5)},
			{EquipmentTemplates.Shields.towerShield,			new SpawnProfile(5, 4)},
			{EquipmentTemplates.Shields.bulwark,				new SpawnProfile(7, 3)},
			{EquipmentTemplates.Shields.heroShield,				new SpawnProfile(10, 2)},
			{EquipmentTemplates.Shields.calamityShield,			new SpawnProfile(12, 1)},
			{EquipmentTemplates.BodyArmors.rustedChestplate,	new SpawnProfile(1, 5)},
			{EquipmentTemplates.BodyArmors.tatteredRags,		new SpawnProfile(1, 5)},
			{EquipmentTemplates.BodyArmors.blackRobes,			new SpawnProfile(2, 4)},
			{EquipmentTemplates.BodyArmors.leatherArmor,		new SpawnProfile(3, 3)},
			{EquipmentTemplates.BodyArmors.moltenArmor,			new SpawnProfile(5, 3)},
			{EquipmentTemplates.BodyArmors.mithrilChainmail,	new SpawnProfile(7, 2)},
			{EquipmentTemplates.BodyArmors.kingSlayerArmor,		new SpawnProfile(10, 2)},
			{EquipmentTemplates.BodyArmors.godSlayerArmor,		new SpawnProfile(12, 1)},
		};

		public static Item? MakeItem(int level)
		{
			return EntityGenerator<Item>.MakeEntity(SPAWNABLE_ITEMS, level);
		}
	}
}
