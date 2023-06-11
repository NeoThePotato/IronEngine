using Game;
using IO.Render;

class ProgramManager
{
	public const int GLOBAL_TICK_RATE = 60;
	public const int FRAMETIME_MILISEC = 1000 / GLOBAL_TICK_RATE;
	private GameManager _gameManager;
	private ConsoleRenderer _consoleRenderer;

	public ProgramManager()
	{
		_gameManager = new GameManager();
		_gameManager.Start();
		_consoleRenderer = new ConsoleRenderer(new GameManagerRenderer(_gameManager));
	}

	public void Run()
	{
		while (true)
		{
			// Logic
			_gameManager.Update();

			// Visuals
			_consoleRenderer.RenderFrame();

			// TODO Make this use time delta instead of a fixed time.
			Thread.Sleep(FRAMETIME_MILISEC);
		}
	}
}
