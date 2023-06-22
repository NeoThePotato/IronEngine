using Game.Progression;
using static Assets.GrowthProfileTemplates;

namespace Assets
{
	static class DifficultyProfileTemplates
	{
		public static DifficultyProfile normalDifficulty = new DifficultyProfile("Normal", enemyGrowthProfile, 50, 30, 0.5f, 2);
	}
}
