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
		public const int PLAYER_INVENTORY_SIZE = 10;

		public ulong CurrentTick
		{ get; private set; }
		public PlayerInputManager InputManager
		{ get; private set; }
		public GameUIManager UIManager
		{ get; private set; }
		public LevelManager LevelManager
		{ get; private set; }
		public EncounterManager? EncounterManager
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		public MapEntity PlayerEntity
		{ get; private set; }
		public Container PlayerInventory
		{ get; private set; }
		public DataLog DataLog
		{ get => UIManager.DataLog; private set => UIManager.DataLog = value; }
		public GameState State
		{ get => GetGameState(); }
		public bool PendingExit
		{ get; private set; }
		private bool InMenu
		{ get => UIManager.StateMenu; }
		private bool InEncounter
		{ get => EncounterManager != null; }

		public GameManager()
		{
			InputManager = new PlayerInputManager();
			PlayerInventory = new Container("Inventory", PLAYER_INVENTORY_SIZE);
			UIManager = new GameUIManager(this);
		}

		public void Start()
		{
			var playerUnit = new Unit(Units.hero);
			LevelManager = LevelFactory.MakeLevel("TestMap");
			PlayerEntity = LevelManager.AddEntityAtEntryTile(playerUnit);
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

			switch (State)
			{
				case GameState.Exit:
					return;
				case GameState.Encounter:
					UpdateEncounter();
					break;
				case GameState.Menu:
					UpdateUIManager();
					break;
				case GameState.World:
					UpdateWorld();
					break;
			}
		}

		public void Exit()
		{
			PendingExit = true;
		}

		private void UpdateWorld()
		{
			Debug.Assert(State == GameState.World);

			if (UIManager.StartMenuCondition) // Enter in-game menu
				StartUIManager();
			else // Update world's entities
			{
				UpdatePlayerMovement();
				// UpdateOtherEntitiesMovement();
			}
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
			Debug.Assert(State == GameState.Encounter);
			EncounterManager.Update();

			if (EncounterManager.Exit)
				EncounterManager = null;
		}

		private void StartEncounter(MapEntity other)
		{
			EncounterManager = new EncounterManager(InputManager, PlayerInventory, (Unit)PlayerEntity.Entity, other.Entity, DataLog);
			Debug.Assert(State == GameState.Encounter);
		}

		private void UpdateUIManager()
		{
			Debug.Assert(State == GameState.Menu);
			UIManager.Update();
		}

		private void StartUIManager()
		{
			UIManager.StartInGameMenu();
			Debug.Assert(State == GameState.Menu);
		}

		private GameState GetGameState()
		{
			if (PendingExit)
				return GameState.Exit;
			else if (InEncounter)
				return GameState.Encounter;
			else if (InMenu)
				return GameState.Menu;
			else
				return GameState.World;
		}

		public enum GameState
		{
			Exit,
			Encounter,
			Menu,
			World
		}
	}
}
