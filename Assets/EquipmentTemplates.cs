using Game.Items.Equipment;

namespace Assets.EquipmentTemplates
{
    static class Weapons
	{
		public static readonly Weapon nothing = new Weapon("", 0);
		public static readonly Weapon rustedBlade = new Weapon("Rusted Blade", 1);
		public static readonly Weapon steelSword = new Weapon("Steel Sword", 2);
		public static readonly Weapon umbraSword = new Weapon("Umbra Sword", 3);
		public static readonly Weapon fieryGreatsword = new Weapon("Fiery Greatsword", 5);
		public static readonly Weapon swordExcalibur = new Weapon("Excalibur", 7);
		public static readonly Weapon magicWand = new Weapon("Magic Wand", 1);
		public static readonly Weapon tyrantGauntletsBootleg = new Weapon("Bootleg Tyrant Gauntlets", 4);
		public static readonly Weapon tyrantGauntlets = new Weapon("Tyrant Gauntlets", 10);
	}

	static class Armors
	{
		public static readonly Armor nothing = new Armor("", 0);

		// Shields
		public static readonly Armor rustedBuckler = new Armor("Rusted Buckler", 1);
		public static readonly Armor steelBuckler = new Armor("Steel Buckler", 2);
		public static readonly Armor towerShield = new Armor("Tower Shield", 3);
		public static readonly Armor heroShield = new Armor("Hero Shield", 4);

		// Body Armor
		public static readonly Armor rustedChestplate = new Armor("Rusted Chestplate", 1);
		public static readonly Armor tatteredRags = new Armor("Tattered Rags", 1);
		public static readonly Armor blackRobes = new Armor("Black Robes", 2);
		public static readonly Armor leatherArmor = new Armor("Leather Armor", 2);
		public static readonly Armor moltenArmor = new Armor("Molten Armor", 4);
		public static readonly Armor mithrilChainmail = new Armor("Mithril Chainmail", 5);
		public static readonly Armor kingSlayerArmor = new Armor("King-Slayer Armor", 6);
		public static readonly Armor godSlayerArmor = new Armor("God-Slayer Armor", 15);
	}
}
