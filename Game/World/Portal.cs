namespace Game.World
{
	class Portal : Entity
	{
		public PortalType PortalType
		{ get; private set; }
		public override string Name
		{ get => $"{PortalType} Portal"; }
		public override int Level
		{ get; }
		public override bool Passable
		{ get => true; }
		public override bool Moveable
		{ get => false; }
		public override bool MarkForDelete
		{ get => false; }
		public override EncounterManager.EncounterType EncounterType
		{ get => EncounterManager.EncounterType.Portal; }

		public Portal(PortalType portalType, int level = 1)
		{
			PortalType = portalType;
			Level = level;
		}
	}

	enum PortalType
	{
		Entry,
		Exit
	}
}
