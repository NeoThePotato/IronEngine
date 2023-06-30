namespace Game.Items.Equipment
{
    abstract class Armor : Equipment
    {
        public override string Name { get; }
		public override int Value
		{ get => Defense; }
		public int Defense { get; private set; }

        public Armor(string name, int defense)
        {
            Name = name;
            Defense = defense;
        }

        public string GetStats()
        {
            return $"{this}\n" +
                    $" Defense: {Defense}";
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
