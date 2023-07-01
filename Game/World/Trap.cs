using Game.Combat;
using IO.UI;
using static IO.Render.EntityRenderer;
using static Assets.EntitiesVisualInfo;

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
		public override VisualEntityInfo VisualInfo
		{ get => Armed ? TRAP_ARMED : TRAP_UNARMED; }

		public Trap(string name, int level, int damage)
		{
			Name = name;
			Level = level;
			Damage = damage;
			Armed = true;
		}

		public Trap(Trap other)
		{
			Name = other.Name;
			Level = other.Level;
			Damage = other.Damage;
			Armed = other.Armed;
		}

		public void TriggerTrap(Unit unit, DataLog dataLog)
		{
			if (Armed)
			{
				unit.TakeDamage(Damage, dataLog);
				Armed = false;
			}
		}
	}
}
