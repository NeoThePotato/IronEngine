﻿using Game.Combat;

namespace Game.World
{
    class EncounterManager
    {
        private Unit _unit;
        private Entity _encounteredEntity;
        private EncounterType _encounterType;
        private CombatManager? _combatManager;
		private DataLog _dataLog;


		public bool Done
        { get; private set; }

        public EncounterManager(Unit unit, Entity encounteredEntity, ref DataLog dataLog)
        {
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
				case EncounterType.Chest:
					throw new NotImplementedException();
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
					Done = true;
					break;
				case EncounterType.Chest:
					throw new NotImplementedException();
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
            Chest,
            Trap,
            Door
        }
    }
}
