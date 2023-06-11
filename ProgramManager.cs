using Game;
using IO.Render;
using System.Diagnostics;

class ProgramManager
{
	public const int GLOBAL_TICK_RATE = 60;
	public const double FRAMETIME_MILISEC = 1000.0 / GLOBAL_TICK_RATE;
	public const long FRAMETIME_TICKS = (long)(TimeSpan.TicksPerMillisecond * FRAMETIME_MILISEC);
	public readonly TimeSpan FRAMETIME = new TimeSpan(FRAMETIME_TICKS);
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
			DateTime startTime = DateTime.Now;

			// Logic
			_gameManager.Update();
			DateTime logicTime = DateTime.Now;

			// Render
			_consoleRenderer.RenderFrame();
			DateTime renderTime = DateTime.Now;

			// Frame Timing
			TimeSpan totalUpdateTime = renderTime - startTime;
			TimeSpan timeDelta = FRAMETIME > totalUpdateTime ? FRAMETIME - totalUpdateTime : new TimeSpan(0);
			Debug.WriteLine($"Logic Time: {TimeSpanToMilliseconds(logicTime-startTime)}ms\nRender Time: {TimeSpanToMilliseconds(renderTime - logicTime)}ms\nTotal update time: {TimeSpanToMilliseconds(totalUpdateTime)}ms");
			Thread.Sleep(timeDelta);
		}
	}

	private static float TimeSpanToMilliseconds(TimeSpan ts)
	{
		return (float)ts.Ticks/TimeSpan.TicksPerMillisecond;
	}
}
