using Game;
using IO.Render;
using System.Diagnostics;

class ProgramManager
{
	#region TIMING_FIELDS
	public const int GLOBAL_UPDATE_RATE = 60;
	public const double FRAMETIME_MILISEC = 1000.0 / GLOBAL_UPDATE_RATE;
	public const long FRAMETIME_TICKS = (long)(TimeSpan.TicksPerMillisecond * FRAMETIME_MILISEC);
	public readonly TimeSpan FRAMETIME = new TimeSpan(FRAMETIME_TICKS);
	private DateTime _startTime;
	private DateTime _logicFinishTime;
	private DateTime _renderFinishTime;
	#endregion
	private GameManager _gameManager;
	private ConsoleRenderer _consoleRenderer;

	#region TIMING_PROPERTIES
	private TimeSpan LogicUpdateTime
	{ get => _logicFinishTime - _startTime; }
	private TimeSpan RenderUpdateTime
	{ get => _renderFinishTime - _logicFinishTime; }
	private TimeSpan TotalUpdateTime
	{ get => _renderFinishTime - _startTime; }
	private TimeSpan TimeUntilNextFrame
	{ get => FRAMETIME > TotalUpdateTime ? FRAMETIME - TotalUpdateTime : new TimeSpan(0); }
	#endregion

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
			UpdateLogic();
			UpdateRender();
			PrintFrameTimes();
			SleepUntilNextFrame();
		}
	}

	private void UpdateLogic()
	{
		_startTime = DateTime.Now;
		_gameManager.Update();
		_logicFinishTime = DateTime.Now;
	}

	private void UpdateRender()
	{
		_consoleRenderer.RenderFrame();
		_renderFinishTime = DateTime.Now;
	}

	private void SleepUntilNextFrame()
	{
		Thread.Sleep(TimeUntilNextFrame);
	}

	[Conditional("DEBUG")]
	private void PrintFrameTimes()
	{
		Debug.WriteLine($"Logic Time: {TimeSpanToMilliseconds(LogicUpdateTime)}ms\nRender Time: {TimeSpanToMilliseconds(RenderUpdateTime)}ms\nTotal update time: {TimeSpanToMilliseconds(TotalUpdateTime)}ms");
	}

	private static float TimeSpanToMilliseconds(TimeSpan ts)
	{
		return (float)ts.Ticks/TimeSpan.TicksPerMillisecond;
	}
}
