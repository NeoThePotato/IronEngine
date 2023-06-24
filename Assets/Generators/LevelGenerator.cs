using Game.World;
using Game.Items;
using Game.Combat;
using Game.Progression;
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

		public static Level MakeLevel(Unit playerUnit, out MapEntity playerEntity, DifficultyProfile difficulty)
		{
			Level level = MakeEmptyLevel(GetRandomMapMeta());
			playerEntity = level.AddEntityAtEntryTile(playerUnit);
			var tileDirectionMap = GetTileDirectionMap(level.Map);
			GeneratePortals(level);
			GenerateDoors(level, difficulty, tileDirectionMap);
			GenerateChests(level, difficulty, tileDirectionMap);
			GenerateTraps(level, difficulty, level.Map.TileSize);
			GenerateEnemies(level, difficulty, level.Map.TileSize);

            return level;
		}

		private static void GeneratePortals(Level level)
		{
			level.AddEntityAtEntryTile(new Portal(PortalType.Entry));
			level.AddEntityAtExitTile(new Portal(PortalType.Exit));
		}

		private static void GenerateDoors(Level level, DifficultyProfile difficulty, Direction[,] tileDirectionMap)
		{
			var validDoorLocations = GetValidDoorLocations(tileDirectionMap);
			int doorsToGenerate = difficulty.MaxNumOfDoors;

			while(validDoorLocations.Any() && doorsToGenerate > 0)
			{
				var point = PopRandomPoint(validDoorLocations);

				if (RandomRoll(difficulty.DoorChance))
				{
					GenerateDoor(level, difficulty, point);
					doorsToGenerate--;
				}
			}
		}

		private static void GenerateDoor(Level level, DifficultyProfile difficulty, Point2D point)
		{
			//throw new NotImplementedException();
			//level.AddEntity(new MapEntity(new Door(), point)); // TODO Implement Door class
		}

		private static void GenerateChests(Level level, DifficultyProfile difficulty, Direction[,] tileDirectionMap)
		{
			var validChestLocations = GetValidChestLocations(tileDirectionMap);
			int chestsGenerated = 0;
			while (validChestLocations.Any())
			{
				var point = PopRandomPoint(validChestLocations);

				if (!RandomRoll(difficulty.ChestChance))
				{
					GenerateChest(level, difficulty, point);
					chestsGenerated++;
				}

				if (chestsGenerated >= difficulty.MaxNumOfChests)
					break;
			}
		}

		private static void GenerateChest(Level level, DifficultyProfile difficulty, Point2D point)
		{
			int numberOfItems = Random.Shared.Next(0, 5);
			var treasureChest = new Container("Chest", numberOfItems);

			for (int i = 0; i < numberOfItems; i++)
				treasureChest.TryAddItem(ItemGenerator.MakeItem(difficulty.Level));

			level.AddEntity(new MapEntity(treasureChest, point));
		}

		private static void GenerateTraps(Level level, DifficultyProfile difficulty, int mapSize)
		{
			int numberOfTraps = mapSize / difficulty.TrapDensity;

			for (int i = 0; i < numberOfTraps; i++)
			{
				var trap = TrapGenerator.MakeTrap(difficulty);

				if (trap != null)
					level.AddEntityAtRandomValidPoint(trap);
			}
		}

		private static void GenerateEnemies(Level level, DifficultyProfile difficulty, int mapSize)
		{
			int numberOfEnemies = mapSize / difficulty.EnemyDensity;

			for (int i = 0; i < numberOfEnemies; i++)
			{
				var unit = UnitGenerator.MakeUnit(difficulty);

				if (unit != null)
					level.AddEntityAtRandomValidPoint(unit);
			}
		}

        private static List<Point2D> GetValidDoorLocations(Direction[,] map)
        {
            List<Point2D> validDoorLocations = new List<Point2D>(map.Length / 3);

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
			bool c =						!map.GetTileInfo(j, i).passable;
			bool e = i < map.TileSizeI-1?	!map.GetTileInfo(j, i+1).passable : true;
			bool n = j > 0 ?				!map.GetTileInfo(j-1, i).passable : true;
			bool w = i > 0 ?				!map.GetTileInfo(j, i-1).passable : true;
			bool s = j < map.TileSizeJ-1?	!map.GetTileInfo(j+1, i).passable : true;

            return (c ? C : None) | (e ? E : None) | (n ? N : None) | (w ? W : None) | (s ? S : None);
		}

		private static Point2D PopRandomPoint(List<Point2D> points)
		{
			int index = Random.Shared.Next(0, points.Count());
			Point2D point = points.ElementAt(index);
			points.RemoveAt(index);

			return point;
		}

		private static bool RandomRoll(float p)
		{
			return Random.Shared.NextDouble() < p;
		}

		[Flags]
        public enum Direction: byte
        {
			None =	0b00000,
            C =		0b00001,
            E =     0b00010,
            N =     0b00100,
            W =     0b01000,
            S =     0b10000,
        }
    }
}
