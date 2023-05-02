using System.Text;

class ConsoleMenu
{

	private const string NORMAL_COLOR = "\u001b[0m";
	private const string HIGHLIGHTED_COLOR = "\u001b[7m";
	private string[] _options;
	private int _startLine;
	private int _endLine;

	public int OptionsLength
	{
		get => _options.Length;
	}
	public bool UseSingleKeySelection
	{
		get => OptionsLength < 10;
	}

	public ConsoleMenu(params string[] options)
	{
		_options = options;
	}

	public int Menu()
	{
		int selection;
		_startLine = Console.CursorTop;
		PrintMenu();
		_endLine = Console.CursorTop;
		selection = GetValidUserInput();
		PrintMenu(selection);

		return selection;
	}

	public string GetOptionStr(int selection)
	{
		try
		{
			return _options[selection - 1];
		}
		catch (IndexOutOfRangeException)
		{
			return "";
		}
	}

	private void PrintMenu(int highlightedChoice = 0)
	{
		Console.SetCursorPosition(0, _startLine);
		Console.Write(GetFullMenuStr(highlightedChoice));
	}

	private string GetFullMenuStr(int highlightedChoice = 0)
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 1; i <= OptionsLength; i++)
		{
			if (i == highlightedChoice)
				sb.Append(HIGHLIGHTED_COLOR);

			sb.Append($"{i}: {GetOptionStr(i)}\n");

			if (i == highlightedChoice)
				sb.Append(NORMAL_COLOR);
		}

		return sb.ToString();
	}

	private int GetValidUserInput()
	{
		int selection;

		// Block until valid user input
		while (!IsValid(BlockUntilUserInput(), out selection))
			ClearLine(_endLine);
		ClearLine(_endLine);

		return selection;
	}

	private string? BlockUntilUserInput()
	{
		return UseSingleKeySelection ? Console.ReadKey().KeyChar.ToString() : Console.ReadLine();
	}

	private bool IsValid(int selection)
	{
		return 0 < selection && selection <= OptionsLength;
	}

	private bool IsValid(string? str, out int selection)
	{
		return int.TryParse(str, out selection) && IsValid(selection);
	}

	private static void ClearLine(int line)
	{
		Console.SetCursorPosition(0, line);
		Console.WriteLine(new string(' ', Console.WindowWidth - 1));
		Console.SetCursorPosition(0, line);
	}
}