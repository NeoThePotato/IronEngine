using System.Diagnostics;
using Game.World;
using static IO.PlayerInput;
using static Game.World.Direction;

namespace Game
{
	class GameManager
	{
		public LevelManager LevelManager
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		private MapEntity PlayerEntity
		{ get; }

		public GameManager()
		{
			LoadMap("../../../Assets/Maps/TestMap.txt");
			PlayerEntity = new MapEntity(new Entity(), 2, 2);
			LevelManager.AddEntity(PlayerEntity);
		}

		public void Start()
		{
			Debug.WriteLine("GameManager started.");
			IO.Render.ConsoleRenderer consoleRenderer = new IO.Render.ConsoleRenderer(new IO.Render.GameManagerRenderer(this));

			while (true)
			{
				consoleRenderer.RenderFrame();
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
	}
}
