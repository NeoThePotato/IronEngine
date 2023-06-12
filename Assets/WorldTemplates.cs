using Game.World;

namespace Assets.WorldTemplates
{
    static class Levels
    {
        const string MAPS_PATH = "../../../Assets/Maps/";

        public static readonly Dictionary<string, LevelMetadata> LEVELS_DICTIONARY = new Dictionary< string, LevelMetadata>()
        {
            { "TestMap",        new LevelMetadata($"{MAPS_PATH}TestMap.txt",        (2, 2), (0, 0))}, // TODO Set exit points
            { "Palace",         new LevelMetadata($"{MAPS_PATH}Palace.txt",         (39, 38), (0, 0)) },
            { "PalaceSmall",    new LevelMetadata($"{MAPS_PATH}PalaceSmall.txt",    (39, 38), (0, 0)) },
            { "Ship",           new LevelMetadata($"{MAPS_PATH}Ship.txt",           (25, 25), (0, 0)) },
        };
	}
}
