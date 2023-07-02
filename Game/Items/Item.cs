using Game.Combat;
using IO.UI;

namespace Game.Items
{
	abstract class Item
	{
		public abstract string Name
		{ get; }
		public virtual int Value
		{ get => 1; }
		public virtual int ExpOnDiscard
		{ get => Value; }

		public void DiscardThis(Unit unit, DataLog dataLog)
		{
			dataLog.WriteLine($"{unit} has destroyed {this} and gained {ExpOnDiscard} Exp");
			unit.AddExp(ExpOnDiscard, dataLog);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
