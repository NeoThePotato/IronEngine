using Game;
using Game.Items;

namespace IO.UI.Menus
{
	static class MenuFactory
	{
		public static SelectionMenu GetInGameMenu(LevelManager levelManager)
		{
			Action returnToGame = () => levelManager.UIManager.ForceExitCurrentMenu(); /// This is some cyclical shit
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
	}
}
