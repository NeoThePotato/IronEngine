using Game.Items.Equipment;

namespace Assets.EquipmentTemplates
{
    static class Weapons
	{
		public static readonly Weapon nothing = new Weapon("", 0);
		public static readonly Weapon rustedBlade = new Weapon("Rusted Blade", 1);
		public static readonly Weapon steelSword = new Weapon("Steel Sword", 2);
		public static readonly Weapon scimitar = new Weapon("Scimitar", 3);
		public static readonly Weapon bootlegExcalibur = new Weapon("Bootleg Excalibur", 4);
		public static readonly Weapon fieryGreatsword = new Weapon("Fiery Greatsword", 5);
		public static readonly Weapon bootlegTyrantGauntlets = new Weapon("Bootleg Tyrant Gauntlets", 5);
		public static readonly Weapon realExcalibur = new Weapon("Excalibur", 7);
		public static readonly Weapon magicWand = new Weapon("Magic Wand", 1);
		public static readonly Weapon realTyrantGauntlets = new Weapon("Tyrant Gauntlets", 10);
		public static readonly Weapon theArk = new Weapon("Ark of The Everything", 15);
	}

	static class Shields
	{
		public static readonly Shield nothing = new Shield("", 0);
		public static readonly Shield rustedBuckler = new Shield("Rusted Buckler", 1);
		public static readonly Shield steelBuckler = new Shield("Steel Buckler", 2);
		public static readonly Shield towerShield = new Shield("Tower Shield", 3);
		public static readonly Shield bulwark = new Shield("Bulwark", 4);
		public static readonly Shield heroShield = new Shield("Shield of the Hero", 6);
		public static readonly Shield calamityShield = new Shield("Shield of Calamity", 9);
	}

	static class BodyArmors
	{
		public static readonly BodyArmor nothing = new BodyArmor("", 0);
		public static readonly BodyArmor rustedChestplate = new BodyArmor("Rusted Chestplate", 1);
		public static readonly BodyArmor tatteredRags = new BodyArmor("Tattered Rags", 1);
		public static readonly BodyArmor blackRobes = new BodyArmor("Black Robes", 2);
		public static readonly BodyArmor leatherArmor = new BodyArmor("Leather Armor", 2);
		public static readonly BodyArmor moltenArmor = new BodyArmor("Molten Armor", 4);
		public static readonly BodyArmor mithrilChainmail = new BodyArmor("Mithril Chainmail", 5);
		public static readonly BodyArmor kingSlayerArmor = new BodyArmor("King-Slayer Armor", 6);
		public static readonly BodyArmor godSlayerArmor = new BodyArmor("God-Slayer Armor", 10);
	}
}
