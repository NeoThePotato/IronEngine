using static IronEngine.ICommandAble;

namespace IronEngine.IO
{
	public interface IInput
	{
		public string GetString(string prompt);

		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandAbles);

		public Command PickCommand(IEnumerable<Command> commands);

		public string GetHelp(IHasKey hasKey);

		public static readonly IInput ConsoleInput = new ConsoleInput();
	}

	public class ConsoleInput : IInput
	{
		private const string HELP_STR = "help";

		private Dictionary<string, Command> _commandsCache = new(5);
		private ICommandAble _selected;

		public bool AutoPrintCommands { get; set; } = true;
		public string SelectCommandAblePrompt = "Select Commandable:";
		public string SelectCommandPrompt = "Select Command:";

		public string GetString(string prompt)
		{
			Console.WriteLine(prompt);
			string? ret;
			do
				ret = Console.ReadLine();
			while (ret == null);
			return ret;
		}

		public Command PickCommand(IEnumerable<Command> commands)
		{
			Store(_commandsCache, commands.Append(Command.Return));
			Console.WriteLine(SelectCommandPrompt);
			if (AutoPrintCommands)
				PrintAll(_commandsCache);
			return SelectFromDictionary(_commandsCache);
		}

		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandables)
		{
			Store(_commandsCache, commandables.Select(Select));
			Console.WriteLine(SelectCommandAblePrompt);
			if (AutoPrintCommands)
				PrintAll(_commandsCache);
			SelectFromDictionary(_commandsCache).Invoke();
			return _selected;
		}

		public string GetHelp(IHasKey hasKey)
		{
			return PrintFull(hasKey);
		}

		#region UTILITY
		private static string PrintFull(IHasKey hasKey) => $"{hasKey.Key}: {hasKey.Description}";

		private static void Store<T>(Dictionary<string, T> dictionary, IEnumerable<T> keyAbles) where T : IHasKey
		{
			dictionary.Clear();
			int index = 1;
			foreach (var keyAble in keyAbles)
			{
				string key;
				if (keyAble.HasKey)
					key = keyAble.Key!.Simplify();
				else
				{
					key = index.ToString();
					index++;
				}
				dictionary.Add(key, keyAble);
			}
		}

		private static void PrintAll<T>(Dictionary<string, T> dictionary) where T : IHasKey
		{
			foreach (var kvp in dictionary)
				Console.WriteLine(PrintFull(kvp.Value));
		}

		private T SelectFromDictionary<T>(Dictionary<string, T> dictionary) where T : IHasKey
		{
			T selected;
			do
			{
				string key = Console.ReadLine().Simplify();
				if (dictionary.TryGetValue(key, out selected))
					break;
				else if (key.StartsWith(HELP_STR))
				{
					key = key.Remove(0, HELP_STR.Length).RemoveSpaces();
					if (dictionary.TryGetValue(key, out selected))
						Console.WriteLine(GetHelp(selected));
					else
						PrintAll(_commandsCache);
				}
				else
					Console.WriteLine("Invalid input. Use \"Help\" to show help.");
			}
			while (true);
			return selected;
		}

		private Command Select(ICommandAble commandAble)
		{
			if (commandAble is IHasKey hasKey && hasKey.HasKey)
				return new Command(() => _selected = commandAble, hasKey.Description, hasKey!.Key, false);
			else
				return new Command(() => _selected = commandAble, commandAble.ToString(), commandAble.ToString(), false);
		}
		#endregion
	}
}
