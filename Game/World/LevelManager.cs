namespace Game.World
{
    class LevelManager
    {
        private Map _map;
        private List<MapEntity> _entities;

        public Map Map
        { get => _map; private set => _map = value; }
        public List<MapEntity> Entities
        { get => _entities; private set => _entities = value; }

        public LevelManager(Map map)
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
            var isTraversable = TileTraversable(entity.PosJ + offsetJ, entity.PosI + offsetI, out occupiedBy);

            if (occupiedBy == entity)
                occupiedBy = null;

            return isTraversable;
        }

        private bool TileTraversable(int posJ, int posI, out MapEntity? occupiedBy)
        {
			occupiedBy = null;

            return !(TileOutOfBounds(posJ, posI) || TileImpassable(posJ, posI) || TileOccupied(posJ, posI, out occupiedBy));
        }

        private bool TileImpassable(int posJ, int posI)
        {
            return !Map.GetTileInfo(posJ, posI).passable;
        }

        private bool TileOutOfBounds(int posJ, int posI)
        {
            return posJ >= Map.SizeJ || posJ < 0 || posI >= Map.SizeI || posI < 0;
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

		public MapEntity(Entity entity, int posJ, int posI)
        {
            Entity = entity;
            PosJ = posJ;
            PosI = posI;
        }

        public void Move(Direction.Directions direction)
        {
            (int movJ, int movI) = Direction.TranslateDirection(direction);
            Move(movJ, movI);
        }

        public void Move(int movJ, int movI)
        {
            PosJ += movJ;
            PosI += movI;
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
}
