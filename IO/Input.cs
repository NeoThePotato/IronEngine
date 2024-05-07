namespace IronEngine.IO
{
	public interface IInput
	{
		public string GetString(string prompt);

		public IActionable PickActionable(IEnumerable<IActionable> actionables);

		public IActionable.Action PickAction(IEnumerable<IActionable.Action> actions);

		public static readonly IInput ConsoleInput = new ConsoleInput();
	}

	internal class ConsoleInput : IInput
	{
		private List<IActionable.Action> _actionsCache = new(5);
		private List<IActionable> _actionablesCache = new(5);

		public string GetString(string prompt)
		{
			Console.WriteLine(prompt);
			string? ret;
			do
				ret = Console.ReadLine();
			while (ret == null);
			return ret;
		}

		public IActionable.Action PickAction(IEnumerable<IActionable.Action> actions)
		{
			_actionsCache.Clear();
			int index = 1;
			foreach (var action in actions)
			{
				_actionsCache[index-1] = action;
				Console.WriteLine($"{index}: {action.description}");
				index++;
			}
			while (!int.TryParse(Console.ReadLine(), out index) && index > _actionsCache.Count)
				Console.WriteLine("Invalid input.");
			return _actionsCache[index-1];
		}

		public IActionable PickActionable(IEnumerable<IActionable> actionables)
		{
			_actionablesCache.Clear();
			int index = 1;
			foreach (var actionable in actionables)
			{
				_actionablesCache[index - 1] = actionable;
				Console.WriteLine($"{index}: {actionable}");
				index++;
			}
			while (!int.TryParse(Console.ReadLine(), out index) && index > _actionablesCache.Count)
				Console.WriteLine("Invalid input.");
			return _actionablesCache[index - 1];
		}
	}
}
