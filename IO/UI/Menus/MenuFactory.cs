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
			Action levelUp = () => throw new NotImplementedException(); // TODO Create "LevelUp" menu

			var actions = new Dictionary<string, Action?>()
			{
				{"Back", back}
			};

			if (playerUnit.CanLevelUp)
				actions.Add("Level Up", levelUp);

			return new SelectionMenu(inputManager, parentUIManager, actions, 1, actions.Count, statsMenuText);
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
