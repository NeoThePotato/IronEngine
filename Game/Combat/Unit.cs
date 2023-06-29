using Game.World;
using Game.Items.Equipment;
using Assets.EquipmentTemplates;
using static IO.Render.EntityRenderer;
using static Game.Combat.UnitStats;

namespace Game.Combat
{
    class Unit : Entity
    {
        // Permanent Stats
        private string _name = "";
        private int _level = 1;
        // Temporary Stats
        private int _currentHP;
        private float _currentHealingPower;
        private bool _blocking;
        // Equipment
        private Weapon? _equippedWeapon;
        private Shield? _equippedShield;
        private BodyArmor? _equippedBodyArmor;
        // Other
        private VisualEntityInfo _visualInfo = Assets.EntitiesVisualInfo.UNIT_ENEMY;

		#region NAME
		public override string Name
        {
            get => _name;
		}
		public override int Level
		{ get => _level; }
		#endregion
		#region COMBAT_STATS
		public UnitStats Stats
        { get; private set; }
        public int MaxHP
        { get => Stats.HP; }
        public int BaseDamage
        { get => Stats.BaseDamage; }
        public float Evasion
        { get => Stats.BaseEvasion; }
        public float MaxHealingPower
        { get => Stats.BaseHealingPower; }
        public float HealingPowerDecay
        { get => Stats.HealingPowerDecay; }
		public int EffectiveAttack
		{ get => BaseDamage + Weapon.Damage; }
		public int EffectiveDefense
		{ get => BodyArmor.Defense + (Blocking ? Shield.Defense : 0); }
		public int EffectiveBaseHealPower
		{ get => (int)(CurrentHealingPower * MaxHP); }
		#region TEMP_STATS
		public int CurrentHP
		{
			get => _currentHP;
			private set => _currentHP = Utility.ClampRange(value, 0, MaxHP);
		}
		public float CurrentHealingPower
		{
			get => _currentHealingPower;
			private set => _currentHealingPower = Utility.ClampRange(value, 0f, 1f);
		}
		public bool Blocking
		{
			get => _blocking;
			private set => _blocking = value;
		}
		#endregion
		#endregion
		#region EQUIPMENT
		public Weapon Weapon
		{
			get => _equippedWeapon ?? Weapons.nothing;
			set => _equippedWeapon = value;
		}
		public Shield Shield
		{
			get => _equippedShield ?? Shields.nothing;
			set => _equippedShield = value;
		}
		public BodyArmor BodyArmor
		{
			get => _equippedBodyArmor ?? BodyArmors.nothing;
			set => _equippedBodyArmor = value;
		}
		#endregion
		#region FLAGS
		public bool Dead
        { get => CurrentHP <= 0; }
		public override bool Passable
        { get => Dead; }
        public override bool Moveable
        { get => !Dead; }
        public override bool MarkForDelete
        { get => Dead; }
		#endregion
		#region OTHER
		public override EncounterManager.EncounterType EncounterType
        { get => EncounterManager.EncounterType.Combat; }
		public override VisualEntityInfo VisualInfo
        { get => _visualInfo; }
		#endregion

		public Unit(string name, int level, UnitStats stats, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor, VisualEntityInfo visualInfo)
        {
            _name = name;
            _level = level;
            Stats = stats;
            _equippedWeapon = weapon;
			_equippedShield = shield;
			_equippedBodyArmor = bodyArmor;
            _visualInfo = visualInfo;
            ResetTempStats();
        }

		public Unit(string name, int level, UnitStats stats, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor) : this(name, level, stats, weapon, shield, bodyArmor, Assets.EntitiesVisualInfo.UNIT_ENEMY)
		{

		}

		public Unit(string name, int level, int vitality, int strength, int speed, int intelligence, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor) : this(name, level, new UnitStats(vitality, strength, speed, intelligence), weapon, shield, bodyArmor, Assets.EntitiesVisualInfo.UNIT_ENEMY)
		{

		}

		public Unit(string name, int level, int vitality, int strength, int speed, int intelligence, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor, VisualEntityInfo visualInfo) : this(name, level, new UnitStats(vitality, strength, speed, intelligence), weapon, shield, bodyArmor, visualInfo)
		{

		}

		public Unit(Unit other) : this(other._name, other._level, other.Stats, other._equippedWeapon, other._equippedShield, other._equippedBodyArmor, other.VisualInfo)
        {

        }

        public void AttackOther(Unit other, ref CombatFeedback feedback)
        {
            CheckValidState();
            feedback.actor = this;
            feedback.other = other;
            other.TakeDamage(EffectiveAttack, ref feedback);
		}

		public void TakeDamage(int damage, ref CombatFeedback feedback)
		{
			CheckValidState();
			if (!AttemptDodge())
			{
				if (Blocking)
					feedback.type = CombatFeedback.FeedbackType.Block;
				else
					feedback.type = CombatFeedback.FeedbackType.Hit;
				int finalDamage = GetUnblockedDamage(damage);
				feedback.numericAmount = finalDamage;
				CurrentHP -= finalDamage;
				Blocking = false;
			}
			else
			{
				feedback.type = CombatFeedback.FeedbackType.Evade;
				feedback.numericAmount = 0;
			}
		}

		public void HealSelf(ref CombatFeedback feedback)
        {
            CheckValidState();
            int previousHP = CurrentHP;
            HealBy(EffectiveBaseHealPower);
            ReduceHealingPower();
            feedback.actor = this;
            feedback.other = this;
            feedback.type = CombatFeedback.FeedbackType.Heal;
            feedback.numericAmount = CurrentHP - previousHP;
        }

        public void RaiseShield(ref CombatFeedback feedback)
        {
            CheckValidState();
            Blocking = true;
            feedback.actor = this;
            feedback.type = CombatFeedback.FeedbackType.Raise;
        }

        public int CalculateTotalDamageFrom(Unit attacker)
        {
            return GetUnblockedDamage(attacker.EffectiveAttack);
        }

        public void ResetTempStats()
        {
            CurrentHP = MaxHP;
            CurrentHealingPower = MaxHealingPower;
            Blocking = false;
        }

        public void LevelUp(Stat stat)
        {
			_level++;
            Stats.UpgradeStat(stat);
		}

        public void Equip(ref Equipment? equipment)
        {
            Equipment? unEquippedItem = equipment;

            switch (equipment)
            {
                case Weapon weapon:
                    unEquippedItem = _equippedWeapon;
                    _equippedWeapon = weapon;
                    break;
				case Shield shield:
					unEquippedItem = _equippedShield;
                    _equippedShield = shield;
					break;
				case BodyArmor bodyArmor:
					unEquippedItem = _equippedBodyArmor;
                    _equippedBodyArmor = bodyArmor;
					break;
			}

			equipment = unEquippedItem;
		}

		public string GetStats()
        {
            return $"{this}" +
                $"\nHP: {CurrentHP}/{MaxHP}" +
                $"\nBase Damage: {BaseDamage}" +
                $"\nEvasion: {Evasion * 100f:0.00}%" +
                $"\nWeapon: {Weapon.GetStats()}" +
                $"\nShield: {Shield.GetStats()}" +
                $"\nBody Armor: {BodyArmor.GetStats()}" +
                $"\nHealing Power: {EffectiveBaseHealPower} ({CurrentHealingPower * 100f:0.00}%)";
        }

        public string GetCombatStats()
        {
            return $"{this}" +
                $"\nHP: {CurrentHP}/{MaxHP}" +
                $"\nAttack Power: {EffectiveAttack} ({BaseDamage}+{Weapon.Damage})" +
                $"\nDefense: {EffectiveDefense} ({BodyArmor.Defense}+{(Blocking ? Shield.Defense : 0)})" +
                $"\nEvasion: {Evasion * 100f:0.00}%" +
                $"\nHealing Power: {EffectiveBaseHealPower} ({CurrentHealingPower * 100f:0.00}%)";
        }

        private void HealBy(int heal)
        {
            CurrentHP += heal;
        }

        private void ReduceHealingPower()
        {
            CurrentHealingPower *= 1f - HealingPowerDecay;
        }

        private int GetUnblockedDamage(int damage)
        {
            return Utility.ClampMin(damage - EffectiveDefense, 1);
        }

        private bool AttemptDodge()
        {
            return Evasion >= Random.Shared.NextDouble();
        }

        private void CheckValidState()
        {
            if (Dead)
                throw new InvalidOperationException($"{this} is dead and cannot act.");
        }
    }

    struct UnitStats
    {
		#region BASE_STATS
		public int TotalBaseStats
		{ get => Vitality + Strength + Speed + Intelligence; }
		public int Vitality
        { get; private set; }
		public int Strength
		{ get; private set; }
		public int Speed
		{ get; private set; }
		public int Intelligence
		{ get; private set; }
		#endregion
		#region EXTRA_STATS
		public int HP
		{ get => Utility.ClampMin(10 * Vitality + 2 * Strength + TotalBaseStats / 2, 1); }
		public int BaseDamage
		{ get => Strength / 2 + Speed / 5 + TotalBaseStats / 10; }
		public float BaseEvasion
		{ get => Utility.ClampMax(0.02f * Speed + 0.01f * TotalBaseStats, 1f); }
		public float EvasionDecay
		{ get => Utility.ClampRange(0.7f / (float)Math.Sqrt(Speed + TotalBaseStats / 5), 0f, 1f); }
		public float BaseHealingPower
		{ get => Utility.ClampMax(0.04f * (Vitality + Intelligence), 1f); }
		public float HealingPowerDecay
        { get => Utility.ClampRange(1f / (float)Math.Sqrt(Vitality + Intelligence), 0f, 1f); }
        public int MovementSpeed
        { get => Utility.ClampRange((int)(((Speed/2f + TotalBaseStats/5f)*Point2D.POINTS_PER_TILE)/32), 16, Point2D.POINTS_PER_TILE); }
		public int DetectionRange
		{ get => (int)((Intelligence / 5f + TotalBaseStats / 10f) * Point2D.POINTS_PER_TILE); }
		#endregion

		public UnitStats(int vitality, int strength, int speed, int intelligence)
        {
            Vitality = vitality;
            Strength = strength;
            Speed = speed;
            Intelligence = intelligence;
		}

		public UnitStats(UnitStats other) : this(other.Vitality, other.Strength, other.Speed, other.Intelligence)
        {

		}

		public UnitStats(int total) : this(0, 0, 0, 0)
		{
			UpgradeRandomStat(total);
		}

		public void UpgradeStat(Stat stat, int by = 1)
		{
			switch (stat)
			{
				case Stat.VIT:
					Vitality += by;
					break;
				case Stat.STR:
					Strength += by;
					break;
				case Stat.SPD:
					Speed += by;
					break;
				case Stat.INT:
					Intelligence += by;
					break;
			}
		}

		public void UpgradeRandomStat(int total = 1)
		{
			while (total > 0)
			{
				UpgradeStat(GetRandomStat());
				total--;
			}
		}

		public static Stat GetRandomStat()
		{
			return (Stat)(Random.Shared.Next(0, Enum.GetNames(typeof(Stat)).Length));
		}

		public static UnitStats operator + (UnitStats s)
			=> s;

		public static UnitStats operator -(UnitStats s)
			=> new UnitStats(-s.Vitality, -s.Strength, -s.Speed, -s.Intelligence);

		public static UnitStats operator +(UnitStats s1, UnitStats s2)
			=> new UnitStats(s1.Vitality + s2.Vitality, s1.Strength + s2.Strength, s1.Speed + s2.Speed, s1.Intelligence + s2.Intelligence);

		public static UnitStats operator -(UnitStats s1, UnitStats s2)
			=> new UnitStats(s1.Vitality - s2.Vitality, s1.Strength - s2.Strength, s1.Speed - s2.Speed, s1.Intelligence - s2.Intelligence);

		public override string ToString()
        {
            return $"VIT: {Vitality}\nSTR: {Strength}\nSPD: {Speed}\nINT: {Intelligence}";
        }

		public enum Stat
		{
			VIT,
			STR,
			SPD,
			INT,
		}
	}

    enum UnitAction
    {
        Attack,
        Defend,
        Heal
    }
}
