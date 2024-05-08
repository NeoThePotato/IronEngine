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
		private Dictionary<string, ICommandAble> _commandablesCache = new(5);

		public bool AutoPrintCommands { get; set; } = true;

		public string GetString(string prompt)
		{
			Console.WriteLine(prompt);
			string? ret;
			do
				ret = Console.ReadLine();
			while (ret == null);
			return ret;
		}

		public ICommandAble.Command PickCommand(IEnumerable<ICommandAble.Command> commands)
		{
			StoreHasKey(_commandsCache, commands.Append(ICommandAble.Command.Return));
			if (AutoPrintCommands)
				PrintHasKey(_commandsCache, c => c.description);
			return SelectFromDictionary(_commandsCache);
		}

		public ICommandAble PickCommandAble(IEnumerable<ICommandAble> commandables)
		{
			StoreGeneric(_commandablesCache, commandables);
			if (AutoPrintCommands)
				PrintGeneric(_commandablesCache);
			return SelectFromDictionary(_commandablesCache);
		}

		#region UTILITY
		private static void StoreHasKey<T>(Dictionary<string, T> dictionary, IEnumerable<T> keyAbles) where T : ICommandAble.IHasKey
		{
			dictionary.Clear();
			int index = 1;
			Console.WriteLine("Select command:");
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

		private static void StoreGeneric<T>(Dictionary<string, T> dictionary, IEnumerable<T> objects)
		{
			dictionary.Clear();
			int index = 1;
			Console.WriteLine("Select command:");
			foreach (var obj in objects)
			{
				string key;
				if (obj is ICommandAble.IHasKey keyable && keyable.HasKey)
					key = keyable.Key!.Simplify();
				else
				{
					key = index.ToString();
					index++;
				}
				dictionary.Add(key, obj);
			}
		}

		private static void PrintHasKey<T>(Dictionary<string, T> dictionary, Func<T, string> valuePrint) where T : ICommandAble.IHasKey
		{
			foreach (var kvp in dictionary)
				Console.WriteLine($"{kvp.Key}: {valuePrint(kvp.Value)}");
		}

		private static void PrintGeneric<T>(Dictionary<string, T> dictionary)
		{
			foreach (var kvp in dictionary)
				Console.WriteLine($"{kvp.Key}: {kvp.Value}");
		}

		private static T SelectFromDictionary<T>(Dictionary<string, T> dictionary)
		{
			T selected;
			while (!dictionary.TryGetValue(Console.ReadLine().Simplify(), out selected))
				Console.WriteLine("Invalid input.");
			return selected;
		}
		#endregion
	}
}
