namespace Game.Progression
{
	struct GrowthProfile
	{
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
	}
}
