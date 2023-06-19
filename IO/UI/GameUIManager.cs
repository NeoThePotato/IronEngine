using Game;
using Game.Items;

namespace IO.UI
{
    class GameUIManager
	{
		public const int DATALOG_LENGTH = 5;

		public DataLog DataLog
		{ get; set; }
		public PlayerInputManager InputManager
		{ get => GameManager.InputManager; }
		public MenuManager InGameMenu
		{ get; private set; }
		public ContainerMenuManager? ContainerMenuManager
		{ get; private set; }
		public Container PlayerInventory
		{ get => GameManager.LevelManager.PlayerInventory; }
		public bool StateMenu
		{ get; private set; }
		public bool StateInventoryMenu
		{ get => ContainerMenuManager != null; }
		public bool StartMenuCondition
		{ get => InputManager.IsInputDown(PlayerInputManager.PlayerInputs.Start) && !StateMenu; }
		public GameManager GameManager
		{ get; private set; }

		public GameUIManager(GameManager gameManager)
		{
			DataLog = new DataLog(DATALOG_LENGTH);
			GameManager = gameManager;
			InGameMenu = new MenuManager(InputManager, new string[] { "Return", "Stats", "Inventory", "Quit" }, 4, 1);
		}

		public void Update()
		{
			if (StateInventoryMenu)
				UpdateContainerManager();
			else
				UpdateInGameMenu();
		}

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
