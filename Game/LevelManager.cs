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
		public DifficultyProfile DifficultyProfile
		{ get; private set; }
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
		public LevelState State
		{ get => GetLevelState(); }
		private bool InMenu
		{ get => UIManager.InMenu; }
		private bool InEncounter
		{ get => EncounterManager != null; }
		private bool PendingExit
		{ get; set; }

		public LevelManager(GameManager gameManager, DifficultyProfile difficultyProfile)
		{
			GameManager = gameManager;
			DifficultyProfile = difficultyProfile;
			PlayerEntity = new MapEntity(new Unit(UnitTemplates.hero));
            PlayerInventory = new Container("Inventory", PLAYER_INVENTORY_SIZE);
		}

		public void Start()
		{
			EncounterManager = null;
			UIManager.ExitAllMenus();
			Level = LevelGenerator.MakeLevel((Unit)PlayerEntity.Entity, out MapEntity playerEntity, DifficultyProfile);
			PlayerEntity = playerEntity;
			DataLog.WriteLine($"{PlayerEntity} has arrived at {Level.Metadata.name}");
        }

		public void Update(ulong currentTick)
		{
			switch (State)
			{
				case LevelState.Exit:
					UpdateExit();
					return;
				case LevelState.Encounter:
					UpdateEncounter();
					break;
				case LevelState.Menu:
					UpdateUIManager();
					break;
				case LevelState.World:
					UpdateWorld();
					break;
			}
			PurgeDeadEntities();
		}

		public void MoveToNextLevel()
		{
			DifficultyProfile.RaiseLevel();
			Start();
		}

		public void Exit()
		{
			PendingExit = true;
		}

		private void UpdateWorld()
		{
			Debug.Assert(State == LevelState.World);

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
			Debug.Assert(State == LevelState.Encounter);
			EncounterManager.Update();

			if (EncounterManager.Exit)
				EncounterManager = null;
		}

		private void StartEncounter(MapEntity other)
		{
			EncounterManager = new EncounterManager(this, other.Entity);
			Debug.Assert(State == LevelState.Encounter);

			if (EncounterManager.Exit)
				EncounterManager = null;
		}

		private void UpdateUIManager()
		{
			Debug.Assert(State == LevelState.Menu);
			UIManager.Update();
		}

		private void StartUIManager()
		{
			UIManager.StackNewMenu(MenuFactory.GetInGameMenu(this));
			Debug.Assert(State == LevelState.Menu);
		}

		private void UpdateExit()
		{
			Debug.Assert(State == LevelState.Exit);
			GameManager.Exit();
		}
		#endregion

		private LevelState GetLevelState()
		{
			if (PendingExit)
				return LevelState.Exit;
			else if (InEncounter)
				return LevelState.Encounter;
			else if (InMenu)
				return LevelState.Menu;
			else
				return LevelState.World;
		}

		public enum LevelState
		{
			Exit,
			Encounter,
			Menu,
			World
		}
	}
}
