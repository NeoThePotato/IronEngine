namespace Game.Progression
{
	static class Leveling
	{
		public const int EXTRA_EXP_PER_LEVEL = 10;

		public static int GetExpAtLevel(int level)
		{
			return (EXTRA_EXP_PER_LEVEL * (level - 1));
		}

		public static int GetTotalExpToLevel(int level)
		{
			return (GetExpAtLevel(level) * level) / 2;
		}
	}
}
