using Game.Combat;
using Assets.EquipmentTemplates;
using static Assets.EntitiesVisualInfo;

namespace Assets
{
    static class UnitTemplates
	{
		public static readonly Unit hero = new Unit("Hero", 1, 50, 5, 0.2f, 0.5f, 0.5f, Weapons.rustedBlade, Armors.rustedBuckler, Armors.rustedChestplate, UNIT_PLAYER);
		public static readonly Unit antiHero = new Unit("Anti-Hero", 1, 45, 4, 0.15f, 0.45f, 0.55f, Weapons.rustedBlade, Armors.rustedBuckler, Armors.rustedChestplate);
		public static readonly Unit slime = new Unit("Slime", 1, 15, 1, 0.15f, 0.5f, 1f, Weapons.nothing, Armors.nothing, Armors.nothing);
		public static readonly Unit bandit = new Unit("Bandit", 3, 30, 2, 0.05f, 0.4f, 0.8f, Weapons.steelSword, Armors.rustedBuckler, Armors.leatherArmor);
		public static readonly Unit imp = new Unit("Imp", 2, 20, 2, 0.3f, 0.2f, 0.8f, Weapons.magicWand, Armors.rustedBuckler, Armors.tatteredRags);
		public static readonly Unit fae = new Unit("Fae", 2, 15, 1, 0.4f, 1f, 0.2f, Weapons.magicWand, Armors.nothing, Armors.tatteredRags);
		public static readonly Unit spawnOfTwilight = new Unit("Spawn of Twilight", 5, 25, 2, 0.2f, 0.3f, 0.7f, Weapons.umbraSword, Armors.steelBuckler, Armors.blackRobes);
	}
}
