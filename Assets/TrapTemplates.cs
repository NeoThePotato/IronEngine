using Game.World;

namespace Assets
{
	internal class TrapsTemplates
	{
		public static readonly Trap dartTrap = new Trap("Dart Trap",1 , 5);
		public static readonly Trap woodenSpike = new Trap("Wooden Spike Trap", 3, 15);
		public static readonly Trap firePit = new Trap("Fire Pit Trap", 4, 20);
		public static readonly Trap rollingCactus = new Trap("Rolling Cactus", 6, 30);
	}
}
