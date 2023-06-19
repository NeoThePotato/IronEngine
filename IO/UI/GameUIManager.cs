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
		}

		public void Update()
		{
            while (InMenu && GetCurrentMenu()!.Exit)
				ExitCurrentMenu();

			if (InMenu)
				GetCurrentMenu()!.Update();
		}

		public void StackNewMenu(Menu menu)
		{
			MenuStack.Push(menu);
		}

		public Menu? GetCurrentMenu()
		{
			if (InMenu)
				return MenuStack.Peek();
			else
				return null;
		}

		public void ForceExitCurrentMenu()
		{
			if (InMenu)
				MenuStack.Pop();
		}

		private void ExitCurrentMenu()
		{
			Debug.Assert(GetCurrentMenu()!.Exit);
			MenuStack.Pop();
		}
	}
}
