namespace Game.World
{
	class Level
	{
		private Map _map;
		private List<MapEntity> _entities;
		private LevelMetadata _metadata;

		public Map Map
		{ get => _map; private set => _map = value; }
		public List<MapEntity> Entities
		{ get => _entities; private set => _entities = value; }
        public LevelMetadata Metadata
		{ get => _metadata; private set => _metadata = value; }

        public Level(Map map, LevelMetadata metadata)
		{
			_map = map;
			_entities = new List<MapEntity>();
			_metadata = metadata;
		}

		public MapEntity AddEntity(Entity entity, int posJ, int posI)
		{
			var mapEntity = new MapEntity(entity, posJ, posI);
			AddEntity(mapEntity);

			return mapEntity;
		}

		public void AddEntity(MapEntity entity)
		{
			Entities.Add(entity);
        }

        public MapEntity AddEntityAtEntryTile(Entity entity)
        {
            (int entryJ, int EntryI) = Metadata.entryTile;

            return AddEntity(entity, entryJ, EntryI);
        }

		public MapEntity AddEntityAtRandomValidTile(Entity entity)
		{
			int randJ;
			int randI;

			do
			{
				randJ = Random.Shared.Next(0, Map.SizeJ);
				randI = Random.Shared.Next(0, Map.SizeI);
			}
			while (!TileTraversable(randJ, randI));

			return AddEntity(entity, randJ, randI);
		}

        public bool MoveEntity(MapEntity entity, Direction.Directions direction, out MapEntity? otherEntity)
		{
			if (CanEntityMoveTo(entity, direction, out otherEntity))
			{
				entity.Move(direction);

				return true;
			}
			else
			{
				return false;
			}
		}

		private bool CanEntityMoveTo(MapEntity entity, Direction.Directions direction, out MapEntity? occupiedBy)
		{
			(int offsetJ, int offsetI) = Direction.TranslateDirection(direction);
			int newPosJ = entity.PosJ + offsetJ, newPosI = entity.PosI + offsetI;
			occupiedBy = null;

			if (TileTraversable(newPosJ, newPosI))
			{
				if (TileOccupied(newPosJ, newPosI, out occupiedBy))
				{
					if (occupiedBy == entity)
					{
						occupiedBy = null;
						return true;
					}
					else if (occupiedBy.Passable)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		private bool TileTraversable(int posJ, int posI)
		{
			return !(TileOutOfBounds(posJ, posI) || TileImpassable(posJ, posI));
		}

		private bool TileOutOfBounds(int posJ, int posI)
		{
			return posJ >= Map.SizeJ || posJ < 0 || posI >= Map.SizeI || posI < 0;
		}

		private bool TileImpassable(int posJ, int posI)
		{
			return !Map.GetTileInfo(posJ, posI).passable;
		}

		private bool TileOccupied(int posJ, int posI, out MapEntity? occupiedBy)
		{
			occupiedBy = GetEntityAt(posJ, posI);

			return occupiedBy != null;
		}

		private MapEntity? GetEntityAt(int posJ, int posI)
		{
			foreach (var entity in Entities)
			{
				if (entity.PosJ == posJ && entity.PosI == posI)
					return entity;
			}

			return null;
		}
	}

	class MapEntity
	{
		public Entity Entity
		{ get; set; }
		public int PosJ
		{ get; set; }
		public int PosI
		{ get; set; }
		public Direction.Directions Dir
		{ get; set; }
		public bool Passable
		{ get => Entity.Passable; }

		public MapEntity(Entity entity, int posJ, int posI, Direction.Directions direction = Direction.Directions.None)
		{
			Entity = entity;
			PosJ = posJ;
			PosI = posI;
			Dir = direction;
		}

        public void Move()
        {
            Move(Dir);
        }

        public void Move(Direction.Directions direction)
		{
			(int movJ, int movI) = Direction.TranslateDirection(direction);
			Dir = direction;
			Move(movJ, movI);
		}

		public void Move(int movJ, int movI)
		{
			PosJ += movJ;
			PosI += movI;
		}

		public override string ToString()
		{
			return Entity.ToString();
		}
	}

	static class Direction
	{
		private static readonly (int, int)[] DIRECTION_VECTORS = {
			(0, 1),
			(-1, 1),
			(-1, 0),
			(-1, -1),
			(0, -1),
			(1, -1),
			(1, 0),
			(1, 1),
			(0, 0)
		};

		public static (int, int) TranslateDirection(Directions direction)
		{
			return DIRECTION_VECTORS[(int)direction];
		}

		public static Directions TranslateDirection((int, int) vector)
		{
			return (Directions)Array.IndexOf(DIRECTION_VECTORS, vector);
		}

		public enum Directions
		{
			E = 0,
			NE = 1,
			N = 2,
			NW = 3,
			W = 4,
			SW = 5,
			S = 6,
			SE = 7,
			None = 8
		}
	}

	static class LevelFactory
	{
		public static Level MakeLevel(string levelName)
        {
			var levelMetadata = LevelMetadata.GetMetadata(levelName);

            return MakeLevel(levelMetadata);
        }

        public static Level MakeLevel(LevelMetadata levelMetadata)
        {
            var charData = IO.File.Map.LoadMapCharData(levelMetadata.filePath);

            if (charData != null)
                return new Level(new Map(charData), levelMetadata);
            else
                throw new NullReferenceException();
        }
    }

	struct LevelMetadata
	{
		public string name;
		public string filePath;
		public (int, int) entryTile;
		public (int, int) exitTile;

		public LevelMetadata(string name, string filePath, (int, int) entryTile, (int, int) exitTile)
		{
			this.name = name;
			this.filePath = filePath;
			this.entryTile = entryTile;
			this.exitTile = exitTile;
        }

        public static LevelMetadata GetMetadata(string levelName)
        {
            return Assets.WorldTemplates.Levels.LEVELS_DICTIONARY[levelName];
        }

		public override string ToString()
		{
			return name;
		}
    }
}
