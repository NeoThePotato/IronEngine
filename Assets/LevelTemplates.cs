using Game.World;

namespace Assets
{
	static class LevelTemplates
	{
		const string MAPS_PATH = "../../../Assets/Maps/";

		public static readonly Dictionary<string, LevelMetadata> LEVELS_DICTIONARY = new Dictionary< string, LevelMetadata>()
		{
			{ "TestMap",        new LevelMetadata("Test Map",	$"{MAPS_PATH}TestMap.txt",		Point2D.Tile(15, 2),	Point2D.Tile(2, 36))},
			{ "Palace",         new LevelMetadata("Palace",		$"{MAPS_PATH}Palace.txt",       Point2D.Tile(102, 49),	Point2D.Tile(4, 90))},
			{ "PalaceSmall",    new LevelMetadata("Palace",		$"{MAPS_PATH}PalaceSmall.txt",  Point2D.Tile(59, 49),	Point2D.Tile(4, 90))},
			{ "Ship",           new LevelMetadata("Ship",		$"{MAPS_PATH}Ship.txt",         Point2D.Tile(9, 23),	Point2D.Tile(70, 15))},
		};
	}
}
