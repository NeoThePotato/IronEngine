using Game.Combat;
using Game.Progression;

namespace Assets.Generators
{
	class UnitGenerator
	{
		private static readonly Dictionary<Unit, SpawnProfile> SPAWNABLE_UNITS = new Dictionary<Unit, SpawnProfile>()
		{
			{UnitTemplates.slime,			new SpawnProfile(1, 5)},
			{UnitTemplates.bandit,			new SpawnProfile(2, 5)},
			{UnitTemplates.imp,				new SpawnProfile(2, 6)},
			{UnitTemplates.fae,				new SpawnProfile(2, 6)},
			{UnitTemplates.spawnOfTwilight,	new SpawnProfile(4, 2)},
			{UnitTemplates.antiHero,		new SpawnProfile(10, 2)},
		};

		public static Unit? MakeUnit(DifficultyProfile difficultyProfile)
		{
			var unit = EntityGenerator<Unit>.MakeEntity(SPAWNABLE_UNITS, difficultyProfile.Level);

			if (unit != null)
			{
				unit = new Unit(unit);
				GrowUnit(ref unit, difficultyProfile.Level, difficultyProfile.EnemyGrowthProfile);
			}

			return unit;
		}

		private static void GrowUnit(ref Unit unit, int levels, GrowthProfile profile)
		{
			profile.GrowUnit(ref unit, levels);
		}
	}
}
