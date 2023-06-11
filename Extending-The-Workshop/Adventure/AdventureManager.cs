using Combat;
using System;
using System.Text;
using Templates;

namespace Adventure
{
	class AdventureManager
	{

		private Unit _playerUnit;
		private Unit _finalBossUnit;
		private Unit[] _enemyPool;
		private RewardsMerchant _rewardsMerchant;

		private bool FinalBossReady
		{ get => EnemiesKilled > 10; }
		private int EnemiesKilled
		{ get; set; }
		private bool PlayerUnitDead
		{ get => _playerUnit.Dead; }
		private bool FinalBossDead
		{ get => _finalBossUnit.Dead; }
		private int EnemySelectionSize
		{ get => FinalBossReady ? 4 : 3; }

		public AdventureManager()
		{
			_playerUnit = new Unit(Units.hero);
			_finalBossUnit = new Unit(Units.tyrantKing);
			_enemyPool = new Unit[] { Units.slime, Units.fae, Units.annoyingFly, Units.imp, Units.spawnOfTwilight, Units.antiHero, Units.invincibleArchdemon, Units.tyrantKingClone};
			_rewardsMerchant = new RewardsMerchant(_playerUnit);
		}

		public void Adventure()
		{
			AdventureStart();
			AdventureLoop();
			AdventureEnd();
		}

		private void AdventureStart()
		{
			Console.Clear();
			Console.WriteLine($"Welcome, {_playerUnit.Name}.\nAre you ready to begin your adventure?\n(Press any key to continue...)");
			Utility.BlockUntilKeyDown();
		}

		private void AdventureLoop()
		{
			// Some would call this the "Core gameplay loop" :)
			while (FindEncounter() && !FinalBossDead)
			{
				_rewardsMerchant.UpgradeScreen();
			}
		}

		private void AdventureEnd()
		{
			if (PlayerUnitDead)
				Console.WriteLine($"{_playerUnit} has been defeated.");
			if (FinalBossDead)
				Console.WriteLine($"{_playerUnit} has triumphed over {_finalBossUnit}.");

			Console.WriteLine($"\nAnd so end {_playerUnit}'s adventure.\nThey have defeated {EnemiesKilled} enemies along the way.");
		}

		private bool FindEncounter()
		{
			Console.Clear();
			Console.WriteLine("You see some enemies in the distance.\nWho would you like to engage?");
			Unit enemy = PickEnemyScreen();
			Unit winner = EnterCombat(enemy);
			bool playerWins = winner == _playerUnit;

			if (playerWins)
			{
				Console.Clear();
				Console.WriteLine($"{_playerUnit} has defeated {enemy}.");
				EnemiesKilled++;
			}
			
			return playerWins;
		}

		private Unit EnterCombat(Unit enemy)
		{
			return new CombatManager(_playerUnit, enemy).Combat();
		}

		private Unit PickEnemyScreen()
		{
			Unit[] enemies = GetEnemySelection(EnemySelectionSize);
			PrintEnemySelection(enemies);

			return GetEnemySelectionPlayerInput(enemies);

		}

		private void PrintEnemySelection(Unit[] enemies)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < enemies.Length; i++)
				stringBuilder.Append($"{i+1}: {enemies[i]}\n");
			stringBuilder.Append($"{enemies.Length+1}+: ???\n");
			Console.Write(stringBuilder.ToString());
		}

		private Unit GetEnemySelectionPlayerInput(Unit[] enemies)
		{
			int input;
			while (true)
			{
				if (!int.TryParse(Console.ReadLine(), out input) || 1 > input)
				{
					(int left, int top) = Console.GetCursorPosition();
					Console.SetCursorPosition(left, top - 1);
					continue;
				}
				if (input > enemies.Length)
					return GetRandomEnemy();
				else
					return enemies[input-1];
			}
		}

		private Unit[] GetEnemySelection(int num)
		{
			Unit[] units = new Unit[num];

			for (int i = 0; i < num; i++)
				units[i] = new Unit(GetRandomEnemy());

			if (FinalBossReady)
				units[units.Length-1] = _finalBossUnit;

			return Utility.RemoveDuplicates(units);
		}

		private Unit GetRandomEnemy()
		{
			return _enemyPool[Random.Shared.Next(0, _enemyPool.Length)];
		}

	}

}
