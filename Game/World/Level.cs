using static Game.World.Point2D;
using static Game.World.Direction;
using System.Diagnostics;

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

		public MapEntity AddEntity(Entity entity, Point2D pos)
		{
			var mapEntity = new MapEntity(entity, pos);
			AddEntity(mapEntity);

			return mapEntity;
		}

		public void AddEntity(MapEntity entity)
		{
			Entities.Add(entity);
        }

        public MapEntity AddEntityAtEntryTile(Entity entity)
        {
            return AddEntity(entity, Metadata.entryTile);
        }

		public MapEntity AddEntityAtRandomValidPoint(Entity entity)
		{
			Point2D randP;

			do
				randP = Map.GetRandomPoint();
			while (!TileTraversable(randP));

			return AddEntity(entity, randP);
		}

		public bool MoveEntity(MapEntity entity, Direction direction, out MapEntity? otherEntity)
		{
			if (CanEntityMoveTo(entity, entity.ProjectedNewLocation(direction), out otherEntity))
			{
				entity.Move(direction);

				return true;
			}
			else
			{
				MoveEntityToEdgeOfTile(entity, direction);

				return false;
			}
        }

        public bool MoveEntity(MapEntity entity, out MapEntity? otherEntity)
        {
			return MoveEntity(entity, entity.Dir, out otherEntity);
        }

		private void MoveEntityToEdgeOfTile(MapEntity entity, Direction direction)
		{
			var projectedPoint = entity.ProjectedNewLocation(direction);
			var actualJ = Utility.ClampRange(projectedPoint.PointJ, TileToPoint(entity.Pos.TileJ), TileToPoint(entity.Pos.TileJ + 1) - 1);
			var actualI = Utility.ClampRange(projectedPoint.PointI, TileToPoint(entity.Pos.TileI), TileToPoint(entity.Pos.TileI + 1) - 1);
			entity.Pos = new Point2D(actualJ, actualI);
		}

        private bool CanEntityMoveTo(MapEntity entity, Point2D newPos, out MapEntity? occupiedBy)
		{
			occupiedBy = null;

			if (TileTraversable(newPos))
			{
				if (!TileOccupied(newPos, out occupiedBy))
				{
					return true;
				}
				else if (occupiedBy == entity)
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
				return false;
			}
		}

		private bool TileTraversable(Point2D pos)
		{
			return !(PointOutOfBounds(pos) || TileImpassable(pos));
		}

		private bool PointOutOfBounds(Point2D pos)
		{
			return pos.PointJ >= TileToPoint(Map.TileSizeJ) || pos.PointJ < 0 || pos.PointI >= TileToPoint(Map.TileSizeI) || pos.PointI < 0;
		}

		private bool TileImpassable(Point2D pos)
		{
			return !Map.GetTileInfo(pos).passable;
		}

		private bool TileOccupied(Point2D pos, out MapEntity? occupiedBy)
		{
			occupiedBy = GetEntityAt(pos);

			return occupiedBy != null;
		}

		private MapEntity? GetEntityAt(Point2D pos)
		{
			foreach (var entity in Entities)
			{
				if (SameTile(entity.Pos, pos))
					return entity;
			}

			return null;
		}
	}

	class MapEntity
	{
		public const int MAX_MOVEMENT_SPEED = POINTS_PER_TILE;
		public Entity Entity
		{ get; set; }
		public Point2D Pos
		{ get; set; }
		public Direction Dir
		{ get; set; }
		public int Speed
		{ get => MAX_MOVEMENT_SPEED / 4; } // TODO Replace with entity "SPD" stat or something
		public bool Passable
		{ get => Entity.Passable; }

		public MapEntity(Entity entity, Point2D pos)
		{
			Entity = entity;
			Pos = pos;
			Dir = TranslateDirection(Directions.None);
		}

		public MapEntity(Entity entity, Point2D pos, Direction dir)
		{
			Entity = entity;
			Pos = pos;
			Dir = dir;
		}

		public void Move()
        {
            Move(Dir);
        }

        public void Move(Direction dir)
		{
			dir.Normalize();
			Debug.Assert(dir.Mag <= MAX_MAG);
			Dir = dir;
			Pos = ProjectedNewLocation(dir);
		}

		public Point2D ProjectedNewLocation(Direction dir)
		{
			return Pos + EffectiveMovement(dir);
		}

		private Direction EffectiveMovement(Direction dir)
		{
			return (dir * Speed) / MAX_MOVEMENT_SPEED;
		}

		public override string ToString()
		{
			return Entity.ToString();
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
		public Point2D entryTile;
		public Point2D exitTile;

		public LevelMetadata(string name, string filePath, Point2D entryTile, Point2D exitTile)
		{
			this.name = name;
			this.filePath = filePath;
			this.entryTile = entryTile;
			this.exitTile = exitTile;
        }

        public static LevelMetadata GetMetadata(string levelName)
        {
            return Assets.LevelTemplates.LEVELS_DICTIONARY[levelName];
        }

		public override string ToString()
		{
			return name;
		}
    }
}
