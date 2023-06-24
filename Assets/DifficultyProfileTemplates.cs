using Game.Progression;
using static Assets.GrowthProfileTemplates;

namespace Assets
{
	static class DifficultyProfileTemplates
	{
		public static DifficultyProfile normalDifficulty = new DifficultyProfile("Normal", enemyGrowthProfile, 250, 500, 0.5f, 2, 0.25f, 3);
	}
}
