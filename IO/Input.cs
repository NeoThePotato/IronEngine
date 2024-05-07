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
		private Dictionary<string, IActionable.Action> _actionsCache = new(5);
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

		#region ACTIONS
		public IActionable.Action PickAction(IEnumerable<IActionable.Action> actions)
		{
			StoreAndPrintAvailableAction(actions);
			return GetActionToPerform();
		}

		private void StoreAndPrintAvailableAction(IEnumerable<IActionable.Action> actions)
		{
			_actionsCache.Clear();
			int index = 1;
			Console.WriteLine("Select action:");
			foreach (var action in actions)
			{
				string key;
				if (action.key != null)
					key = action.key.ToLower();
				else
				{
					key = index.ToString();
					index++;
				}
				_actionsCache.Add(key, action);
				Console.WriteLine($"{key}: {action.description}");
			}
		}

		private IActionable.Action GetActionToPerform()
		{
			IActionable.Action selectedAction;
			while (!_actionsCache.TryGetValue(Console.ReadLine().ToLower(), out selectedAction))
				Console.WriteLine("Invalid input.");
			return selectedAction;
		}
		#endregion

		#region ACTIONABLES
		public IActionable PickActionable(IEnumerable<IActionable> actionables)
		{
			_actionablesCache.Clear();
			int index = 1;
			Console.WriteLine("Select actionable:");
			foreach (var actionable in actionables)
			{
				_actionablesCache.Add(actionable);
				Console.WriteLine($"{index}: {actionable}");
				index++;
			}
			while (!int.TryParse(Console.ReadLine(), out index) || index > _actionablesCache.Count)
				Console.WriteLine("Invalid input.");
			return _actionablesCache[index - 1];
		}
		#endregion
	}
}
