namespace Game.World
{
    abstract class Entity
    {
        public abstract string Name
        { get; }
        public abstract EncounterManager.EncounterType EncounterType
        { get; }
    }
}
