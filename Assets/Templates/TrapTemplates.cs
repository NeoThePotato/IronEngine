using Game.World;

namespace Assets.Templates
{
	internal class TrapsTemplates
	{
		public static readonly Trap dartTrap =		new("Dart Trap", 1, 5);
		public static readonly Trap woodenSpike =	new("Wooden Spike Trap", 3, 15);
		public static readonly Trap firePit =		new("Fire Pit Trap", 4, 20);
		public static readonly Trap rollingCactus =	new("Rolling Cactus", 6, 30);
	}
}
