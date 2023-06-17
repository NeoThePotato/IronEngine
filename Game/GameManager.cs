using System.Diagnostics;
using IO;
using IO.UI;
using Game.World;
using Game.Combat;
using Assets.CombatTemplates;

namespace Game
{
	class GameManager
	{
		public const int DATALOG_LENGTH = 5;
		public const int PLAYER_INVENTORY_SIZE = 10;
		private DataLog _dataLog;

		public MenuManager InGameMenu
		{ get; private set; }
		public PlayerInputManager InputManager
		{ get; private set; }
		public LevelManager LevelManager
		{ get; private set; }
		public EncounterManager? EncounterManager
		{ get; private set; }
		public ContainerMenuManager? ContainerMenuManager
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		public MapEntity PlayerEntity
		{ get; private set; }
		public Container PlayerInventory
		{ get; private set; }
		public DataLog DataLog
		{ get => _dataLog; private set => _dataLog = value; }
		public ulong CurrentTick
		{ get; private set; }
		public bool StateEncounter
		{ get => EncounterManager != null; }
		public bool StateMenu
		{ get; private set; }
		public bool StateInventoryMenu
		{ get => ContainerMenuManager != null; }
		public bool StartMenuCondition
		{ get => InputManager.IsInputDown(PlayerInputManager.PlayerInputs.Start) && !StateMenu; }
		public bool Exit
		{ get; private set; }

		public GameManager()
		{
			_dataLog = new DataLog(DATALOG_LENGTH);
		}

		public void Start()
		{
			InputManager = new PlayerInputManager();
			InGameMenu = new MenuManager(InputManager, new string[] { "Return", "Stats", "Inventory", "Quit" }, 4, 1);
			var playerUnit = new Unit(Units.hero);
			LevelManager = LevelFactory.MakeLevel("TestMap");
			PlayerEntity = LevelManager.AddEntityAtEntryTile(playerUnit);
			PlayerInventory = new Container("Inventory", PLAYER_INVENTORY_SIZE);
			DataLog.WriteLine($"{PlayerEntity} has arrived at {LevelManager.Metadata.name}");

			// TODO This is a test, remove this in the final release
			//LevelManager.AddEntityAtRandomValidTile(Units.slime);
			var treasureChest = new Container("Chest", 5);
			treasureChest.TryAddItem(Armors.rustedChestplate);
			treasureChest.TryAddItem(Weapons.rustedBlade);
			LevelManager.AddEntityAtRandomValidTile(treasureChest);
		}

		public void Update(ulong currentTick)
		{
			CurrentTick = currentTick;
			InputManager.PollKeyBoard();

			if (StateEncounter) // Encounter
			{
				UpdateEncounter();
			}
			else
			{
				if (StartMenuCondition) // Enter menu
					StartInGameMenu();
				else if (StateMenu) // Update menu
					UpdateInGameMenu();
				else // World
					UpdateWorld();
			}
		}

		private void UpdateWorld()
		{
			UpdatePlayerMovement();
			// UpdateOtherEntitiesMovement();
		}

		private void UpdatePlayerMovement()
		{
			var movementDirection = InputManager.GetMovementDirection();
			LevelManager.MoveEntity(PlayerEntity, movementDirection, out MapEntity? encounteredEntity);

			if (encounteredEntity != null)
				StartEncounter(encounteredEntity);
		}

		private void UpdateOtherEntitiesMovement()
		{
			throw new NotImplementedException(); // TODO Implement entity auto-movement
		}

		private void UpdateEncounter()
		{
			Debug.Assert(EncounterManager != null);
			EncounterManager.Update();

			if (EncounterManager.Exit)
				EncounterManager = null;
		}

		private void StartEncounter(MapEntity other)
		{
			EncounterManager = new EncounterManager(InputManager, PlayerInventory, (Unit)PlayerEntity.Entity, other.Entity, ref _dataLog);
		}

		private void UpdateInGameMenu()
		{
			if (StateInventoryMenu)
			{
				UpdateContainerManager();

				if (ContainerMenuManager.Exit)
					ContainerMenuManager = null;
			}
			else
			{
				var input = InGameMenu.Update();

				if (InGameMenu.Exit)
				{
					StateMenu = false;

					return;
				}
				else
				{
					switch (input)
					{
						case "Return":
							StateMenu = false;
							break;
						case "Stats":
							throw new NotImplementedException();
							break;
						case "Inventory":
							ContainerMenuManager = new ContainerMenuManager(InputManager, PlayerInventory);
							break;
						case "Quit":
							Exit = true;
							break;
					}
				}
			}
		}

		private void UpdateContainerManager()
		{
			ContainerMenuManager.Update();
		}

		private void StartInGameMenu()
		{
			StateMenu = true;
			InGameMenu.Start();
		}
	}
}
