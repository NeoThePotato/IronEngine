namespace Game.World
{
    abstract class Entity
    {
        public abstract string Name
        { get; }
        public abstract bool Passable
        { get; }
        public abstract EncounterManager.EncounterType EncounterType
        { get; }

		public override string ToString()
		{
            return Name;
		}
	}
}
