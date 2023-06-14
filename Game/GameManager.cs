using System.Diagnostics;
using IO;
using Game.World;
using Game.Combat;
using Assets.CombatTemplates;

namespace Game
{
	class GameManager
	{
		public const int DATALOG_LENGTH = 5;
		public DataLog _dataLog;

		public LevelManager LevelManager
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		public MapEntity PlayerEntity
		{ get; private set; }
		public DataLog DataLog
		{ get => _dataLog; private set => _dataLog = value; }
		public ulong CurrentTick
		{ get; private set; }

		public GameManager()
		{
			_dataLog = new DataLog(DATALOG_LENGTH);
		}

		public void Start() // TODO Clean the class GameManager. This entire class is messy.
		{
			Debug.WriteLine("GameManager started.");
			var playerUnit = new Unit(Units.hero);
            LevelManager = LevelFactory.MakeLevel("TestMap");
			PlayerEntity = LevelManager.AddEntityAtEntryTile(playerUnit);
			DataLog.WriteLine($"{PlayerEntity} has arrived at {LevelManager.Metadata.name}");
			LevelManager.AddEntityAtRandomValidTile(Units.slime);
		}

		public void Update(ulong currentTick)
		{
			CurrentTick = currentTick;
			var movementDirection = PlayerInput.InputToDirection(PlayerInput.PollKeyBoard());
			LevelManager.MoveEntity(PlayerEntity, movementDirection, out MapEntity? encounteredEntity);

			if (encounteredEntity != null)
			{
				DataLog.WriteLine($"{PlayerEntity} has encountered a {encounteredEntity}");
				Encounter(PlayerEntity, encounteredEntity);
			}
		}

		private void Encounter(MapEntity player, MapEntity other)
		{
			new CombatManager((Unit)player.Entity, (Unit)other.Entity).Combat();
		}
	}
}
