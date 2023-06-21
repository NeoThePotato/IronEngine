using Game.World;
using Game.Items;
using Game.Combat;
using Assets.EquipmentTemplates;
using static Assets.MapTemplates;
using static Assets.Generators.LevelGenerator.Direction;

namespace Assets.Generators
{
	static class LevelGenerator
	{
		public static Level MakeEmptyLevel(string levelName)
		{
			var levelMetadata = MapMetadata.GetMetadata(levelName);

			return MakeEmptyLevel(levelMetadata);
		}

		public static Level MakeEmptyLevel(MapMetadata mapMetadata)
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
			level = MakeEmptyLevel(GetRandomMapMeta());
            playerEntity = level.AddEntityAtEntryTile(playerUnit);
			var tileDirectionMap = GetTileDirectionMap(level.Map);
			GeneratePortals(level);
			GenerateDoors(level, tileDirectionMap);
			GenerateChests(level, tileDirectionMap);
			GenerateTraps(level);
			GenerateEnemies(level);

            return level;
		}

		private static void GeneratePortals(Level level)
		{
			level.AddEntityAtEntryTile(new Portal(PortalType.Entry));
			level.AddEntityAtExitTile(new Portal(PortalType.Exit));
		}

		private static void GenerateDoors(Level level, Direction[,] tileDirectionMap)
		{
			var validDoorLocations = GetValidDoorLocations(tileDirectionMap);
			throw new NotImplementedException(); // TODO Call ChestGenerator here, code below is rough guide
		}

		private static void GenerateChests(Level level, Direction[,] tileDirectionMap)
		{
			var validChestLocations = GetValidChestLocations(tileDirectionMap);
			throw new NotImplementedException(); // TODO Call ChestGenerator here, code below is rough guide
			var treasureChest = new Container("Chest", 5);
			treasureChest.TryAddItem(Armors.rustedChestplate);
			treasureChest.TryAddItem(Weapons.rustedBlade);
			level.AddEntityAtRandomValidPoint(treasureChest);
		}

		private static void GenerateTraps(Level level)
		{
			throw new NotImplementedException(); // TODO Call TrapGenerator here, code below is rough guide
			level.AddEntityAtRandomValidPoint(TrapsTemplates.firePit);
		}

		private static void GenerateEnemies(Level level)
		{
			throw new NotImplementedException(); // TODO Call UnitGenerator here, code below is rough guide
			level.AddEntityAtRandomValidPoint(UnitTemplates.slime);
        }

        private static List<Point2D> GetValidDoorLocations(Direction[,] map)
        {
            List<Point2D> validDoorLocations = new List<Point2D>(map.Length/3);

            for (int j = 1; j < map.GetLength(0); j++)
            {
                for (int i = 1; i < map.GetLength(1); i++)
                {
                    if (IsValidDoorLocation(map[j, i]))
                    {
                        validDoorLocations.Add(Point2D.Tile(j, i));
                    }
                }
            }

            return validDoorLocations;
		}

		private static List<Point2D> GetValidChestLocations(Direction[,] map)
		{
			List<Point2D> validDoorLocations = new List<Point2D>(map.Length / 3);

			for (int j = 1; j < map.GetLength(0); j++)
			{
				for (int i = 1; i < map.GetLength(1); i++)
				{
					if (IsValidChestLocation(map[j, i]))
					{
						validDoorLocations.Add(Point2D.Tile(j, i));
					}
				}
			}

			return validDoorLocations;
		}

		private static bool IsValidDoorLocation(Direction dir)
		{
			return dir == (E | W) || dir == (N | S);
		}

		private static bool IsValidChestLocation(Direction dir)
		{
			return dir == (E | N | W) || dir == (E | N | S) || dir == (E | W | S) || dir == (N | W | S);
		}

		private static Direction[,] GetTileDirectionMap(Map map)
        {
            Direction[,] directionMap = new Direction[map.TileSizeJ, map.TileSizeI];

			for (int j = 0; j < map.TileSizeJ; j++)
			{
				for (int i = 0; i < map.TileSizeI; i++)
				{
                    directionMap[j, i] = GetTileDirection(map, j, i);
				}
			}

            return directionMap;
		}

		private static Direction GetTileDirection(Map map, int j, int i)
		{
			bool e = i < map.TileSizeI? !map.GetTileInfo(j, i+1).passable : true;
			bool n = j > 0 ? !map.GetTileInfo(j-1, i).passable : true;
			bool w = i > 0 ? !map.GetTileInfo(j, i-1).passable : true;
			bool s = j > map.TileSizeJ ? !map.GetTileInfo(j+1, i).passable : true;

            return (e ? E : None) | (n ? N : None) | (w ? W : None) | (s ? S : None);
		}

		[Flags]
        public enum Direction: byte
        {
            None =  0b0000,
            E =     0b0001,
            N =     0b0010,
            W =     0b0100,
            S =     0b1000,
        }
    }
}
