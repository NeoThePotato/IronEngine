using Game;
using Game.Items;

namespace IO.UI.Menus
{
	static class MenuFactory
	{
		public static SelectionMenu GetConfirmPortalMenu(LevelManager levelManager)
		{
			Action onTrue = () => levelManager.MoveToNextLevel();
			Action onFalse = () => levelManager.UIManager.ForceExitCurrentMenu();

			return GetConfirmMenu(levelManager.InputManager, "Are you sure you want to proceed?", onTrue, onFalse);
		}

		public static SelectionMenu GetInGameMenu(LevelManager levelManager)
		{
			Action returnToGame = () => levelManager.UIManager.ForceExitCurrentMenu(); // This is some cyclical shit
			Action openStatsMenu = () => throw new NotImplementedException();
			Action openInventoryMenu = () => levelManager.UIManager.StackNewMenu(GetContainerMenu(levelManager.InputManager, levelManager.PlayerInventory));
			Action quitGame = () => levelManager.Exit();

			var actions = new Dictionary<string, Action?>()
			{
				{ "Return", returnToGame},
				{ "Stats", openStatsMenu},
				{ "Inventory", openInventoryMenu},
				{ "Quit", quitGame}
			};

			return new SelectionMenu(levelManager.InputManager, actions, 4, 1);
		}

		public static ContainerMenu GetContainerMenu(PlayerInputManager inputManager, params Container[] containers)
		{
			return new ContainerMenu(inputManager, containers);
		}

		private static SelectionMenu GetConfirmMenu(PlayerInputManager inputManager, string query, Action onTrue, Action onFalse)
		{
			var actions = new Dictionary<string, Action?>()
			{
				{ "Yes", onTrue},
				{ "No", onFalse},
			};

			return new SelectionMenu(inputManager, actions, 1, 2, query);
		}
	}
}
