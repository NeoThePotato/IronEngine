using Game.Combat;
using static Assets.Templates.EntitiesVisualInfo;

namespace Assets.Templates
{
	static class UnitTemplates
	{
		public static readonly Unit hero =		new("Hero", 1, 5, 5, 5, 5, null, null, null, UNIT_PLAYER);
		public static readonly Unit antiHero =	new("Anti-Hero", 1, 5, 5, 5, 5, Weapons.rustedBlade, Shields.rustedBuckler, BodyArmors.rustedChestplate);
		public static readonly Unit slime =		new("Slime", 1, 2, 1, 1, 1, null, null, null);
		public static readonly Unit imp =		new("Imp", 1, 2, 2, 4, 3, Weapons.magicWand, Shields.rustedBuckler, BodyArmors.tatteredRags);
		public static readonly Unit fae =		new("Fae", 1, 2, 1, 4, 5, Weapons.magicWand, null, BodyArmors.tatteredRags);
		public static readonly Unit bandit =	new("Bandit", 2, 3, 3, 4, 4, Weapons.steelSword, Shields.rustedBuckler, BodyArmors.leatherArmor);
		public static readonly Unit mercenary =	new("Mercenary", 2, 4, 5, 5, 4, Weapons.scimitar, Shields.steelBuckler, BodyArmors.leatherArmor);
		public static readonly Unit archDemon =	new("Arch Demon", 3, 7, 6, 6, 6, Weapons.fieryGreatsword, Shields.towerShield, BodyArmors.moltenArmor);
		public static readonly Unit finalBoss =	new("Epitaph", 20, 15, 15, 15, 15, Weapons.bossWeapon, Shields.bossShield, BodyArmors.bossArmor);
	}
}
