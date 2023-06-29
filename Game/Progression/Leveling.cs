namespace Game.Progression
{
	static class Leveling
	{
		public const int EXP_PER_LEVEL = 10;

		public static int GetExpAtLevel(int level)
		{
			return (level * EXP_PER_LEVEL);
		}

		public static int GetTotalExpToLevel(int level)
		{
			return (GetExpAtLevel(level) * level) / 2;
		}
	}
}
