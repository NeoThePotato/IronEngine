using Game.Combat;

namespace Game.Items.Equipment
{
    class Weapon : Equipment
    {
        public override string Name { get; }
        public int Damage { get; private set; }

        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
		}

		public string GetStats()
        {
            return $"{this}\n" +
                    $"	Damage: {Damage}";
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
