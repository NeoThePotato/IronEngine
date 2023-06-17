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

		public PlayerInputManager InputManager
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
		{ get => _dataLog; private set => _dataLog = value; }
		public ulong CurrentTick
		{ get; private set; }
		public bool Exit
		{ get; private set; }

		public GameManager()
		{
			_dataLog = new DataLog(DATALOG_LENGTH);
		}

		public void Start() // TODO Clean the class GameManager. This entire class is messy.
		{
			Debug.WriteLine("GameManager started.");
			InputManager = new PlayerInputManager();
			var playerUnit = new Unit(Units.hero);
            LevelManager = LevelFactory.MakeLevel("TestMap");
			PlayerEntity = LevelManager.AddEntityAtEntryTile(playerUnit);
			PlayerInventory = new Container("Inventory", PLAYER_INVENTORY_SIZE);
			DataLog.WriteLine($"{PlayerEntity} has arrived at {LevelManager.Metadata.name}");
			LevelManager.AddEntityAtRandomValidTile(Units.slime);
		}

		public void Update(ulong currentTick)
		{
			CurrentTick = currentTick;
			InputManager.PollKeyBoard();

			if (EncounterManager != null) // Encounter
			{
				UpdateEncounter();
			}
			else // World
			{
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

			if (EncounterManager.Done)
				EncounterManager = null;
		}

		private void StartEncounter(MapEntity other)
		{
			EncounterManager = new EncounterManager((Unit)PlayerEntity.Entity, other.Entity, ref _dataLog);
		}
	}
}
