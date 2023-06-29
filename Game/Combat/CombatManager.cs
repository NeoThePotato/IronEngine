using Game.World;
using IO.UI;

namespace Game.Combat
{
    class CombatManager
    {
        private Unit PlayerUnit
        { get; set; }
        private Unit CPUUnit
		{ get; set; }
		private DataLog DataLog
		{ get; set; }
        private CPUAI AI
        { get; set; }

		private bool BothAlive
        { get => !PlayerUnit.Dead && !CPUUnit.Dead; }

        public CombatManager(Unit playerUnit, Unit cpuUnit, DataLog dataLog)
        {
            PlayerUnit = playerUnit;
            CPUUnit = cpuUnit;
            DataLog = dataLog;
            AI = new CPUAI(CPUUnit, PlayerUnit);
		}

        public Unit Combat()
        {
            CombatInitiate();
            CombatLoop();
            CombatEnd();

            return GetWinner();
        }

        private void CombatInitiate()
		{
			Console.Clear();
			Console.WriteLine($"{PlayerUnit} has encountered a {CPUUnit}.");
            Utility.BlockUntilKeyDown();
        }

        private void CombatLoop()
        {
            Unit actingUnit = PlayerUnit;
            Unit passiveUnit = CPUUnit;
            CombatFeedback combatFeedback = new CombatFeedback();

            // Main combat loop
            while (BothAlive)
            {
				// Print combat status
				Console.Clear();
                Console.WriteLine(
                    $"{PlayerUnit.GetCombatStats()}\n\n" +
                    $"{CPUUnit.GetCombatStats()}\n\n" +
                    $"{actingUnit}'s turn.\n");

                // Action phase
                UnitAction action = actingUnit == PlayerUnit ? GetPlayerAction() : GetCPUAction();
                DoAction(action, actingUnit, passiveUnit, ref combatFeedback);

                // End of action phase
                Console.WriteLine(combatFeedback.ParseFeedback());
                Utility.BlockUntilKeyDown();

                // Prepare for next turn
                Utility.Swap(ref actingUnit, ref passiveUnit);
            }
        }

        private void CombatEnd()
        {
            Unit winner = GetWinner();
			DataLog.WriteLine($"{winner} wins!");
			winner.ResetPostCombatTempStats();
		}

        private UnitAction GetPlayerAction()
        {
			Console.WriteLine(
                $"What will {PlayerUnit} do?\n" +
                $"{GetPlayerOptions()}");

            return GetPlayerInput() switch
            {
                1 => UnitAction.Attack,
                2 => UnitAction.Defend,
                3 => UnitAction.Heal,
                _ => UnitAction.Attack,
            };
        }

        private UnitAction GetCPUAction()
        {
            Thread.Sleep(300);

            return AI.GetCPUAction();
        }

        private void DoAction(UnitAction action, Unit actingUnit, Unit passiveUnit, ref CombatFeedback feedback)
        {
            switch (action)
            {
                case UnitAction.Attack:
                    actingUnit.AttackOther(passiveUnit, ref feedback);
                    break;
                case UnitAction.Defend:
                    actingUnit.RaiseShield(ref feedback);
                    break;
                case UnitAction.Heal:
                    actingUnit.HealSelf(ref feedback);
                    break;
            }
        }

        private string GetPlayerOptions()
        {
            return "1: Attack\n" +
                    "2: Defend\n" +
                    "3: Heal";
        }

        private int GetPlayerInput()
        {
            int input;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out input) || 1 > input || input > 3)
                {
                    (int left, int top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(left, top - 1);
                    continue;
                }
                return input;
            }
        }

        private Unit GetWinner()
        {
            return CPUUnit.Dead ? PlayerUnit : CPUUnit;
        }
	}

	enum UnitAction
	{
		Attack,
		Defend,
		Heal
	}

	struct CPUAI
    {
        private Unit PlayerUnit;
        private Unit CPUUnit;

        // Flags useful for AI decision-making
        private int DamageToPlayer
        {
            get => PlayerUnit.CalculateTotalDamageFrom(CPUUnit);
        }
        private int DamageFromPlayer
        {
            get => CPUUnit.CalculateTotalDamageFrom(PlayerUnit);
        }
        private bool PlayerDiesIfHit
        {
            get => DamageToPlayer >= PlayerUnit.CurrentHP;
        }
        private bool CPUDiesIfHit
        {
            get => DamageFromPlayer >= CPUUnit.CurrentHP;
        }
        private bool CPUCanHeal
        {
            get => CPUUnit.EffectiveBaseHealPower > 0;
        }
        private bool CPUCanBlock
        {
            get => !CPUUnit.Blocking;
        }
        private bool CPUCanOutHealFatalAttack
        {
            get => CPUCanHeal && CPUUnit.CurrentHP + CPUUnit.EffectiveBaseHealPower - DamageFromPlayer > 0;
        }
        private bool CPUCanBlockFatalAttack
        {
            get => CPUCanBlock && CPUUnit.CurrentHP + CPUUnit.EffectiveDefense + CPUUnit.Shield.Defense - DamageFromPlayer > 0;
        }
        private bool CPUShouldHeal
        {
            get => CPUDiesIfHit && CPUCanOutHealFatalAttack;
        }
        private bool CPUShouldAttack
        {
            get => PlayerDiesIfHit || !CPUDiesIfHit;
        }
        private bool CPUShouldBlock
        {
            get => CPUDiesIfHit && CPUCanBlockFatalAttack;
        }

        public CPUAI(Unit cpuUnit, Unit playerUnit)
        {
            CPUUnit = cpuUnit;
            PlayerUnit = playerUnit;
        }

        public UnitAction GetCPUAction()
        {
            if (CPUShouldHeal)
                return UnitAction.Heal; // Highest priority
            else if (CPUShouldAttack)
                return UnitAction.Attack; // Usually happens
            else if (CPUShouldBlock)
                return UnitAction.Defend; // Happens when healing runs out
            else
                return UnitAction.Attack; // Desperate efforts
        }
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
