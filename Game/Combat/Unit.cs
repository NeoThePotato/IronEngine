using Game.World;
using Game.Items.Equipment;
using Assets.EquipmentTemplates;
using Game.Progression;
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

        public override string Name
        {
            get => _name;
        }
        public UnitStats Stats
        { get; private set; }
        public int MaxHP
        { get => Stats.HP; }
        public int BaseDamage
        { get => Stats.BaseDamage; }
        public float Evasion
        {
            get => _evasion;
            private set => _evasion = Utility.ClampRange(value, 0f, 1f);
        }
        public float MaxHealingPower
        {
            get => _maxHealingPower;
            private set => _maxHealingPower = Utility.ClampRange(value, 0f, 1f);
        }
        public float HealingPowerDecay
        {
            get => _healingPowerDecay;
            private set => _healingPowerDecay = Utility.ClampRange(value, 0f, 1f);
		}
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
        public bool Dead
        {
            get => CurrentHP == 0;
        }
        public int EffectiveAttack
        {
            get => BaseDamage + Weapon.Damage;
        }
        public int EffectiveDefense
        {
            get => BodyArmor.Defense + (Blocking ? Shield.Defense : 0);
        }
        public int EffectiveHealPower
        {
            get => (int)(CurrentHealingPower * MaxHP);
        }
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
		public override int Level
        { get => _level; }
		public override bool Passable
        { get => Dead; }
        public override bool Moveable
        { get => true; }
        public override bool MarkForDelete
        { get => Dead; }
        public override EncounterManager.EncounterType EncounterType
        { get => EncounterManager.EncounterType.Combat; }
		public override VisualEntityInfo VisualInfo
        { get => _visualInfo; }

		public Unit(string name, int level, int HP, int strength, float evasion, float initialHealingPower, float healingPowerDecay, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor, VisualEntityInfo visualInfo)
        {
            _name = name;
            _level = level;
            MaxHP = HP;
            BaseDamage = strength;
            Evasion = evasion;
            MaxHealingPower = initialHealingPower;
            HealingPowerDecay = healingPowerDecay;
            _equippedWeapon = weapon;
			_equippedShield = shield;
			_equippedBodyArmor = bodyArmor;
            _visualInfo = visualInfo;
            ResetTempStats();
        }

		public Unit(string name, int level, int HP, int strength, float evasion, float initialHealingPower, float healingPowerDecay, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor) : this(name, level, HP, strength, evasion, initialHealingPower, healingPowerDecay, weapon, shield, bodyArmor, Assets.EntitiesVisualInfo.UNIT_ENEMY)
		{

		}

		public Unit(Unit other) : this(other._name, other._level, other._maxHP, other._strength, other._evasion, other._maxHealingPower, other._healingPowerDecay, other._equippedWeapon, other._equippedShield, other._equippedBodyArmor, other.VisualInfo)
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
            HealBy(EffectiveHealPower);
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

        public void UpgradeStat(Stat stat, GrowthProfile growthProfile)
        {
			_level++;
            switch (stat)
            {
                case Stat.HP:
                    MaxHP += growthProfile.maxHP;
                    break;
                case Stat.Strength:
                    BaseDamage += growthProfile.strength;
                    break;
                case Stat.Evasion:
                    Evasion += (1f - Evasion) * growthProfile.evasion;
                    break;
                case Stat.HealingPower:
                    MaxHealingPower += (1f - MaxHealingPower) * growthProfile.maxHealingPower;
                    break;
            }
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
                $"\nHealing Power: {EffectiveHealPower} ({CurrentHealingPower * 100f:0.00}%)";
        }

        public string GetCombatStats()
        {
            return $"{this}" +
                $"\nHP: {CurrentHP}/{MaxHP}" +
                $"\nAttack Power: {EffectiveAttack} ({BaseDamage}+{Weapon.Damage})" +
                $"\nDefense: {EffectiveDefense} ({BodyArmor.Defense}+{(Blocking ? Shield.Defense : 0)})" +
                $"\nEvasion: {Evasion * 100f:0.00}%" +
                $"\nHealing Power: {EffectiveHealPower} ({CurrentHealingPower * 100f:0.00}%)";
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
		{ get => 5 * Strength + 2 * Speed + TotalBaseStats/2; }
		public float BaseEvasion
		{ get => Utility.ClampMax(0.05f * Speed + 0.01f * TotalBaseStats, 1f); }
		public float EvasionDecay
		{ get => Utility.ClampRange(0.7f / (float)Math.Sqrt(Speed + TotalBaseStats / 5), 0f, 1f); }
		public float BaseHealingPower
		{ get => Utility.ClampMax(0.04f * (Vitality + Intelligence), 1f); }
		public float HealingPowerDecay
        { get => Utility.ClampRange(1f / (float)Math.Sqrt(Vitality + Intelligence), 0f, 1f); }
        public int MovementSpeed
        { get => Utility.ClampRange(((Speed/2 + TotalBaseStats/5)*Point2D.POINTS_PER_TILE)/64, 16, Point2D.POINTS_PER_TILE); }
		public int DetectionRange
		{ get => (Intelligence / 5 + TotalBaseStats / 15) * Point2D.POINTS_PER_TILE; }
		#endregion

		public UnitStats(int vitality, int strength, int speed, int intelligence)
        {
            Vitality = vitality;
            Strength = strength;
            Speed = speed;
            Intelligence = intelligence;
        }

        public override string ToString()
        {
            return $"VIT: {Vitality}\nSTR: {Strength}\nSPD: {Speed}\nINT: {Intelligence}";
        }

		public enum Stat
		{
			Vitality,
			Strength,
			Speed,
			Intelligence,
		}
	}

    enum UnitAction
    {
        Attack,
        Defend,
        Heal
    }

    struct CombatFeedback
    {
        public Entity actor;
        public Entity other;
        public FeedbackType type;
        public int numericAmount;

        public string ParseFeedback()
        {
            switch (type)
            {
                case FeedbackType.Hit: return $"{actor} attacked {other} and dealt {numericAmount} damage.";
                case FeedbackType.Block: return $"{actor}'s attack was blocked but dealt {numericAmount} damage to {other}.";
                case FeedbackType.Evade: return $"{actor}'s attack missed {other}.";
                case FeedbackType.Raise: return $"{actor} raised their shield.";
                case FeedbackType.Heal: return $"{actor} healed for {numericAmount} HP.";
                default: return "";
            }
        }

        public enum FeedbackType
        {
            Hit,
            Block,
            Evade,
            Raise,
            Heal
        }
    }
}
