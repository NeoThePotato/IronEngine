using Game;
using Game.Combat;
using Game.Progression;
using System.Diagnostics;

namespace Assets.Generators
{
	static class UnitGenerator
	{
		private static readonly List<SpawnProfile> SPAWNABLE_UNITS = new List<SpawnProfile>()
		{
			{ new SpawnProfile(UnitTemplates.slime, 1, 5)},
			// TODO Add more templates
		};

		public static Unit MakeUnit(DifficultyProfile difficultyProfile)
		{
			var unit = new Unit(PickRandomSpawnableUnit(difficultyProfile.Level));
			GrowUnit(ref unit, difficultyProfile.Level, difficultyProfile.EnemyGrowthProfile);

			return unit;
		}

		private static Unit PickRandomSpawnableUnit(int level)
		{
			return PickRandomUnitFromList(FilterByLevel(SPAWNABLE_UNITS, level)).unit;
		}

		private static void GrowUnit(ref Unit unit, int levels, GrowthProfile profile)
		{
			profile.GrowUnit(ref unit, levels);
		}

		private static SpawnProfile PickRandomUnitFromList(List<SpawnProfile> units)
		{
			var rand = Random.Shared.Next(0, GetCumulativeSpawnChance(units));

			foreach (var unit in units)
			{
				if (rand < unit.relativeSpawnChance)
					return unit;
				else
					rand = rand - unit.relativeSpawnChance;
			}
			Debug.Assert(true, "This function should return a value inside of the foreach loop.");

			return units[0];
		}

		private static List<SpawnProfile> FilterByLevel(List<SpawnProfile> units, int level)
		{
			var retList = new List<SpawnProfile>(units.Count);

            foreach (var unit in units)
            {
                if (unit.minLevel >= level)
					retList.Add(unit);
            }

			return retList;
        }

		private static int GetCumulativeSpawnChance(List<SpawnProfile> units)
		{
			int sum = 0;

			foreach (SpawnProfile profile in units)
				sum += profile.relativeSpawnChance;

			return sum;
		}

		struct SpawnProfile
		{
			public Unit unit;
			public int minLevel;
			public int relativeSpawnChance;

			public SpawnProfile(Unit unit, int minLevel, int relativeSpawnChance)
			{
				this.unit = unit;
				this.minLevel = minLevel;
				this.relativeSpawnChance = relativeSpawnChance;
			}
		}
	}
}
