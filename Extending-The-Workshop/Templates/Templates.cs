using Combat;
using Combat.Equipment;

namespace Templates
{

	static class Units
	{
		public static Unit hero = new Unit("Hero", 50, 5, 0.2f, 0.5f, 0.5f, Weapons.rustedBlade, Armors.rustedBuckler, Armors.rustedChestplate);
		public static Unit antiHero = new Unit("Anti-Hero", 45, 4, 0.15f, 0.45f, 0.55f, Weapons.rustedBlade, Armors.rustedBuckler, Armors.rustedChestplate);
		public static Unit annoyingFly = new Unit("Annoying Fly", 1, 0, 0.8f, 0f, 1f, Weapons.nothing, Armors.nothing, Armors.nothing);
		public static Unit slime = new Unit("Slime", 10, 1, 0.15f, 0.5f, 1f, Weapons.nothing, Armors.nothing, Armors.nothing);
		public static Unit imp = new Unit("Imp", 20, 2, 0.3f, 0.2f, 0.8f, Weapons.magicWand, Armors.rustedBuckler, Armors.tatteredRags);
		public static Unit fae = new Unit("Fae", 15, 1, 0.4f, 1f, 0.2f, Weapons.magicWand, Armors.nothing, Armors.tatteredRags);
		public static Unit spawnOfTwilight = new Unit("Spawn of Twilight", 25, 2, 0.2f, 0.3f, 0.7f, Weapons.umbraSword, Armors.steelBuckler, Armors.blackRobes);
		public static Unit invincibleArchdemon = new Unit("The Invincible Archdemon", 100, 3, 0f, 0f, 1f, Weapons.fieryGreatsword, Armors.towerShield, Armors.moltenArmor);
		public static Unit tyrantKingClone = new Unit("Clone of The Tyrant King", 80, 5, 0.3f, 0.2f, 1f, Weapons.tyrantGauntletsBootleg, Armors.nothing, Armors.kingSlayerArmor);
		public static Unit tyrantKing = new Unit("The Tyrant King", 200, 10, 0.4f, 0.3f, 1f, Weapons.tyrantGauntlets, Armors.nothing, Armors.godSlayerArmor);
	}

	static class Weapons
	{
		public static Weapon nothing = new Weapon("", 0);
		public static Weapon rustedBlade = new Weapon("Rusted Blade", 1);
		public static Weapon steelSword = new Weapon("Steel Sword", 2);
		public static Weapon umbraSword = new Weapon("Umbra Sword", 3);
		public static Weapon fieryGreatsword = new Weapon("Fiery Greatsword", 5);
		public static Weapon swordExcalibur = new Weapon("Excalibur", 7);
		public static Weapon magicWand = new Weapon("Magic Wand", 1);
		public static Weapon tyrantGauntletsBootleg = new Weapon("Bootleg Tyrant Gauntlets", 4);
		public static Weapon tyrantGauntlets = new Weapon("Tyrant Gauntlets", 10);
	}

	static class Armors
	{
		public static Armor nothing = new Armor("", 0);

		// Shields
		public static Armor rustedBuckler = new Armor("Rusted Buckler", 1);
		public static Armor steelBuckler = new Armor("Steel Buckler", 2);
		public static Armor towerShield = new Armor("Tower Shield", 3);
		public static Armor heroShield = new Armor("Hero Shield", 4);

		// Body Armor
		public static Armor rustedChestplate = new Armor("Rusted Chestplate", 1);
		public static Armor tatteredRags = new Armor("Tattered Rags", 1);
		public static Armor blackRobes = new Armor("Black Robes", 2);
		public static Armor leatherArmor = new Armor("Leather Armor", 2);
		public static Armor moltenArmor = new Armor("Molten Armor", 4);
		public static Armor mithrilChainmail = new Armor("Mithril Chainmail", 5);
		public static Armor kingSlayerArmor = new Armor("King-Slayer Armor", 6);
		public static Armor godSlayerArmor = new Armor("God-Slayer Armor", 15);
	}

}
