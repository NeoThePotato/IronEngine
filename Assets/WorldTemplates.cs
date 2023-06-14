using Game.World;

namespace Assets.WorldTemplates
{
	static class Levels
	{
		const string MAPS_PATH = "../../../Assets/Maps/";

		public static readonly Dictionary<string, LevelMetadata> LEVELS_DICTIONARY = new Dictionary< string, LevelMetadata>()
		{
			{ "TestMap",        new LevelMetadata("Test Map",	$"{MAPS_PATH}TestMap.txt",		(15, 2), (2, 36))},
			{ "Palace",         new LevelMetadata("Palace",		$"{MAPS_PATH}Palace.txt",		(102, 49), (4, 90))},
			{ "PalaceSmall",    new LevelMetadata("Palace",		$"{MAPS_PATH}PalaceSmall.txt",	(59, 49), (4, 90))},
			{ "Ship",           new LevelMetadata("Ship",		$"{MAPS_PATH}Ship.txt",			(9, 23), (70, 15)) },
		};
	}
}
