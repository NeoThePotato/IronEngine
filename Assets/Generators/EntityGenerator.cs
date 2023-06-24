using System.Diagnostics;

namespace Assets.Generators
{
	static class EntityGenerator<Entity>
	{
		public static Entity MakeEntity(Dictionary<Entity, SpawnProfile> spawnableEntities, int level)
		{
			if (spawnableEntities is null)
				throw new ArgumentNullException(nameof(spawnableEntities));

			var entity = PickRandomSpawnableEntity(spawnableEntities, level);

			return entity;
		}

		private static Entity PickRandomSpawnableEntity(Dictionary<Entity, SpawnProfile> entities, int level)
		{
			return PickRandomEntityFromDict(FilterByLevel(entities, level));
		}

		private static Entity PickRandomEntityFromDict(Dictionary<Entity, SpawnProfile> entities)
		{
			var rand = Random.Shared.Next(0, GetCumulativeSpawnChance(entities));

			foreach (var kvp in entities)
			{
				if (rand < kvp.Value.relativeSpawnChance)
					return kvp.Key;
				else
					rand = rand - kvp.Value.relativeSpawnChance;
			}
			Debug.Assert(true, "This function should return a value inside of the foreach loop.");

			return default(Entity);
		}

		private static Dictionary<Entity, SpawnProfile> FilterByLevel(Dictionary<Entity, SpawnProfile> entities, int level)
		{
			var retList = new Dictionary<Entity, SpawnProfile>(entities.Count);

			foreach (var kvp in entities)
			{
				if (kvp.Value.minLevel <= level)
					retList.Add(kvp.Key, kvp.Value);
			}

			return retList;
		}

		private static int GetCumulativeSpawnChance(Dictionary<Entity, SpawnProfile> entities)
		{
			int sum = 0;

			foreach (var kvp in entities)
				sum += kvp.Value.relativeSpawnChance;

			return sum;
		}
	}
}
