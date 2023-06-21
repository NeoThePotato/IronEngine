namespace Game.World
{
	class Portal : Entity
	{
		public PortalType PortalType
		{ get; private set; }

		public override string Name
		{ get => $"{PortalType} Portal"; }

		public override bool Passable
		{ get => true; }

		public override bool Moveable
		{ get => false; }

		public override bool MarkForDelete
		{ get => false; }

		public override EncounterManager.EncounterType EncounterType
		{ get => EncounterManager.EncounterType.Portal; }

		public Portal(PortalType portalType)
		{
			PortalType = portalType;
		}
	}

	enum PortalType
	{
		Entry,
		Exit
	}
}
