namespace Assets.Generators
{
	struct SpawnProfile
	{
		public int minLevel;
		public int relativeSpawnChance;

		public SpawnProfile(int minLevel, int relativeSpawnChance)
		{
			this.minLevel = minLevel;
			this.relativeSpawnChance = relativeSpawnChance;
		}
	}
}
