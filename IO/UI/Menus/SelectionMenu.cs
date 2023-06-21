using IO.Render;
using System.Diagnostics;
using static System.Windows.Forms.Design.AxImporter;

namespace IO.UI.Menus
{
    class SelectionMenu : Menu
    {
        private int _cursorJ = 0;
        private int _cursorI = 0;

		public string Title
		{ get; private set; }
        public string?[,] Strings
        { get; private set; }
		public Dictionary<string, Action?>? Actions
		{ get; private set; }
		public int DimJ
        { get => Strings.GetLength(0); }
        public int DimI
        { get => Strings.GetLength(1); }
        public int LongestString
        { get; private set; }
        public int ColumnLength
        { get => LongestString + 1; }
        public int CursorJ
        {
            get => _cursorJ;
            private set => _cursorJ = Utility.Modulo(value, DimJ);
        }
        public int CursorI
        {
            get => _cursorI;
            private set => _cursorI = Utility.Modulo(value, DimI);
		}
		public bool CursorJValidPosition
		{ get => 0 <= CursorJ && CursorJ < DimJ; }
		public bool CursorIValidPosition
		{ get => 0 <= CursorI && CursorI < DimI; }
		public bool CursorValidPosition
        { get => CursorIValidPosition && CursorJValidPosition; }
		public override int LengthJ
		{ get => DimJ; }
		public override int LengthI
		{ get => DimI * ColumnLength; }
		public bool HasTitle
		{ get => Title != string.Empty; }
		public override bool Exit
        { get; set; }

        public SelectionMenu(PlayerInputManager inputManager, Dictionary<string, Action?> actions, int dimJ, int dimI, string? title = null) : base(inputManager)
        {
			Title = title ?? string.Empty;
            if (dimJ * dimI >= actions.Count)
            {
				Actions = actions;
				Strings = GetStringMatrix(actions.Keys.ToArray(), dimJ, dimI);
				UpdateLongestString();
				Start();
            }
            else
            {
                throw new ArgumentException($"2D array of size {dimJ}*{dimI} cannot contain all strings in options.");
            }
        }

		public SelectionMenu(PlayerInputManager inputManager, Dictionary<string, Action?> actions, string?[,] strings, string? title = null) : base(inputManager)
		{
			Title = title ?? string.Empty;
			Actions = actions;
			Strings = strings;
			UpdateLongestString();
			Start();
		}

		public override void Start()
		{
            ResetCursor();
            Continue();
		}

        public override string? Update()
        {
			if (InputManager.IsInputDown(PlayerInputManager.PlayerInputs.Start) || InputManager.IsInputDown(PlayerInputManager.PlayerInputs.Back))
			{
				Exit = true;
                return null;
			}
            MoveCursor(InputManager.GetMenuVector());

            if (InputManager.IsInputDown(PlayerInputManager.PlayerInputs.Confirm))
			{
				var option = GetOptionAtCursor();

                if (option != null && Actions != null)
				{
					try
					{
						Actions[option](); // Call "Action" delegate
					}
					catch (KeyNotFoundException)
					{
						Debug.WriteLine($"Couldn't find Action delegate for string {option}");
					}
					catch (NullReferenceException)
					{
						Debug.WriteLine($"Couldn't find Action delegate for string {option}");
					}
				}

				return option;
			}
            else
                return null;
        }

        public string? GetOptionAtCursor()
        {
            return Strings[CursorJ, CursorI];
		}

		public void ResetCursor()
		{
			CursorJ = 0;
			CursorI = 0;
		}

        public void Continue()
        {
            Exit = false;
		}

        public void SetOptions(string?[,] options)
        {
            Strings = options;
		}

		public override Renderer GetRenderer()
		{
			return new SelectionMenuRenderer(this);
		}

		private void UpdateLongestString()
        {
			var longestString = GetLongestString(Strings);
			LongestString = longestString != null? longestString.Length : 0;
		}

		private void MoveCursor((int, int) vector)
		{
			do
			{
				CursorJ += vector.Item1;
				CursorI += vector.Item2;
			}
			while (GetOptionAtCursor() == null);
		}

		private static string? GetLongestString(string?[,] strings)
		{
			var longestString = "";

			foreach (var s in strings)
			{
				if (s != null && s.Length > longestString.Length)
					longestString = s;
			}

			return longestString;
		}

		private static string? GetLongestString(IEnumerable<string?> strings)
		{
			return strings.MaxBy(s => s != null? s.Length : 0);
		}

		private static string?[,] GetStringMatrix(string[] options, int dimJ, int dimI)
		{
			var ret = new string?[dimJ, dimI];
			int str = 0;

			for (int j = 0; j < dimJ; j++)
			{
				for (int i = 0; i < dimI; i++)
				{
					ret[j, i] = (str < options.Length) ? options[str] : null;
					str++;
				}
			}

            return ret;
		}
	}
}