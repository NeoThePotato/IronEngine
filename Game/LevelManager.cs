using IO;
using IO.UI;
using IO.UI.Menus;
using Game.Items;
using Game.World;
using Game.Combat;
using Assets;
using Assets.EquipmentTemplates;
using System.Diagnostics;

namespace Game
{
    class LevelManager
	{
		public const int PLAYER_INVENTORY_SIZE = 10;

		public GameManager GameManager
		{ get; private set; }
		public PlayerInputManager InputManager
		{ get => GameManager.InputManager; }
		public GameUIManager UIManager
		{ get => GameManager.UIManager; }
		public DataLog DataLog
		{ get => UIManager.DataLog; }
		public Level Level
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => Level.Entities; }
		public MapEntity PlayerEntity
		{ get; private set; }
		public Container PlayerInventory
		{ get; private set; }
		public EncounterManager? EncounterManager
		{ get; private set; }
		public GameState State
		{ get => GetGameState(); }
		private bool InMenu
		{ get => UIManager.InMenu; }
		private bool InEncounter
		{ get => EncounterManager != null; }
		private bool PendingExit
		{ get; set; }

		public LevelManager(GameManager gameManager)
		{
			GameManager = gameManager;
			var playerUnit = new Unit(UnitTemplates.hero);
			Level = LevelFactory.MakeLevel("TestMap");
			PlayerEntity = Level.AddEntityAtEntryTile(playerUnit);
			PlayerInventory = new Container("Inventory", PLAYER_INVENTORY_SIZE);
			DataLog.WriteLine($"{PlayerEntity} has arrived at {Level.Metadata.name}");
		}

		public void Start()
		{
			// TODO This is a test, remove this in the final release
			Level.AddEntityAtRandomValidTile(UnitTemplates.slime);
			Level.AddEntityAtEntryTile(Assets.TrapsTemplates.firePit);
			var treasureChest = new Container("Chest", 5);
			treasureChest.TryAddItem(Armors.rustedChestplate);
			treasureChest.TryAddItem(Weapons.rustedBlade);
			Level.AddEntityAtRandomValidTile(treasureChest);
		}

		public void Update(ulong currentTick)
		{
			switch (State)
			{
				case GameState.Exit:
					UpdateExit();
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
			PurgeDeadEntities();
        }

		private void PurgeDeadEntities()
		{
			for (int i = 0; i < Entities.Count; i++)
			{
				if (Entities[i].Entity.MarkForDelete)
					Entities.RemoveAt(i);
			}
		}

		private void UpdateWorld()
		{
			Debug.Assert(State == GameState.World);

			if (UIManager.StartMenuCondition) // Enter in-game menu
				StartUIManager();
			else // Update world's entities
			{
				UpdatePlayerMovement();
				UpdateOtherEntitiesMovement();
			}
		}

		private void UpdatePlayerMovement()
		{
			var movementDirection = InputManager.GetMovementDirection();
			Level.MoveEntity(PlayerEntity, movementDirection, out MapEntity? encounteredEntity);

			if (encounteredEntity != null)
				StartEncounter(encounteredEntity);
		}

		private void UpdateOtherEntitiesMovement()
		{
			foreach (var entity in Entities)
			{
				if (entity.Entity.Moveable && entity != PlayerEntity)
					AutoMoveEntity(entity);
			}
		}

		private void AutoMoveEntity(MapEntity entity)
		{
			if (entity.Dir == Direction.Directions.None)
                entity.Dir = Direction.GetRandomDirection(); // Start moving

			if (!Level.MoveEntity(entity, out MapEntity? encounteredEntity))
				entity.Dir = Direction.GetRandomDirection(); // Move or change direction

            if (encounteredEntity == PlayerEntity)
                StartEncounter(entity); // Encounter player

            // TODO Follow player if in range
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
			EncounterManager = new EncounterManager(this, other.Entity);
			Debug.Assert(State == GameState.Encounter);

			if (EncounterManager.Exit)
				EncounterManager = null;
		}

		private void UpdateUIManager()
		{
			Debug.Assert(State == GameState.Menu);
			UIManager.Update();
		}

		private void StartUIManager()
		{
			UIManager.StackNewMenu(MenuFactory.GetInGameMenu(this));
			Debug.Assert(State == GameState.Menu);
		}

		private void UpdateExit()
		{
			Debug.Assert(State == GameState.Exit);
			GameManager.Exit();
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

		public void Exit()
		{
			PendingExit = true;
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
