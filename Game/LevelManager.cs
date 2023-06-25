using IO;
using IO.UI;
using IO.UI.Menus;
using Game.Items;
using Game.World;
using Game.Combat;
using Assets;
using Assets.Generators;
using System.Diagnostics;
using Game.Progression;

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

		public LevelManager(GameManager gameManager, DifficultyProfile difficultyProfile)
		{
			GameManager = gameManager;
			var playerUnit = new Unit(UnitTemplates.hero);
            PlayerInventory = new Container("Inventory", PLAYER_INVENTORY_SIZE);
            MapEntity playerEntity;
			Level = LevelGenerator.MakeLevel(playerUnit, out playerEntity, difficultyProfile);
			PlayerEntity = playerEntity;
		}

		public void Start()
        {
            DataLog.WriteLine($"{PlayerEntity} has arrived at {Level.Metadata.name}");
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

		private void PurgeDeadEntities()
		{
			for (int i = Entities.Count - 1; i > 0; i--)
			{
				if (Entities[i].Entity.MarkForDelete)
					Entities.RemoveAt(i);
			}
		}

		#region ENTITY_BEHAVIOUR
		private void UpdatePlayerMovement()
		{
			var movementDirection = new Direction(InputManager.GetMovementVector(Point2D.POINTS_PER_TILE));
			Level.MoveEntity(PlayerEntity, movementDirection, out MapEntity? encounteredEntity);

			if (encounteredEntity != null)
				StartEncounter(encounteredEntity);
		}

		private void UpdateOtherEntitiesMovement()
		{
			foreach (var entity in Entities)
			{
				if (entity.Moveable && entity != PlayerEntity)
					AutoMoveEntity(entity);
			}
		}

		private void AutoMoveEntity(MapEntity entity)
		{
			if (EntityDetectsPlayer(entity)) // Track player if in range
			{
				if (!entity.IsTargeting) // Found player
					DataLog.WriteLine($"{entity} has spotted {PlayerEntity}");

				entity.Target = PlayerEntity;
			}
			else
			{
				if (entity.IsTargeting) // Lost player
					DataLog.WriteLine($"{entity} has lost sight of {entity.Target}");

				entity.Target = null;
			}

			if (entity.Dir.Mag == 0)
                entity.Dir = Direction.GetRandomDirection(); // Start moving in random direction

			if (!Level.MoveEntity(entity, out MapEntity? encounteredEntity))
				entity.Dir = Direction.GetRandomDirection(); // Move or change direction

            if (encounteredEntity == PlayerEntity)
                StartEncounter(entity); // Encounter player
        }

		private bool EntityDetectsPlayer(MapEntity entity)
		{
			return entity.OtherInDetectionRange(PlayerEntity) &&  Level.CanEntityMoveTo(entity, PlayerEntity);
		}
		#endregion

		#region ENCOUNTERS_LOGIC
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
		#endregion

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
