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
		private LevelManager _levelManager;
        private Entity _encounteredEntity;
        private CombatManager? _combatManager;
		public EncounterType encounterType;

		private PlayerInputManager InputManager
		{ get => _levelManager.InputManager; }
		private GameUIManager UIManager
		{ get => _levelManager.UIManager; }
		private Container PlayerInventory
		{ get => _levelManager.PlayerInventory; }
		private Unit PlayerUnit
		{ get => (Unit)_levelManager.PlayerEntity.Entity; }
		private DataLog DataLog
		{ get => _levelManager.DataLog; }
		public bool Exit
        { get; private set; }

		public EncounterManager(LevelManager levelManager, Entity encounteredEntity)
		{
			_levelManager = levelManager;
			_encounteredEntity = encounteredEntity;
			encounterType = encounteredEntity.EncounterType;
		}

		public void Update()
		{
			Debug.Assert(!Exit);
			switch (encounterType)
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
			switch (encounterType)
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
			var unit = (Unit)_encounteredEntity;

			if (!unit.Dead)
			{
				DataLog.WriteLine($"{PlayerUnit} has encountered a {unit}");
				_combatManager = new CombatManager(PlayerUnit, unit);
			}
			else
			{
				Exit = true;
			}
		}

		private void UpdateCombat()
		{
			_combatManager.Combat(); // TODO Make CombatManager an "Update" function and call it here
			Exit = true;
		}

		private void StartContainer()
		{
			var container = (Container)_encounteredEntity;
			DataLog.WriteLine($"{PlayerUnit} has found {container}");
			UIManager.StackNewMenu(MenuFactory.GetContainerMenu(InputManager, PlayerInventory, container));
			Exit = true;
		}

		private void UpdateContainer()
		{
			Exit = true;
		}

		private void StartTrap()
		{
			var trap = (Trap)_encounteredEntity;

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
			var portal = (Portal)_encounteredEntity;

			if (portal.PortalType == PortalType.Exit)
			{
				DataLog.WriteLine($"{PlayerUnit} has found {portal}");
				UIManager.StackNewMenu(MenuFactory.GetConfirmPortalMenu(_levelManager));
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
