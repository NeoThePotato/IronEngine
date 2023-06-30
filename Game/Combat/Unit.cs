using Game.World;
using Game.Items.Equipment;
using Assets.EquipmentTemplates;
using static Game.Combat.Stats;
using static IO.Render.EntityRenderer;
using IO.UI;

namespace Game.Combat
{
	class Unit : Entity
	{
		#region FIELDS
		// Permanent Stats (Never changes)
		private string _name = "";
		// Mid-Temporary Stats (Changes, persists between encounters)
		private int _currentHP;
		private float _currentHealingPower;
		// High-Temporary Stats (Changes, doesn't persist between encounters)
		private float _currentEvasion;
		private bool _blocking;
		// Equipment
		private Weapon? _equippedWeapon;
		private Shield? _equippedShield;
		private BodyArmor? _equippedBodyArmor;
		// Progression
		private int _level = 1;
		private int _experience = 0;
		// Other
		private VisualEntityInfo _visualInfo = Assets.EntitiesVisualInfo.UNIT_ENEMY;
		#endregion

		#region PROPERTIES
		#region NAME
		public override string Name
		{ get => _name; }
		#endregion
		#region STATS
		public Stats Stats
		{ get; private set; }
		public int MaxHP
		{ get => Stats.HP; }
		public int BaseDamage
		{ get => Stats.BaseDamage; }
		public float Evasion
		{ get => Stats.BaseEvasion; }
		public float EvasionDecay
		{ get => Stats.EvasionDecay; }
		public float BaseHealingPower
		{ get => Stats.BaseHealingPower; }
		public float HealingPowerDecay
		{ get => Stats.HealingPowerDecay; }
		public int EffectiveAttack
		{ get => BaseDamage + Weapon.Damage; }
		public int EffectiveDefense
		{ get => BodyArmor.Defense + (Blocking ? Shield.Defense : 0); }
		public int EffectiveHealPower
		{ get => (int)(CurrentHealingPower * MaxHP); }
		#region TEMP_STATS
		#region MID-TEMP_STATS
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
		#endregion
		#region HIGH-TEMP_STATS
		public float CurrentEvasion
		{
			get => _currentEvasion;
			private set => _currentEvasion = Utility.ClampRange(value, 0f, 1f);
		}
		public bool Blocking
		{
			get => _blocking;
			private set => _blocking = value;
		}
		#endregion
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
		#region PROGRESSION
		public override int Level
		{ get => _level; }
		public int TotalExp
		{ get => _experience; private set => _experience = value; }
		public int ExpToNextLevel
		{ get => NextLevelTotalExp - TotalExp; }
		public int NextLevelTotalExp
		{ get => Progression.Leveling.GetTotalExpToLevel(Level+1); }
		public bool CanLevelUp
		{ get => TotalExp >= NextLevelTotalExp; }
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
		#endregion

		#region METHODS
		#region CONSTRUCTORS
		public Unit(string name, int level, Stats stats, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor, VisualEntityInfo visualInfo)
		{
			_name = name;
			_level = level;
			Stats = stats;
			_equippedWeapon = weapon;
			_equippedShield = shield;
			_equippedBodyArmor = bodyArmor;
			_visualInfo = visualInfo;
			ResetAllTempStats();
		}

		public Unit(string name, int level, Stats stats, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor) : this(name, level, stats, weapon, shield, bodyArmor, Assets.EntitiesVisualInfo.UNIT_ENEMY)
		{

		}

		public Unit(string name, int level, int vitality, int strength, int speed, int intelligence, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor) : this(name, level, new Stats(vitality, strength, speed, intelligence), weapon, shield, bodyArmor, Assets.EntitiesVisualInfo.UNIT_ENEMY)
		{

		}

		public Unit(string name, int level, int vitality, int strength, int speed, int intelligence, Weapon? weapon, Shield? shield, BodyArmor? bodyArmor, VisualEntityInfo visualInfo) : this(name, level, new Stats(vitality, strength, speed, intelligence), weapon, shield, bodyArmor, visualInfo)
		{

		}

		public Unit(Unit other) : this(other._name, other._level, other.Stats, other._equippedWeapon, other._equippedShield, other._equippedBodyArmor, other.VisualInfo)
		{

		}
		#endregion
		#region COMBAT_ACTIONS
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

			if (!AttemptEvasion())
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
				ReduceEvasion();
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
		#endregion
		#region RESET_TEMP_STATS
		public void ResetAllTempStats()
		{
			CurrentHP = MaxHP;
			CurrentHealingPower = BaseHealingPower;
			ResetPostCombatTempStats();
		}

		public void ResetPostCombatTempStats()
		{
			CurrentEvasion = Evasion;
			Blocking = false;
		}
		#endregion
		#region PROGRESSION
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

		public void AddExp(int exp, DataLog dataLog)
		{
			if (ExpToNextLevel < exp)
				dataLog.WriteLine($"{this} has leveled up");

			TotalExp += exp;
		}

		public void LevelUp(Stat stat)
		{
			_level++;
			Stats.UpgradeStat(stat);
			ResetAllTempStats();
		}

		public int GetExpOnDeath()
		{
			return (Level * Stats.TotalBaseStats) / 4;
		}
		#endregion
		#region GET_STATS
		public string GetStats()
		{
			return $"{Name}/{MaxHP}\nLevel: {Level}\nExp: {TotalExp}/{NextLevelTotalExp}\nHP: {CurrentHP}\nBase Damage: {BaseDamage}\nHealing Power: {EffectiveHealPower} ({CurrentHealingPower * 100f:0.00}%)\nEvasion: {Evasion * 100f:0.00}%\nWeapon: {Weapon.GetStats()}\nShield: {Shield.GetStats()}\nBody Armor: {BodyArmor.GetStats()}";
		}

		public string GetCombatStats()
		{
			return $"{this}\nHP: {CurrentHP}/{MaxHP}\nAttack Power: {EffectiveAttack} ({BaseDamage}+{Weapon.Damage})\nDefense: {EffectiveDefense} ({BodyArmor.Defense}+{(Blocking ? Shield.Defense : 0)})\nEvasion: {CurrentEvasion * 100f:0.00}%\nHealing Power: {EffectiveHealPower} ({CurrentHealingPower * 100f:0.00}%)";
		}
		#endregion
		#region UTILITY
		public int CalculateTotalDamageFrom(Unit attacker)
		{
			return GetUnblockedDamage(attacker.EffectiveAttack);
		}

		private int GetUnblockedDamage(int damage)
		{
			return Utility.ClampMin(damage - EffectiveDefense, 1);
		}

		private void HealBy(int heal)
		{
			CurrentHP += heal;
		}

		private void ReduceHealingPower()
		{
			CurrentHealingPower *= 1f - HealingPowerDecay;
		}

		private bool AttemptEvasion()
		{
			return CurrentEvasion >= Random.Shared.NextDouble();
		}

		private void ReduceEvasion()
		{
			CurrentEvasion *= 1f - EvasionDecay;
		}

		private void CheckValidState()
		{
			if (Dead)
				throw new InvalidOperationException($"{this} is dead and cannot act.");
		}
		#endregion
		#endregion
	}
}
