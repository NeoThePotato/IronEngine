using IO.Render;
using System.Diagnostics;
using World;
using static IO.PlayerInput;
using static World.Direction;

namespace Game
{
	class GameManager : IRenderable
	{
		public LevelManager MapManager
		{ get; private set; }
		private List<MapEntity> Entities
		{ get => MapManager.Entities; }
		private MapEntity PlayerEntity
		{ get; }
		public int SizeJ
		{ get => MapManager.Map.SizeJ; }
		public int SizeI
		{ get => MapManager.Map.SizeI; }

		public GameManager()
		{
			LoadMap("../../../Assets/Maps/TestMap.txt");
			PlayerEntity = new MapEntity(new Entity(), 2, 2);
			MapManager.AddEntity(PlayerEntity);
		}

		public void Start()
		{
			Debug.WriteLine("GameManager started.");
			ConsoleRenderer renderer = new ConsoleRenderer(2, MapManager.Map.SizeJ, MapManager.Map.SizeI);
			renderer.AddElement(new Element(this));

			while (true)
			{
				renderer.RenderFrame();
				var dir = InputToDirection(WaitForKey());
				var moved = MapManager.MoveEntity(PlayerEntity, dir);
				Debug.WriteLine($"Moved: {moved}, {PlayerEntity.posJ}, {PlayerEntity.posI}");
				Debug.Assert(PlayerEntity.Equals(MapManager.Entities[0]));
			}
		}

		private void Encounter(MapEntity player, MapEntity other)
		{
			throw new NotImplementedException();
		}

		private bool LoadMap(string pathName)
		{
			var charData = IO.File.LoadMapCharData(pathName);

			if (charData != null)
			{
				MapManager = new LevelManager(new Map(charData));
				return true;
			}
			else
			{
				return false;
			}
		}

		private Directions InputToDirection(PlayerInputs input)
		{
			var directionDict = new Dictionary<PlayerInputs, Directions>()
			{
				{ PlayerInputs.Right, Directions.E},
				{ PlayerInputs.UpRight, Directions.NE},
				{ PlayerInputs.Up, Directions.N },
				{ PlayerInputs.UpLeft, Directions.NW },
				{ PlayerInputs.Left, Directions.W },
				{ PlayerInputs.DownLeft, Directions.SW },
				{ PlayerInputs.Down, Directions.S },
				{ PlayerInputs.DownRight, Directions.SE },
				{ PlayerInputs.Any, Directions.None },
				{ PlayerInputs.Confirm, Directions.None },
			};

			return directionDict[input];
		}

		public char[,] Render()
		{
			var map = new char[SizeJ, SizeI];

			for (int j = 0; j < SizeJ; j++)
			{
				for (int i = 0; i < SizeI; i++)
				{
					map[j, i] = MapManager.Map.TileMap[j, i];
				}
			}

			foreach (var entity in Entities)
				map[entity.posJ, entity.posI] = '@';

			return map;
		}
	}
}
