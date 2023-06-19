using IO;
using IO.UI;
using Game.Combat;
using Game.Items;

namespace Game.World
{
    class EncounterManager
    {
		private PlayerInputManager _inputManager;
		private Container _playerInventory;
		private Unit _unit;
        private Entity _encounteredEntity;
        private CombatManager? _combatManager;
		private DataLog _dataLog;
		public EncounterType _encounterType;
		public ContainerMenuManager? _containerMenuManager;

		public bool Exit
        { get; private set; }

        public EncounterManager(PlayerInputManager inputManager, Container playerInventory, Unit unit, Entity encounteredEntity, DataLog dataLog)
        {
			_inputManager = inputManager;
			_playerInventory = playerInventory;
			_unit = unit;
            _encounteredEntity = encounteredEntity;
			_encounterType = encounteredEntity.EncounterType;
			_dataLog = dataLog;
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
			_containerMenuManager = new ContainerMenuManager(_inputManager, _playerInventory, container);
		}

		private void UpdateContainer()
		{
			_containerMenuManager.Update();
			Exit = _containerMenuManager.Exit;
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
			return;
		}

		private void StartDoor()
		{
			throw new NotImplementedException();
		}

		private void UpdateDoor()
		{
			throw new NotImplementedException();
		}


		public enum EncounterType
        {
            Combat,
            Container,
            Trap,
            Door
        }
    }
}
