namespace Combat.Equipment
{

	class Armor
	{
		public string Name { get; private set; }
		public int Defense { get; private set; }

		public Armor(string name, int shield)
		{
			Name = name;
			Defense = shield;
		}

		public string GetStats()
		{
			return	$"{this}\n" +
					$"	Defense: {Defense}";
		}

		public override string ToString()
		{
			return Name;
		}
	}

}
