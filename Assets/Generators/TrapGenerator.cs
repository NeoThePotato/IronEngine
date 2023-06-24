using Game.Progression;
using Game.World;

namespace Assets.Generators
{
	class TrapGenerator
	{
		private static readonly Dictionary<Trap, SpawnProfile> SPAWNABLE_TRAPS = new Dictionary<Trap, SpawnProfile>()
		{
			{TrapsTemplates.dartTrap,       new SpawnProfile(2, 4)},
			{TrapsTemplates.woodenSpike,    new SpawnProfile(3, 4)},
			{TrapsTemplates.firePit,        new SpawnProfile(5, 5)},
			{TrapsTemplates.rollingCactus,  new SpawnProfile(7, 5)},
		};

		public static Trap? MakeTrap(DifficultyProfile difficultyProfile)
		{
			var trap = EntityGenerator<Trap>.MakeEntity(SPAWNABLE_TRAPS, difficultyProfile.Level);

			if (trap != null)
			{
				trap = new Trap(trap);
			}

			return trap;
		}
	}
}
