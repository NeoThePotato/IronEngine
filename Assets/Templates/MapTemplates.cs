using Game.World;

namespace Assets.Templates
{
	static class MapTemplates
	{
		const string MAPS_PATH = "../../../Assets/Maps/";

		public static readonly Dictionary<string, MapMetadata> MAPS_DICTIONARY = new()
		{
			{ "Plains1",	new("Rocky Plains",			$"{MAPS_PATH}Plains1.txt",		Point2D.Tile(0, 50),	Point2D.Tile(37, 80))},
			{ "Plains2",	new("Suspicious Structure",	$"{MAPS_PATH}Plains2.txt",		Point2D.Tile(29, 29),	Point2D.Tile(0, 26))},
			{ "Village1",	new("Fancy village",		$"{MAPS_PATH}Village1.txt",		Point2D.Tile(28, 3),	Point2D.Tile(4, 43))},
			{ "Village2",	new("Quiet village",		$"{MAPS_PATH}Village2.txt",		Point2D.Tile(29, 25),	Point2D.Tile(2, 47))},
			{ "Village3",	new("Moat-Side village",	$"{MAPS_PATH}Village3.txt",		Point2D.Tile(29, 23),	Point2D.Tile(0, 23))},
			{ "Palace1",	new("The Grand Palace",		$"{MAPS_PATH}Palace1.txt",		Point2D.Tile(41, 48),	Point2D.Tile(6, 97))},
			{ "Sewers1",	new("Sewers Cistern",		$"{MAPS_PATH}Sewers1.txt",		Point2D.Tile(42, 50),	Point2D.Tile(0, 50))},
			{ "Sewers2",	new("Sewers North",			$"{MAPS_PATH}Sewers2.txt",		Point2D.Tile(13, 24),	Point2D.Tile(13, 26))},
			{ "Sewers3",	new("Sewers South",			$"{MAPS_PATH}Sewers3.txt",		Point2D.Tile(1, 33),	Point2D.Tile(23, 32))},
			{ "Ship1",		new("Ship",					$"{MAPS_PATH}Ship1.txt",		Point2D.Tile(6, 15),	Point2D.Tile(45, 7))},
		};

		public static readonly Dictionary<string, MapMetadata> CUT_MAPS_DICTIONARY = new()
		{
			{ "TestMap",		new("Test Map",			$"{MAPS_PATH}TestMap.txt",		Point2D.Tile(15, 2),	Point2D.Tile(2, 36))},
			{ "Palace",			new("Palace",			$"{MAPS_PATH}Palace.txt",		Point2D.Tile(102, 49),	Point2D.Tile(4, 90))},
			{ "CastleRuins1",	new("Castle Ruins",		$"{MAPS_PATH}CastleRuins1.txt",	Point2D.Tile(74, 59),	Point2D.Tile(21, 30))},
		};

		public static MapMetadata GetRandomMapMeta()
		{
			return MAPS_DICTIONARY.ElementAt(Random.Shared.Next(0, MAPS_DICTIONARY.Count)).Value;
		}
	}
}
