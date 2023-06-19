using Game;
using Game.Items;
using IO.UI.Menus;
using System.Diagnostics;

namespace IO.UI
{
    class GameUIManager
	{
		public const int DATALOG_LENGTH = 5;
		public const int DEFAULT_MENU_STACK_SIZE = 5;

		public Stack<Menu> MenuStack
		{ get; private set; }
		public DataLog DataLog
		{ get; set; }
		public PlayerInputManager InputManager
		{ get => GameManager.InputManager; }
		public SelectionMenu InGameMenu
		{ get; private set; }
		public ContainerMenu? ContainerMenuManager
		{ get; private set; }
		public Container PlayerInventory
		{ get => GameManager.LevelManager.PlayerInventory; }
		public bool InMenu
		{ get => MenuStack.Count > 0; }
		public bool StateInventoryMenu
		{ get => ContainerMenuManager != null; }
		public bool StartMenuCondition
		{ get => InputManager.IsInputDown(PlayerInputManager.PlayerInputs.Start) && !InMenu; }
		public GameManager GameManager
		{ get; private set; }

		public GameUIManager(GameManager gameManager)
		{
			GameManager = gameManager;
			MenuStack = new Stack<Menu>(DEFAULT_MENU_STACK_SIZE);
			DataLog = new DataLog(DATALOG_LENGTH);
			InGameMenu = new SelectionMenu(InputManager, new string[] { "Return", "Stats", "Inventory", "Quit" }, 4, 1);
		}

		public void Update()
		{
            if (InMenu)
			{
				while (GetCurrentMenu().Exit) // TODO Verify that this doesn't exit all menus because of a single ESC/Back keystroke
					ExitCurrentMenu();

				if (InMenu)
					GetCurrentMenu().Update();
			}
		}

		public void StackNewMenu(Menu menu)
		{
			MenuStack.Push(menu);
		}

		public Menu? GetCurrentMenu()
		{
			if (MenuStack.Count > 0)
				return MenuStack.Peek();
			else
				return null;
		}

		private void ExitCurrentMenu()
		{
			Debug.Assert(GetCurrentMenu().Exit);
			MenuStack.Pop();
		}

		// TODO Remove these 4 to an external file
		public void StartInGameMenu()
		{
			ContainerMenuManager = null;
			StateMenu = true;
			InGameMenu.Start();
		}

		private void UpdateInGameMenu()
		{
			var input = InGameMenu.Update();

			if (InGameMenu.Exit)
			{
				StateMenu = false;

				return;
			}
			else
			{
				switch (input)
				{
					case "Return":
						StateMenu = false;
						break;
					case "Stats":
						throw new NotImplementedException();
						break;
					case "Inventory":
						StartContainerManager();
						break;
					case "Quit":
						GameManager.Exit();
						break;
				}
			}
		}

		private void UpdateContainerManager()
		{
			ContainerMenuManager.Update();

			if (ContainerMenuManager.Exit)
				ContainerMenuManager = null;
		}

		private void StartContainerManager()
		{
			ContainerMenuManager = new ContainerMenuManager(InputManager, PlayerInventory);
		}
	}
}
