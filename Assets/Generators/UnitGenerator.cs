using Game.Combat;
using Game.Progression;
using System.Diagnostics;

namespace Assets.Generators
{
	static class UnitGenerator
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

		public static Unit MakeUnit(DifficultyProfile difficultyProfile)
		{
			var unit = new Unit(PickRandomSpawnableUnit(difficultyProfile.Level));
			GrowUnit(ref unit, difficultyProfile.Level, difficultyProfile.EnemyGrowthProfile);

			return unit;
		}

		private static Unit PickRandomSpawnableUnit(int level)
		{
			return PickRandomUnitFromDict(FilterByLevel(SPAWNABLE_UNITS, level));
		}

		private static void GrowUnit(ref Unit unit, int levels, GrowthProfile profile)
		{
			profile.GrowUnit(ref unit, levels);
		}

		private static Unit PickRandomUnitFromDict(Dictionary<Unit, SpawnProfile> units)
		{
			var rand = Random.Shared.Next(0, GetCumulativeSpawnChance(units));

			foreach (var kvp in units)
			{
				if (rand < kvp.Value.relativeSpawnChance)
					return kvp.Key;
				else
					rand = rand - kvp.Value.relativeSpawnChance;
			}
			Debug.Assert(true, "This function should return a value inside of the foreach loop.");

			return null;
		}

		private static Dictionary<Unit, SpawnProfile> FilterByLevel(Dictionary<Unit, SpawnProfile> units, int level)
		{
			var retList = new Dictionary<Unit, SpawnProfile>(units.Count);

			foreach (var kvp in units)
			{
				if (kvp.Value.minLevel >= level)
					retList.Add(kvp.Key, kvp.Value);
			}

			return retList;
		}

		private static int GetCumulativeSpawnChance(Dictionary<Unit, SpawnProfile> units)
		{
			int sum = 0;

			foreach (var kvp in units)
				sum += kvp.Value.relativeSpawnChance;

			return sum;
		}
	}
}
