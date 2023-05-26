namespace World
{
	class Map
	{
		private char[,] _tileMap;
		private List<MapEntity> _entities;

		public int SizeJ
		{
			get => _tileMap.GetLength(0);
		}
		public int SizeI
		{
			get => _tileMap.GetLength(1);
		}
		public char[,] TileMap
		{ get => _tileMap; private set => _tileMap = value; }
		public List<MapEntity> Entities
		{ get => _entities; private set => _entities = value; }

		public Map(int sizeJ, int sizeI)
		{
			_tileMap = new char[sizeJ, sizeI];
			_entities = new List<MapEntity>();
		}

		public MapEntity AddEntity(Entity entity, int posJ, int posI)
		{
			var mapEntity = new MapEntity(entity, posJ, posI);
			AddEntity(mapEntity);

			return mapEntity;
		}

		public void AddEntity(MapEntity mapEntity)
		{
			Entities.Add(mapEntity);
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
		private static readonly (int, int)[] directionDictionary = {
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
			return directionDictionary[(int)direction];
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
