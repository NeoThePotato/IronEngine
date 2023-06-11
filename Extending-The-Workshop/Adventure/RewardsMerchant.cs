using Combat;
using Combat.Equipment;
using System.Text;
using Templates;

namespace Adventure
{
	class RewardsMerchant
	{

		private Unit _unit;
		private readonly Weapon[] _weaponUpgrades;
		private readonly Armor[] _shieldUpgrades;
		private readonly Armor[] _bodyArmorUpgrades;

		private bool MaxWeapon { get => _unit.Weapon == _weaponUpgrades.Last(); }
		private bool MaxShield { get => _unit.Shield == _shieldUpgrades.Last(); }
		private bool MaxBodyArmor { get => _unit.BodyArmor == _bodyArmorUpgrades.Last(); }

		public RewardsMerchant(Unit unit)
		{
			_unit = unit;
			_weaponUpgrades = new Weapon[] { Weapons.rustedBlade, Weapons.steelSword, Weapons.umbraSword, Weapons.fieryGreatsword, Weapons.swordExcalibur };
			_shieldUpgrades = new Armor[] { Armors.rustedBuckler, Armors.steelBuckler, Armors.towerShield, Armors.heroShield };
			_bodyArmorUpgrades = new Armor[] { Armors.rustedChestplate, Armors.leatherArmor, Armors.moltenArmor, Armors.mithrilChainmail, Armors.kingSlayerArmor };
		}

		public void UpgradeScreen()
		{
			PrintIntroduction();
			PrintAvailableUpgrades();
			UpgradeUnit();
			PrintNewStats();
		}

		private void PrintIntroduction()
		{
			Console.Clear();
			Console.WriteLine($"Current Stats:\n\n{_unit.GetStats()}\n\nWelcome, {_unit}.\nWhat would you like to upgrade?");
		}

		private void PrintAvailableUpgrades()
		{
			const string maxStr = " (MAX)";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("1: HP\n2: Strength\n3: Evasion\n4: Healing Power\n5: Healing Power Decay\n");
			stringBuilder.Append($"6: Weapon{(MaxWeapon? maxStr:"")}\n7: Shield{(MaxShield ? maxStr : "")}\n8: Body Armor{(MaxBodyArmor ? maxStr : "")}\n9: Skip");
			Console.WriteLine(stringBuilder.ToString());
		}

		private void UpgradeUnit()
		{
			int input;
			do
			{
				input = GetPlayerInput();
			}
			while (!Upgrade(input));
		}

		private void PrintNewStats()
		{
			Console.WriteLine($"\n{_unit}'s current stats are now:\n\n{_unit.GetStats()}\n\n(Press any key to continue...)");
			Utility.BlockUntilKeyDown();
		}

		private bool Upgrade(int input)
		{
			switch (input)
			{
				case 1:
					_unit.UpgradeStat(UnitStat.HP);
					break;
				case 2:
					_unit.UpgradeStat(UnitStat.Strength);
					break;
				case 3:
					_unit.UpgradeStat(UnitStat.Evasion);
					break;
				case 4:
					_unit.UpgradeStat(UnitStat.HealingPower);
					break;
				case 5:
					_unit.UpgradeStat(UnitStat.HealingPowerDecay);
					break;
				case 6:
					return UpgradeWeapon();
				case 7:
					return UpgradeShield();
				case 8:
					return UpgradeBodyArmor();
				default:
					break;
			}

			return true;
		}

		private int GetPlayerInput()
		{
			int input;
			while (true)
			{
				if (!int.TryParse(Console.ReadLine(), out input) || 1 > input || input > 9)
				{
					(int left, int top) = Console.GetCursorPosition();
					Console.SetCursorPosition(left, top - 1);
					continue;
				}

				return input;
			}
		}

		private bool UpgradeWeapon()
		{
			bool canUpgrade = !MaxWeapon;
			if (canUpgrade)
			{
				Weapon currentWeapon = _unit.Weapon;
				_unit.Weapon = _weaponUpgrades[Array.IndexOf(_weaponUpgrades, currentWeapon) + 1];
			}

			return canUpgrade;
		}

		private bool UpgradeShield()
		{
			bool canUpgrade = !MaxShield;
			if (canUpgrade)
			{
				Armor currentShield = _unit.Shield;
				_unit.Shield = _shieldUpgrades[Array.IndexOf(_shieldUpgrades, currentShield) + 1];
			}

			return canUpgrade;
		}

		private bool UpgradeBodyArmor()
		{
			bool canUpgrade = !MaxBodyArmor;
			if (canUpgrade)
			{
				Armor currentBodyArmor = _unit.BodyArmor;
				_unit.BodyArmor = _bodyArmorUpgrades[Array.IndexOf(_bodyArmorUpgrades, currentBodyArmor) + 1];
			}

			return canUpgrade;
		}

	}
}
