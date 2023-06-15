namespace Game
{
	abstract class Item
	{
		public abstract string Name
		{ get; }
		public virtual int Count
		{ get => 1; }
		public virtual int Value
		{ get => 1; }
	}
}
