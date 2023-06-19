namespace Game.Items
{
    abstract class Item
    {
        public abstract string Name
        { get; }
        public virtual int Value
        { get => 1; }

        public override string ToString()
        {
            return Name;
        }
    }
}
