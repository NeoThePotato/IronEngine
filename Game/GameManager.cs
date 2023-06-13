using System.Diagnostics;
using IO;
using Game.World;
using Game.Combat;
using Assets.CombatTemplates;

namespace Game
{
	class GameManager
	{
		public LevelManager LevelManager
		{ get; private set; }
		public List<MapEntity> Entities
		{ get => LevelManager.Entities; }
		public MapEntity PlayerEntity
		{ get; private set; }

		public GameManager()
		{ }

		public void Start() // TODO Clean the class GameManager. This entire class is messy.
		{
			Debug.WriteLine("GameManager started.");
			var playerUnit = new Unit(Units.hero);
            LevelManager = LevelFactory.MakeLevel("TestMap");
			PlayerEntity = LevelManager.AddEntityAtEntryTile(playerUnit);
			LevelManager.AddEntity(new MapEntity(Units.slime, 3, 3));
		}

		public void Update()
		{
			var movementDirection = PlayerInput.InputToDirection(PlayerInput.PollKeyBoard());
			LevelManager.MoveEntity(PlayerEntity, movementDirection, out MapEntity? encounteredEntity);

			if (encounteredEntity != null)
			{
				Encounter(PlayerEntity, encounteredEntity);
			}
		}

		private void Encounter(MapEntity player, MapEntity other)
		{
			new CombatManager((Unit)player.Entity, (Unit)other.Entity).Combat();
		}
	}
}
