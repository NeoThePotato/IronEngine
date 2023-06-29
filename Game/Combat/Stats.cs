using Game.World;

namespace Game.Combat
{
	struct Stats
	{
		#region BASE_STATS
		public int TotalBaseStats
		{ get => Vitality + Strength + Speed + Intelligence; }
		public int Vitality
		{ get; private set; }
		public int Strength
		{ get; private set; }
		public int Speed
		{ get; private set; }
		public int Intelligence
		{ get; private set; }
		#endregion
		#region EXTRA_STATS
		public int HP
		{ get => Utility.ClampMin(10 * Vitality + 2 * Strength + TotalBaseStats / 2, 1); }
		public int BaseDamage
		{ get => Strength / 2 + Speed / 5 + TotalBaseStats / 10; }
		public float BaseEvasion
		{ get => Utility.ClampMax(0.02f * Speed + 0.01f * TotalBaseStats, 1f); }
		public float EvasionDecay
		{ get => Utility.ClampRange(0.7f / (float)Math.Sqrt(Speed + TotalBaseStats / 5), 0f, 1f); }
		public float BaseHealingPower
		{ get => Utility.ClampMax(0.04f * (Vitality + Intelligence), 1f); }
		public float HealingPowerDecay
		{ get => Utility.ClampRange(1f / (float)Math.Sqrt(Vitality + Intelligence), 0f, 1f); }
		public int MovementSpeed
		{ get => Utility.ClampRange((int)(((Speed / 2f + TotalBaseStats / 5f) * Point2D.POINTS_PER_TILE) / 32), 16, Point2D.POINTS_PER_TILE); }
		public int DetectionRange
		{ get => (int)((Intelligence / 5f + TotalBaseStats / 10f) * Point2D.POINTS_PER_TILE); }
		#endregion

		public Stats(int vitality, int strength, int speed, int intelligence)
		{
			Vitality = vitality;
			Strength = strength;
			Speed = speed;
			Intelligence = intelligence;
		}

		public Stats(Stats other) : this(other.Vitality, other.Strength, other.Speed, other.Intelligence)
		{

		}

		public Stats(int total) : this(0, 0, 0, 0)
		{
			UpgradeRandomStat(total);
		}

		public void UpgradeStat(Stat stat, int by = 1)
		{
			switch (stat)
			{
				case Stat.VIT:
					Vitality += by;
					break;
				case Stat.STR:
					Strength += by;
					break;
				case Stat.SPD:
					Speed += by;
					break;
				case Stat.INT:
					Intelligence += by;
					break;
			}
		}

		public void UpgradeRandomStat(int total = 1)
		{
			while (total > 0)
			{
				UpgradeStat(GetRandomStat());
				total--;
			}
		}

		public static Stat GetRandomStat()
		{
			return (Stat)(Random.Shared.Next(0, Enum.GetNames(typeof(Stat)).Length));
		}

		public static Stats operator +(Stats s)
			=> s;

		public static Stats operator -(Stats s)
			=> new Stats(-s.Vitality, -s.Strength, -s.Speed, -s.Intelligence);

		public static Stats operator +(Stats s1, Stats s2)
			=> new Stats(s1.Vitality + s2.Vitality, s1.Strength + s2.Strength, s1.Speed + s2.Speed, s1.Intelligence + s2.Intelligence);

		public static Stats operator -(Stats s1, Stats s2)
			=> new Stats(s1.Vitality - s2.Vitality, s1.Strength - s2.Strength, s1.Speed - s2.Speed, s1.Intelligence - s2.Intelligence);

		public override string ToString()
		{
			return $"VIT: {Vitality}\nSTR: {Strength}\nSPD: {Speed}\nINT: {Intelligence}";
		}

		public enum Stat
		{
			VIT,
			STR,
			SPD,
			INT,
		}
	}
}
