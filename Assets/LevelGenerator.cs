using Game.World;
using Game.Items;
using Game.Combat;
using Assets.EquipmentTemplates;
using static Assets.MapTemplates;

namespace Assets
{
	static class LevelGenerator
	{
		public static Level MakeLevel(string levelName)
		{
			var levelMetadata = MapMetadata.GetMetadata(levelName);

			return MakeLevel(levelMetadata);
		}

		public static Level MakeLevel(MapMetadata mapMetadata)
		{
			var charData = IO.File.Map.LoadMapCharData(mapMetadata.filePath);

			if (charData != null)
				return new Level(new Map(charData), mapMetadata);
			else
				throw new NullReferenceException();
		}
		public static Level MakeLevel(Unit playerUnit, out MapEntity playerEntity)
		{
			Level level;
			level = MakeLevel(GetRandomMapMeta());
            playerEntity = level.AddEntityAtEntryTile(playerUnit);
            GenerateEnemies(level);
            GenerateTraps(level);
            GenerateChests(level);
            GenerateDoors(level);
            GeneratePortals(level);

            return level;
        }

		private static void GenerateEnemies(Level level)
        {
            level.AddEntityAtRandomValidPoint(UnitTemplates.slime);
        }

        private static void GenerateTraps(Level level)
        {
            level.AddEntityAtEntryTile(TrapsTemplates.firePit);
        }

        private static void GenerateChests(Level level)
        {
            var treasureChest = new Container("Chest", 5);
            treasureChest.TryAddItem(Armors.rustedChestplate);
            treasureChest.TryAddItem(Weapons.rustedBlade);
            level.AddEntityAtRandomValidPoint(treasureChest);
        }

        private static void GenerateDoors(Level level)
        {

        }

        private static void GeneratePortals(Level level)
        {
            level.AddEntityAtEntryTile(new Portal(PortalType.Entry));
            level.AddEntityAtExitTile(new Portal(PortalType.Exit));
        }
    }
}
