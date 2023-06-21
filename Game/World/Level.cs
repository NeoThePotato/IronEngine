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

		#region ENTITY_SPAWNING
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
		#endregion

		#region ENTITY_MOVEMENT
		public bool MoveEntity(MapEntity entity, Direction direction, out MapEntity? otherEntity)
		{
			if (CanEntityMoveTo(entity, direction, out otherEntity))
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
		#endregion

		#region SPATIAL_CHECKS
		public bool CanEntityMoveTo(MapEntity entity, Direction targetDir, out MapEntity? occupiedBy)
		{
			occupiedBy = null;
			var newPos = entity.ProjectedNewLocation(targetDir);

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

		/// <summary>
		/// Checks if there is a direct line-of-sight/movement between this entity and targetPos.
		/// </summary>
		/// <param name="entity">MapEntity to check movement for.</param>
		/// <param name="targetPos">Target point to check if entity can move to.</param>
		/// <param name="occupiedBy">Returns entity standing in the way, null if there isn't any.</param>
		/// <returns>true</returns>
		public bool CanEntityMoveTo(MapEntity entity, Point2D targetPos)
		{
			Debug.Assert(entity.Moveable);
			MapEntity? _ = null;
			int stepsCounter = 0;
			int maxSteps = (entity.DetectionRange/entity.MovementSpeed) + 1;
			Point2D currentLocation = entity.Pos;
			Direction currentTrajectory = new Direction(entity.Pos, targetPos);
			CanEntityMoveTo(entity, currentTrajectory, out _);

			while (!SameTile(currentLocation, targetPos))
			{
				if (CanEntityMoveTo(entity, currentTrajectory, out _) && stepsCounter < maxSteps)
				{
					currentLocation = entity.ProjectedNewLocation(currentLocation, currentTrajectory);
					currentTrajectory = new Direction(currentLocation, targetPos);
					stepsCounter++;
				}
				else
				{
					Debug.WriteIf(stepsCounter >= maxSteps, $"{entity} took {stepsCounter} steps and failed to reach its destination");

					return false;
				}
			}

			return true;
		}

		public bool CanEntityMoveTo(MapEntity entity, MapEntity other)
		{
			Debug.Assert(entity != other);
			bool inLineOfSight = CanEntityMoveTo(entity, other.Pos);

			return inLineOfSight;
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
		#endregion
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
