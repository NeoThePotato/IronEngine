using IO;
using IO.UI;
using Game.Combat;
using Game.Items;
using IO.UI.Menus;

namespace Game.World
{
    class EncounterManager
    {
		private LevelManager _levelManager;
		private PlayerInputManager _inputManager;
		private GameUIManager _uiManager;
		private Container _playerInventory;
		private Unit _unit;
        private Entity _encounteredEntity;
        private CombatManager? _combatManager;
		private DataLog _dataLog;
		public EncounterType _encounterType;

		public bool Exit
        { get; private set; }

		public EncounterManager(LevelManager levelManager, Entity encounteredEntity)
		{
			_levelManager = levelManager;
			_inputManager = levelManager.InputManager;
			_uiManager = levelManager.UIManager;
			_playerInventory = levelManager.PlayerInventory;
			_unit = (Unit)levelManager.PlayerEntity.Entity;
			_encounteredEntity = encounteredEntity;
			_encounterType = encounteredEntity.EncounterType;
			_dataLog = levelManager.DataLog;
			Start(_encounterType);
		}

        public void Update()
        {
            Update(_encounterType);
		}

		private void Start(EncounterType encounterType)
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

		private void Update(EncounterType encounterType)
        {
            switch(encounterType)
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

		private void StartCombat()
		{
			var unit = (Unit)_encounteredEntity;

			if (!unit.Dead)
			{
				_dataLog.WriteLine($"{_unit} has encountered a {unit}");
				_combatManager = new CombatManager(_unit, unit);
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
			_dataLog.WriteLine($"{_unit} has found {container}");
			_uiManager.StackNewMenu(MenuFactory.GetContainerMenu(_inputManager, _playerInventory, container));
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
				_dataLog.WriteLine($"{_unit} has stepped on {trap}");
				trap.TriggerTrap(_unit, _dataLog);
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
				_dataLog.WriteLine($"{_unit} has found {portal}");
				_uiManager.StackNewMenu(MenuFactory.GetConfirmPortalMenu(_levelManager));
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
