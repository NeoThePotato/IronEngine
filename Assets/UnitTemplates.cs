using Game.Combat;
using Assets.EquipmentTemplates;

namespace Assets
{
    static class UnitTemplates
	{
		public static readonly Unit hero = new Unit("Hero", 50, 5, 0.2f, 0.5f, 0.5f, Weapons.rustedBlade, Armors.rustedBuckler, Armors.rustedChestplate);
		public static readonly Unit antiHero = new Unit("Anti-Hero", 45, 4, 0.15f, 0.45f, 0.55f, Weapons.rustedBlade, Armors.rustedBuckler, Armors.rustedChestplate);
		public static readonly Unit annoyingFly = new Unit("Annoying Fly", 1, 0, 0.8f, 0f, 1f, Weapons.nothing, Armors.nothing, Armors.nothing);
		public static readonly Unit slime = new Unit("Slime", 10, 1, 0.15f, 0.5f, 1f, Weapons.nothing, Armors.nothing, Armors.nothing);
		public static readonly Unit imp = new Unit("Imp", 20, 2, 0.3f, 0.2f, 0.8f, Weapons.magicWand, Armors.rustedBuckler, Armors.tatteredRags);
		public static readonly Unit fae = new Unit("Fae", 15, 1, 0.4f, 1f, 0.2f, Weapons.magicWand, Armors.nothing, Armors.tatteredRags);
		public static readonly Unit spawnOfTwilight = new Unit("Spawn of Twilight", 25, 2, 0.2f, 0.3f, 0.7f, Weapons.umbraSword, Armors.steelBuckler, Armors.blackRobes);
		public static readonly Unit invincibleArchdemon = new Unit("The Invincible Archdemon", 100, 3, 0f, 0f, 1f, Weapons.fieryGreatsword, Armors.towerShield, Armors.moltenArmor);
		public static readonly Unit tyrantKingClone = new Unit("Clone of The Tyrant King", 80, 5, 0.3f, 0.2f, 1f, Weapons.tyrantGauntletsBootleg, Armors.nothing, Armors.kingSlayerArmor);
		public static readonly Unit tyrantKing = new Unit("The Tyrant King", 200, 10, 0.4f, 0.3f, 1f, Weapons.tyrantGauntlets, Armors.nothing, Armors.godSlayerArmor);
	}
}
