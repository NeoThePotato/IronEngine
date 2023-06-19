using Game.Combat;
using IO.UI;

namespace Game.World
{
	class Trap : Entity
	{
		public override string Name
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

		public Trap(string name, int damage)
		{
			Name = name;
			Damage = damage;
			Armed = true;
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
