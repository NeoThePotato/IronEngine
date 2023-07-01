using IO;
using IO.UI;
using Game.Combat;
using Game.Items;
using IO.UI.Menus;
using System.Diagnostics;

namespace Game.World
{
    class EncounterManager
    {
		private LevelManager LevelManager
		{ get; set; }
        private Entity EncounteredEntity
		{ get; set; }
        private CombatManager? CombatManager
		{ get; set; }
		public EncounterType Type
		{ get => EncounteredEntity.EncounterType; }
		private PlayerInputManager InputManager
		{ get => LevelManager.InputManager; }
		private GameUIManager UIManager
		{ get => LevelManager.UIManager; }
		private Container PlayerInventory
		{ get => LevelManager.PlayerInventory; }
		private Unit PlayerUnit
		{ get => (Unit)LevelManager.PlayerEntity.Entity; }
		private DataLog DataLog
		{ get => LevelManager.DataLog; }
		public bool Exit
        { get; private set; }

		public EncounterManager(LevelManager levelManager, Entity encounteredEntity)
		{
			LevelManager = levelManager;
			EncounteredEntity = encounteredEntity;
		}

		public void Update()
		{
			Debug.Assert(!Exit);
			switch (Type)
			{
				case EncounterType.Combat:
					UpdateCombat();
					break;
				case EncounterType.Container:
					UpdateContainer();
					break;
				case EncounterType.Trap:
					UpdateTrap();
					break;
				case EncounterType.Door:
					UpdateDoor();
					break;
				case EncounterType.Portal:
					UpdatePortal();
					break;
			}
		}

		public void Start()
		{
			switch (Type)
			{
				case EncounterType.Combat:
					StartCombat();
					break;
				case EncounterType.Container:
					StartContainer();
					break;
				case EncounterType.Trap:
					StartTrap();
					break;
				case EncounterType.Door:
					StartDoor();
					break;
				case EncounterType.Portal:
					StartPortal();
					break;
			}
		}

		private void StartCombat()
		{
			var unit = (Unit)EncounteredEntity;

			if (!unit.Dead)
			{
				DataLog.WriteLine($"{PlayerUnit} has encountered a {unit}");
				CombatManager = new CombatManager(PlayerUnit, unit, DataLog);
			}
			else
			{
				Exit = true;
			}
		}

		private void UpdateCombat()
		{
			CombatManager.Combat(); // TODO Make CombatManager an "Update" function and call it here
			Exit = true;
		}

		private void StartContainer()
		{
			var container = (Container)EncounteredEntity;
			DataLog.WriteLine($"{PlayerUnit} has found {container}");
			UIManager.StackNewMenu(MenuFactory.GetContainerMenu(InputManager, UIManager, PlayerUnit, PlayerInventory, container));
			Exit = true;
		}

		private void UpdateContainer()
		{
			Exit = true;
		}

		private void StartTrap()
		{
			var trap = (Trap)EncounteredEntity;

			if (trap.Armed)
			{
				DataLog.WriteLine($"{PlayerUnit} has stepped on {trap}");
				trap.TriggerTrap(PlayerUnit, DataLog);
			}
			Exit = true;
		}

		private void UpdateTrap()
		{
			Exit = true;
		}

		private void StartDoor()
		{
			throw new NotImplementedException();
		}

		private void UpdateDoor()
		{
			throw new NotImplementedException();
		}

		private void StartPortal()
		{
			var portal = (Portal)EncounteredEntity;

			if (portal.PortalType == PortalType.Exit)
			{
				DataLog.WriteLine($"{PlayerUnit} has found {portal}");
				UIManager.StackNewMenu(MenuFactory.GetConfirmPortalMenu(LevelManager));
			}
			Exit = true;
		}

		private void UpdatePortal()
		{
			Exit = true;
		}

		public enum EncounterType
        {
            Combat,
            Container,
            Trap,
            Door,
			Portal
        }
    }
}
