using Game.Combat;
using IO.UI;

namespace Game.World
{
	class Trap : Entity
	{
		public override string Name
		{ get; }
		public override int Level
		{ get; }
		public int Damage
		{ get; private set; }
		public bool Armed
		{ get; private set; }
        public override bool Passable
        { get => true; }
        public override bool Moveable
        { get => false; }
        public override bool MarkForDelete
		{ get => false; }
        public override EncounterManager.EncounterType EncounterType
		{ get => EncounterManager.EncounterType.Trap; }

		public Trap(string name, int level, int damage)
		{
			Name = name;
			Level = level;
			Damage = damage;
			Armed = true;
			Level = level;
		}

		public Trap(Trap other)
		{
			Name = other.Name;
			Level = other.Level;
			Damage = other.Damage;
			Armed = other.Armed;
			Level = other.Level;
		}

		public void TriggerTrap(Unit unit, DataLog dataLog)
		{
			if (Armed)
			{
				CombatFeedback combatFeedback = new CombatFeedback();
				combatFeedback.actor = this;
				combatFeedback.other = unit;
				unit.TakeDamage(Damage, ref combatFeedback);
				Armed = false;
				dataLog.WriteLine(combatFeedback.ParseFeedback());
			}
		}
	}
}
