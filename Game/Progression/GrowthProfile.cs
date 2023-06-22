using Game.Combat;

namespace Game.Progression
{
	struct GrowthProfile
	{
		public const int STATS_COUNT = 5;

		public readonly int maxHP;
		public readonly int strength;
		public readonly float evasion;
		public readonly float maxHealingPower;
		public readonly float healingPowerDecay;

		public GrowthProfile(int maxHP, int strength, float evasion, float maxHealingPower, float healingPowerDecay)
		{
			this.maxHP = maxHP;
			this.strength = strength;
			this.evasion = evasion;
			this.maxHealingPower = maxHealingPower;
			this.healingPowerDecay = healingPowerDecay;
		}

		public void GrowUnit(ref Unit unit, int levels)
		{
			GrowUnit(ref unit, levels, this);
		}

		public static void GrowUnit(ref Unit unit, int levels, GrowthProfile growthProfile)
		{
			while (levels > 0)
			{
				unit.UpgradeStat(GetRandomStat(), growthProfile);
				levels--;
			}
		}

		public static UnitStat GetRandomStat()
		{
			return (UnitStat)(Random.Shared.Next(0, Enum.GetNames(typeof(UnitStat)).Length));
		}
	}
}
