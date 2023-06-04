using IO.Render;
using System.Diagnostics;
using World;
using static IO.PlayerInput;
using static World.Direction;

namespace Game
{
	class GameManager
	{
		public LevelManager LevelManager
		{ get; private set; }
		private List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		private MapEntity PlayerEntity
		{ get; }
		public int SizeJ
		{ get => LevelManager.Map.SizeJ; }
		public int SizeI
		{ get => LevelManager.Map.SizeI; }

		public GameManager()
		{
			LoadMap("../../../Assets/Maps/TestMap.txt");
			PlayerEntity = new MapEntity(new Entity(), 2, 2);
			LevelManager.AddEntity(PlayerEntity);
		}

		public void Start()
		{
			Debug.WriteLine("GameManager started.");
			ConsoleRenderer renderer = new ConsoleRenderer(2, LevelManager.Map.SizeJ, LevelManager.Map.SizeI);
			renderer.AddElement(new Element(this));

			while (true)
			{
				renderer.RenderFrame();
				var dir = InputToDirection(WaitForKey());
				var moved = LevelManager.MoveEntity(PlayerEntity, dir);
				Debug.WriteLine($"Moved: {moved}, {PlayerEntity.posJ}, {PlayerEntity.posI}");
				Debug.Assert(PlayerEntity.Equals(LevelManager.Entities[0]));
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
				LevelManager = new LevelManager(new Map(charData));
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
					map[j, i] = LevelManager.Map.TileMap[j, i];
				}
			}

			foreach (var entity in Entities)
				map[entity.posJ, entity.posI] = '@';

			return map;
		}
	}
}
