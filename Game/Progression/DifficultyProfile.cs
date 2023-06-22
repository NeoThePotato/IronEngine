namespace Game.Progression
{
    class DifficultyProfile
    {
        private int _level; // A multiplier for all other stats (This increases as the game progressed)
        private readonly GrowthProfile _enemyGrowthProfile; // Multiply enemy Unit stats by this
        private readonly int _baseEnemyDensity; // Tiles per enemy
        private readonly int _baseTrapDensity; // Tiles per trap
        private readonly float _baseDoorChance; // Chance to generate doors in alleyways
        private readonly int _baseMaxNumOfDoors; // Max number of doors in a level

        public string Name
        { get; private set; }
        public int Level
        { get => _level; set => _level = value; }
        public GrowthProfile EnemyGrowthProfile
		{ get => _enemyGrowthProfile; }
        public int EnemyDensity
        { get => Utility.ClampMin(_baseEnemyDensity - Level / 5, 1); }
        public int TrapDensity
        { get => Utility.ClampMin(_baseTrapDensity - Level / 4, 1); }
        public float DoorChance
        { get => Utility.ClampRange(_baseDoorChance - Level / 8, 0, 1); }
        public int MaxNumOfDoors
        { get => _baseMaxNumOfDoors + Level / 10; }

        public DifficultyProfile(string name, GrowthProfile enemyGrowthProfile, int baseEnemyDensity, int baseTrapDensity, int baseDoorChance, int baseMaxNumOfDoors)
        {
            Name = name ?? string.Empty;
            Level = 1;
			_enemyGrowthProfile = enemyGrowthProfile;
            _baseEnemyDensity = baseEnemyDensity;
            _baseTrapDensity = baseTrapDensity;
            _baseDoorChance = baseDoorChance;
            _baseMaxNumOfDoors = baseMaxNumOfDoors;
        }

        public void RaiseLevel(int by = 1)
        {
            Level += by;
        }

        public override string ToString()
        {
            return
                $"Difficulty: {Name} Lv{Level}\nEnemy density: 1/{EnemyDensity} tiles\nTrap density: 1/{TrapDensity} tiles\nDoor frequency: {EnemyDensity * 100:0.00}%";
        }
    }
}
