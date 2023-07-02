using Game.Items;
using Assets.Templates;

namespace Assets.Generators
{
	static class ItemGenerator
	{
		private static readonly Dictionary<Item, SpawnProfile> SPAWNABLE_ITEMS = new()
		{
			{Weapons.rustedBlade,				new SpawnProfile(1, 5)},
			{Weapons.steelSword,				new SpawnProfile(2, 4)},
			{Weapons.scimitar,					new SpawnProfile(4, 3)},
			{Weapons.bootlegExcalibur,			new SpawnProfile(5, 3)},
			{Weapons.realExcalibur,				new SpawnProfile(8, 2)},
			{Weapons.bootlegTyrantGauntlets,	new SpawnProfile(6, 3)},
			{Weapons.realTyrantGauntlets,		new SpawnProfile(10, 2)},
			{Weapons.theArk,					new SpawnProfile(13, 1)},
			{Shields.rustedBuckler,				new SpawnProfile(1, 5)},
			{Shields.steelBuckler,				new SpawnProfile(2, 5)},
			{Shields.towerShield,				new SpawnProfile(5, 4)},
			{Shields.bulwark,					new SpawnProfile(7, 3)},
			{Shields.heroShield,				new SpawnProfile(10, 2)},
			{Shields.calamityShield,			new SpawnProfile(12, 1)},
			{BodyArmors.rustedChestplate,		new SpawnProfile(1, 5)},
			{BodyArmors.tatteredRags,			new SpawnProfile(1, 5)},
			{BodyArmors.blackRobes,				new SpawnProfile(2, 4)},
			{BodyArmors.leatherArmor,			new SpawnProfile(3, 3)},
			{BodyArmors.moltenArmor,			new SpawnProfile(5, 3)},
			{BodyArmors.mithrilChainmail,		new SpawnProfile(7, 2)},
			{BodyArmors.kingSlayerArmor,		new SpawnProfile(10, 2)},
			{BodyArmors.godSlayerArmor,			new SpawnProfile(12, 1)},
		};

		public static Item? MakeItem(int level)
		{
			return EntityGenerator<Item>.MakeEntity(SPAWNABLE_ITEMS, level);
		}
	}
}
