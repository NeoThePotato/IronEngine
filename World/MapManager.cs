﻿namespace World
{
	class MapManager
	{
		private Map _map;
		private List<MapEntity> _entities;

		public Map Map
		{ get => _map; private set => _map = value; }
		public List<MapEntity> Entities
		{ get => _entities; private set => _entities = value; }

		public MapManager(Map map)
		{
			_map = map;
			_entities = new List<MapEntity>();
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

		private bool TileTraversable(int posJ, int posI)
		{
			return !(TileImpassable(posJ, posI) || TileOccupied(posJ, posI) || TileOutOfBounds(posJ, posI));
		}

		private bool TileImpassable(int posJ, int posI)
		{
			return !Map.GetTileInfo(posJ, posI).passable;
		}

		private bool TileOutOfBounds(int posJ, int posI)
		{
			return posJ > Map.SizeJ || posI > Map.SizeI;
		}

		private bool TileOccupied(int posJ, int posI)
		{
			return GetEntityAt(posJ, posI) != null;
		}

		private MapEntity? GetEntityAt(int posJ, int posI)
		{
			foreach (var entity in Entities)
			{
				if (entity.posJ == posJ && entity.posI == posI)
					return entity;
			}

			return null;
		}
	}

	struct MapEntity
	{
		public Entity entity;
		public int posJ, posI;

		public MapEntity(Entity entity, int posI, int posJ)
		{
			this.entity = entity;
			this.posJ = posJ;
			this.posI = posI;
		}

		public void Move(Direction.Directions direction)
		{
			(int movJ, int movI) = Direction.TranslateDirection(direction);
			Move(movJ, movI);
		}

		public void Move(int movJ, int movI)
		{
			posJ += movJ;
			posI += movI;
		}
	}

	struct Direction
	{
		private static readonly (int, int)[] DIRECTION_VECTORS = {
			(0, 1),
			(-1, 1),
			(-1, 0),
			(-1, -1),
			(0, -1),
			(1, -1),
			(1, 0),
			(1, 1)
		};

		public static (int, int) TranslateDirection(Directions direction)
		{
			return DIRECTION_VECTORS[(int)direction];
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
			SE = 7
		}
	}
}
