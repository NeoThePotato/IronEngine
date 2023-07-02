using Game.Items.Equipment;

namespace Assets.Templates
{
	static class Weapons
	{
		public static readonly Weapon nothing =					new("", 0);
		public static readonly Weapon rustedBlade =				new("Rusted Blade", 1);
		public static readonly Weapon steelSword =				new("Steel Sword", 2);
		public static readonly Weapon scimitar =				new("Scimitar", 3);
		public static readonly Weapon bootlegExcalibur =		new("Bootleg Excalibur", 4);
		public static readonly Weapon fieryGreatsword =			new("Fiery Greatsword", 5);
		public static readonly Weapon bootlegTyrantGauntlets =	new("Bootleg Tyrant Gauntlets", 5);
		public static readonly Weapon realExcalibur =			new("Excalibur", 7);
		public static readonly Weapon magicWand =				new("Magic Wand", 1);
		public static readonly Weapon realTyrantGauntlets =		new("Tyrant Gauntlets", 10);
		public static readonly Weapon theArk =					new("Ark of The Everything", 15);
		public static readonly Weapon bossWeapon =				new("Snake's Maw", 10);
	}

	static class Shields
	{
		public static readonly Shield nothing =			new("", 0);
		public static readonly Shield rustedBuckler =	new("Rusted Buckler", 1);
		public static readonly Shield steelBuckler =	new("Steel Buckler", 2);
		public static readonly Shield towerShield =		new("Tower Shield", 3);
		public static readonly Shield bulwark =			new("Bulwark", 4);
		public static readonly Shield heroShield =		new("Shield of the Hero", 6);
		public static readonly Shield calamityShield =	new("Shield of Calamity", 9);
		public static readonly Shield bossShield =		new("Basilisk's Gaze", 8);
	}

	static class BodyArmors
	{
		public static readonly BodyArmor nothing =			new("", 0);
		public static readonly BodyArmor rustedChestplate =	new("Rusted Chestplate", 1);
		public static readonly BodyArmor tatteredRags =		new("Tattered Rags", 1);
		public static readonly BodyArmor blackRobes =		new("Black Robes", 2);
		public static readonly BodyArmor leatherArmor =		new("Leather Armor", 2);
		public static readonly BodyArmor moltenArmor =		new("Molten Armor", 4);
		public static readonly BodyArmor mithrilChainmail =	new("Mithril Chainmail", 5);
		public static readonly BodyArmor kingSlayerArmor =	new("King-Slayer Armor", 6);
		public static readonly BodyArmor godSlayerArmor =	new("God-Slayer Armor", 10);
		public static readonly BodyArmor bossArmor =		new("Serpent's Scales", 9);
	}
}
