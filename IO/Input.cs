namespace IronEngine.IO
{
	using static ICommandAble;

	/// <summary>
	/// Interface for receiving input from an <see cref="Actor"/>.
	/// </summary>
	public interface IInput
	{
		/// <summary>
		/// Prompts the <see cref="Actor"/> to input a string.
		/// </summary>
		/// <param name="prompt">Prompt to show the user.</param>
		/// <returns>The user's returned string.</returns>
		public string GetString(string prompt);

		/// <summary>
		/// Prompts the <see cref="Actor"/> to select an object from <paramref name="commandables"/>.
		/// </summary>
		/// <param name="commandables"><see cref="IEnumerable"/> of <see cref="ICommandAble"/> to select from.</param>
		/// <returns>The selected <see cref="ICommandAble"/>.</returns>
		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandables);

		/// <summary>
		/// Prompts the <see cref="Actor"/> to select a command from <paramref name="commands"/>.
		/// </summary>
		/// <param name="commands"><see cref="IEnumerable"/> of <see cref="Command"/> to select from.</param>
		/// <returns>The selected <see cref="Command"/>.</returns>
		public Command PickCommand(IEnumerable<Command> commands);

		/// <param name="hasKey">Object to get help for.</param>
		/// <returns>A string containing the full description of <paramref name="hasKey"/>.</returns>
		public string GetHelp(IHasKey hasKey);

		/// <summary>
		/// Default implementation of <see cref="IInput"/> using the <see cref="Console"/>.
		/// </summary>
		public static readonly IInput ConsoleInput = new ConsoleInput();
	}

	/// <summary>
	/// Default implementation of <see cref="IInput"/> using the <see cref="Console"/>.
	/// </summary>
	public class ConsoleInput : IInput
	{
		private const string HELP_STR = "help";

		private Dictionary<string, Command> _commandsCache = new(5);
		private ICommandAble _selected;

		/// <summary>
		/// Whether all available commands are automatically printed when calling <see cref="PickCommand"/> or <see cref="PickCommandAble"/>.
		/// </summary>
		public bool AutoPrintCommands { get; set; } = true;

		/// <summary>
		/// Prompt to print when calling <see cref="PickCommandAble"/>.
		/// </summary>
		public string SelectCommandAblePrompt = "Select Commandable:";

		/// <summary>
		/// Prompt to print when calling <see cref="PickCommand"/>.
		/// </summary>
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

		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandables)
		{
			Store(_commandsCache, commandables.Select(Select));
			Console.WriteLine(SelectCommandAblePrompt);
			if (AutoPrintCommands)
				PrintAll(_commandsCache);
			SelectFromDictionary(_commandsCache).Invoke();
			return _selected;
		}

		public Command PickCommand(IEnumerable<Command> commands)
		{
			Store(_commandsCache, commands.Append(Command.Return));
			Console.WriteLine(SelectCommandPrompt);
			if (AutoPrintCommands)
				PrintAll(_commandsCache);
			return SelectFromDictionary(_commandsCache);
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
