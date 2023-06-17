using IO;
using IO.UI;
using Game.Combat;

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
					_dataLog.WriteLine($"{_unit} has encountered a {_encounteredEntity}");
					_combatManager = new CombatManager(_unit, (Unit)_encounteredEntity);
					break;
				case EncounterType.Container:
					_dataLog.WriteLine($"{_unit} has found {_encounteredEntity}");
					_containerMenuManager = new ContainerMenuManager(_inputManager, _playerInventory, (Container)_encounteredEntity);
					break;
				case EncounterType.Trap:
					throw new NotImplementedException();
					break;
				case EncounterType.Door:
					throw new NotImplementedException();
					break;
			}
		}

		private void Update(EncounterType encounterType)
        {
            switch(encounterType)
			{
				case EncounterType.Combat:
					_combatManager.Combat(); // TODO Make CombatManager an "Update" function and call it here
					Exit = true;
					break;
				case EncounterType.Container:
					_containerMenuManager.Update();
					Exit = _containerMenuManager.Exit;
					break;
				case EncounterType.Trap:
					throw new NotImplementedException();
					break;
				case EncounterType.Door:
					throw new NotImplementedException();
					break;
			}
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
