using Game.Progression;

namespace Assets
{
	static class GrowthProfileTemplates
	{
		public static readonly GrowthProfile playerGrowthProfile = new GrowthProfile(3, 1, 0.1f, 0.2f, 0.2f);
		public static readonly GrowthProfile enemyGrowthProfile = new GrowthProfile(2, 1, 0.05f, 0.1f, 0.15f);
	}
}
