using IronEngine;
using System.Diagnostics;
using static Game.World.Position;

namespace Game.World
{
	class Level
	{
		private Map _map;
		private List<LevelEntity> _entities;
		private MapMetadata _metadata;
		private List<LevelEntity> _entitiesList;

		public Map Map
		{ get => _map; private set => _map = value; }
		public List<LevelEntity> Entities
		{ get => _entities; private set => _entities = value; }
		public MapMetadata Metadata
		{ get => _metadata; private set => _metadata = value; }

		public Level(Map map, MapMetadata metadata)
		{
			_map = map;
			_entities = new List<LevelEntity>();
			_metadata = metadata;
			_entitiesList = new List<LevelEntity>(_entities.Count);
		}

		#region ENTITY_SPAWNING
		public LevelEntity AddEntity(Entity entity, Position pos)
		{
			var mapEntity = new LevelEntity(entity, pos);
			AddEntity(mapEntity);

			return mapEntity;
		}

		public void AddEntity(LevelEntity entity)
		{
			Entities.Add(entity);
		}

		public LevelEntity AddEntityAtEntryTile(Entity entity)
		{
			return AddEntity(entity, Metadata.entryTile);
		}

		public LevelEntity AddEntityAtExitTile(Entity entity)
		{
			return AddEntity(entity, Metadata.exitTile);
		}

		public LevelEntity AddEntityAtRandomValidPoint(Entity entity)
		{
			Position randP;

			do
				randP = Map.GetRandomPoint();
			while (!TileTraversable(randP));

			return AddEntity(entity, randP);
		}
		#endregion

		#region ENTITY_MOVEMENT
		public bool MoveEntity(LevelEntity entity, Direction direction, out List<LevelEntity> occupiedBy)
		{
			if (CanEntityMoveTo(entity, direction, out occupiedBy))
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

		public bool MoveEntity(LevelEntity entity, out List<LevelEntity> occupiedBy)
		{
			return MoveEntity(entity, entity.Dir, out occupiedBy);
		}

		private static void MoveEntityToEdgeOfTile(LevelEntity entity, Direction direction)
		{
			var projectedPoint = entity.ProjectedNewLocation(direction);
			var actualJ = Math.ClampRange(projectedPoint.X, TileToPoint(entity.Pos.TileJ), TileToPoint(entity.Pos.TileJ + 1) - 1);
			var actualI = Math.ClampRange(projectedPoint.Y, TileToPoint(entity.Pos.TileI), TileToPoint(entity.Pos.TileI + 1) - 1);
			entity.Pos = new Position(actualJ, actualI);
		}
		#endregion

		#region SPATIAL_CHECKS
		public bool CanEntityMoveTo(LevelEntity entity, Direction targetDir, out List<LevelEntity> occupiedBy)
		{
			return CanEntityMoveTo(entity, entity.Pos, targetDir, out occupiedBy);
		}
		
		public bool CanEntityMoveTo(LevelEntity entity, Position startingPoint, Direction targetDir, out List<LevelEntity> occupiedBy)
		{
			_entitiesList.Clear();
			occupiedBy = _entitiesList;
			var newPos = entity.ProjectedNewLocation(startingPoint, targetDir);

			if (TileTraversable(newPos))
			{
				if (!TileOccupied(newPos, entity) || AllPassable(occupiedBy))
					return true;
			}

			return false;
		}

		public bool CanEntityMoveTo(LevelEntity entity, LevelEntity other)
		{
			Debug.Assert(entity != other);
			bool inLineOfSight = CanEntityMoveTo(entity, other.Pos, out var occupiedBy);

			return inLineOfSight || occupiedBy.Contains(other);
		}

		/// <summary>
		/// Checks if there is a direct line-of-sight/movement between this entity and targetPos.
		/// </summary>
		/// <param name="entity">MapEntity to check movement for.</param>
		/// <param name="targetPos">Target point to check if entity can move to.</param>
		/// <param name="occupiedBy">Returns entity standing in the way, null if there isn't any.</param>
		/// <returns>true</returns>
		public bool CanEntityMoveTo(LevelEntity entity, Position targetPos, out List<LevelEntity> occupiedBy)
		{
			Debug.Assert(entity.Moveable);
			var stepsCounter = 0;
			var maxSteps = (entity.DetectionRange/entity.MovementSpeed) + 1;
			var currentLocation = entity.Pos;
			var currentTrajectory = new Direction(entity.Pos, targetPos);

			while (!SameTile(currentLocation, targetPos))
			{
				if (CanEntityMoveTo(entity, currentLocation, currentTrajectory, out occupiedBy) && stepsCounter < maxSteps)
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
			occupiedBy = GetEntitiesAt(targetPos);

			return true;
		}

		private bool TileTraversable(Position pos)
		{
			return !(PointOutOfBounds(pos) || TileImpassable(pos));
		}

		private bool PointOutOfBounds(Position pos)
		{
			return pos.X >= TileToPoint(Map.TileSizeJ) || pos.X < 0 || pos.Y >= TileToPoint(Map.TileSizeI) || pos.Y < 0;
		}

		private bool TileImpassable(Position pos)
		{
			return !Map.GetTileInfo(pos).passable;
		}

		private bool TileOccupied(Position pos)
		{
			var occupiedBy = GetEntitiesAt(pos);

			return occupiedBy.Any();
		}

		private bool TileOccupied(Position pos, LevelEntity exceptFor)
		{
			var occupiedBy = GetEntitiesAt(pos, exceptFor);

			return occupiedBy.Any();
		}

		public List<LevelEntity> GetEntitiesAt(Position pos)
		{
			_entitiesList.Clear();

			foreach (var entity in Entities)
			{
				if (SameTile(entity.Pos, pos))
					_entitiesList.Add(entity);
			}

			return _entitiesList;
		}

		private List<LevelEntity> GetEntitiesAt(Position pos, LevelEntity exceptFor)
		{
			_entitiesList = GetEntitiesAt(pos);
			_entitiesList.Remove(exceptFor);

			return _entitiesList;
		}

		private static bool AllPassable(List<LevelEntity> entities)
		{
			return entities.All(e => e.Passable);
		}
		#endregion
	}
}
