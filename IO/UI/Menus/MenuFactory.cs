using Game;
using Game.Combat;
using Game.Items;
using Game.Items.Equipment;

namespace IO.UI.Menus
{
	static class MenuFactory
	{
		public static SelectionMenu GetConfirmPortalMenu(LevelManager levelManager)
		{
			var parentUIManager = levelManager.UIManager;
			Action onTrue = () => levelManager.MoveToNextLevel();
			Action onFalse = () => levelManager.UIManager.ForceExitCurrentMenu();

			return GetConfirmMenu(levelManager.InputManager, parentUIManager, "Are you sure you want to proceed?", onTrue, onFalse);
		}

		public static SelectionMenu GetInGameMenu(LevelManager levelManager)
		{
			var parentUIManager = levelManager.UIManager;
			Action returnToGame = () => parentUIManager.ForceExitCurrentMenu();
			Action openStatsMenu = () => levelManager.UIManager.StackNewMenu(GetPlayerStatsMenu(levelManager.InputManager, levelManager.UIManager, (Unit)levelManager.PlayerEntity.Entity));
			Action openInventoryMenu = () => levelManager.UIManager.StackNewMenu(GetContainerMenu(levelManager.InputManager, levelManager.UIManager, (Unit)levelManager.PlayerEntity.Entity, levelManager.PlayerInventory));
			Action quitGame = () => levelManager.Exit();

			var actions = new Dictionary<string, Action?>()
			{
				{ "Return", returnToGame},
				{ "Stats", openStatsMenu},
				{ "Inventory", openInventoryMenu},
				{ "Quit", quitGame}
			};

			return new SelectionMenu(levelManager.InputManager, parentUIManager, actions, 4, 1);
		}

		public static ContainerMenu GetContainerMenu(PlayerInputManager inputManager, GameUIManager parentUIManager, Unit playerUnit, params Container[] containers)
		{
			return new ContainerMenu(inputManager, parentUIManager, playerUnit, containers);
		}

		public static SelectionMenu GetEquipmentMenu(PlayerInputManager inputManager, GameUIManager parentUIManager, ContainerMenu parent, Equipment equipment, Unit playerUnit)
		{
			Action equip = () => { parent.EquipSelectedOnUnit(playerUnit); parentUIManager.ForceExitCurrentMenu(); };
			Action discard = () => { parent.RemoveItemAtSelection(); parentUIManager.ForceExitCurrentMenu(); };

			var actions = new Dictionary<string, Action?>()
			{
				{ "Equip", equip},
				{ "Discard", discard},
			};

			return new SelectionMenu(inputManager, parentUIManager, actions, 2, 1, equipment.Name);
		}

		public static SelectionMenu GetPlayerStatsMenu(PlayerInputManager inputManager, GameUIManager parentUIManager, Unit playerUnit)
		{
			var statsMenuText = playerUnit.GetStats();
			Action back = () => parentUIManager.ForceExitCurrentMenu();
			Action levelUp = () => parentUIManager.StackNewMenu(GetPlayerLevelUpMenu(inputManager, parentUIManager, playerUnit));

			var actions = new Dictionary<string, Action?>()
			{
				{"Back", back}
			};

			if (playerUnit.CanLevelUp)
				actions.Add("Level Up", levelUp);

			return new SelectionMenu(inputManager, parentUIManager, actions, 1, actions.Count, "Stats", statsMenuText);
		}

		public static SelectionMenu GetPlayerLevelUpMenu(PlayerInputManager inputManager, GameUIManager parentUIManager, Unit playerUnit)
		{
			var unitStats = playerUnit.Stats;
			var level = playerUnit.Level;
			Action vitUp = () => { playerUnit.LevelUp(Stats.Stat.VIT); parentUIManager.ForceExitCurrentMenu(); };
			Action strUp = () => { playerUnit.LevelUp(Stats.Stat.STR); parentUIManager.ForceExitCurrentMenu(); };
			Action spdUp = () => { playerUnit.LevelUp(Stats.Stat.SPD); parentUIManager.ForceExitCurrentMenu(); };
			Action intUp = () => { playerUnit.LevelUp(Stats.Stat.INT); parentUIManager.ForceExitCurrentMenu(); };
			Action back = () => parentUIManager.ForceExitCurrentMenu();

			var actions = new Dictionary<string, Action?>()
			{
				{ $"VIT: {unitStats.Vitality} +1", vitUp},
				{ $"STR: {unitStats.Strength} +1", strUp},
				{ $"SPD: {unitStats.Speed} +1", spdUp},
				{ $"INT: {unitStats.Intelligence} +1", intUp},
				{"Back", back}
			};

			return new SelectionMenu(inputManager, parentUIManager, actions, 4, actions.Count, $"Lv {level} -> {level+1}");
		}

		private static SelectionMenu GetConfirmMenu(PlayerInputManager inputManager, GameUIManager parentUIManager, string query, Action onTrue, Action onFalse)
		{
			var actions = new Dictionary<string, Action?>()
			{
				{ "Yes", onTrue},
				{ "No", onFalse},
			};

			return new SelectionMenu(inputManager, parentUIManager, actions, 1, 2, query);
		}
	}
}
