namespace IronEngine.IO
{
	public interface IInput
	{
		public string GetString(string prompt);

		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandAbles);

		public ICommandAble.Command PickCommand(IEnumerable<ICommandAble.Command> commands);

		public static readonly IInput ConsoleInput = new ConsoleInput();
	}

	internal class ConsoleInput : IInput
	{
		private Dictionary<string, ICommandAble.Command> _commandsCache = new(5);
		private List<ICommandAble> _commanablesCache = new(5);

		public string GetString(string prompt)
		{
			Console.WriteLine(prompt);
			string? ret;
			do
				ret = Console.ReadLine();
			while (ret == null);
			return ret;
		}

		#region COMMANDS
		public ICommandAble.Command PickCommand(IEnumerable<ICommandAble.Command> actions)
		{
			StoreAndPrintAvailableAction(actions);
			return GetCommandToPerform();
		}

		private void StoreAndPrintAvailableAction(IEnumerable<ICommandAble.Command> commands)
		{
			_commandsCache.Clear();
			int index = 1;
			Console.WriteLine("Select command:");
			foreach (var command in commands)
			{
				string key;
				if (command.HasKey)
					key = command.Key!.ToLower();
				else
				{
					key = index.ToString();
					index++;
				}
				_commandsCache.Add(key, command);
				Console.WriteLine($"{command.Key}: {command.description}");
			}
		}

		private ICommandAble.Command GetCommandToPerform()
		{
			ICommandAble.Command selectedCommand;
			while (!_commandsCache.TryGetValue(Console.ReadLine().ToLower(), out selectedCommand))
				Console.WriteLine("Invalid input.");
			return selectedCommand;
		}
		#endregion

		#region COMMANDABLES
		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandables)
		{
			_commanablesCache.Clear();
			int index = 1;
			Console.WriteLine("Select commandable:");
			foreach (var commandable in commandables)
			{
				_commanablesCache.Add(commandable);
				Console.WriteLine($"{index}: {commandable}");
				index++;
			}
			while (!int.TryParse(Console.ReadLine(), out index) || index > _commanablesCache.Count)
				Console.WriteLine("Invalid input.");
			return _commanablesCache[index - 1];
		}
		#endregion
	}
}
