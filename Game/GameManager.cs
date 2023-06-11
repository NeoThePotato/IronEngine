using System.Diagnostics;
using Game.World;
using static IO.PlayerInput;
using static Game.World.Direction;

namespace Game
{
	class GameManager
	{
		public const int GLOBAL_TICK_RATE = 60;

		public LevelManager LevelManager
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		public MapEntity PlayerEntity
		{ get; }

		public GameManager()
		{
			LoadMap("../../../Assets/Maps/TestMap.txt");
			PlayerEntity = new MapEntity(new Entity(), 2, 2);
			LevelManager.AddEntity(PlayerEntity);
			LevelManager.AddEntity(new MapEntity(new Entity(), 3, 3));
		}

		public void Start()
		{
			Debug.WriteLine("GameManager started.");
			IO.Render.ConsoleRenderer consoleRenderer = new IO.Render.ConsoleRenderer(new IO.Render.GameManagerRenderer(this));

			while (true)
			{
				var movementDirection = InputToDirection(PollKeyBoard());
				LevelManager.MoveEntity(PlayerEntity, movementDirection, out MapEntity? encounteredEntity);

				if (encounteredEntity != null)
				{
					try
					{
						Encounter(PlayerEntity, encounteredEntity);
					}
					catch (NotImplementedException)
					{
						Debug.WriteLine($"Encountered {encounteredEntity}.");
					}
				}
				consoleRenderer.RenderFrame();
				Thread.Sleep(1000 / GLOBAL_TICK_RATE);
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
	}
}
