namespace Game.Progression
{
    class DifficultyProfile
    {
        private int _level; // A multiplier for all other stats (This increases as the game progressed)
        private readonly int _baseEnemyDensity; // Tiles per enemy
        private readonly int _baseTrapDensity; // Tiles per trap
        private readonly float _baseDoorChance; // Chance to generate doors in alleyways
        private readonly int _baseMaxNumOfDoors; // Max number of doors in a level
		private readonly float _baseChestChance; // Chance to generate chests in corners
		private readonly int _baseMaxNumOfChests; // Max number of chests in a level
		private readonly int _finalBossSpawnLevel; // The level at which the final boss starts appearing

		public string Name
        { get; private set; }
        public int Level
        { get => _level; set => _level = value; }
        public int EnemyDensity
        { get => Utility.ClampMin(_baseEnemyDensity - Level / 5, 1); }
        public int TrapDensity
        { get => Utility.ClampMin(_baseTrapDensity - Level / 4, 1); }
        public float DoorChance
        { get => Utility.ClampRange(_baseDoorChance - Level / 8, 0, 1); }
        public int MaxNumOfDoors
        { get => _baseMaxNumOfDoors + Level / 10; }
		public float ChestChance
		{ get => Utility.ClampRange(_baseChestChance - Level / 8, 0, 1); }
		public int MaxNumOfChests
		{ get => _baseMaxNumOfChests + Level / 10; }
        public bool FinalBossCanSpawn
        { get => _finalBossSpawnLevel <= Level; }

		public DifficultyProfile(string name, int baseEnemyDensity, int baseTrapDensity, float baseDoorChance, int baseMaxNumOfDoors, float baseChestChance, int baseMaxNumOfChests, int finalBossSpawnLevel)
        {
            Name = name ?? string.Empty;
            Level = 1;
            _baseEnemyDensity = baseEnemyDensity;
            _baseTrapDensity = baseTrapDensity;
            _baseDoorChance = baseDoorChance;
            _baseMaxNumOfDoors = baseMaxNumOfDoors;
            _baseChestChance = baseChestChance;
            _baseMaxNumOfChests = baseMaxNumOfChests;
            _finalBossSpawnLevel = finalBossSpawnLevel;
        }

        public void RaiseLevel(int by = 1)
        {
            Level += by;
        }

        public override string ToString()
        {
            return
                $"Difficulty: {Name} Lv{Level}\nEnemy density: 1/{EnemyDensity} tiles\nTrap density: 1/{TrapDensity} tiles\nDoor frequency: {DoorChance * 100:0.00}%\nChest frequency: {ChestChance * 100:0.00}%";
        }
    }
}
