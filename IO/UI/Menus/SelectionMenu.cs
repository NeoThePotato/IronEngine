namespace IO.UI.Menus
{
    class SelectionMenu : Menu
    {
        private int _cursorJ = 0;
        private int _cursorI = 0;

        public string?[,] Options
        { get; private set; }
		public int DimJ
        { get => Options.GetLength(0); }
        public int DimI
        { get => Options.GetLength(1); }
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
		public override bool Exit
        { get; set; }

        public SelectionMenu(PlayerInputManager inputManager, string[] options, int dimJ, int dimI) : base(inputManager)
        {
            if (dimJ * dimI >= options.Length)
            {
                Options = new string[dimJ, dimI];
                SetOptions(options);
                Start();
            }
            else
            {
                throw new ArgumentException($"2D array of size {dimJ}*{dimI} cannot contain all strings in options.");
            }
        }

		public SelectionMenu(PlayerInputManager inputManager, string?[,] options) : base(inputManager)
		{
			SetOptions(options);
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
                return GetOptionAtCursor();
            else
                return null;
        }

        public string? GetOptionAtCursor()
        {
            return Options[CursorJ, CursorI];
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
            Options = options;
            UpdateLongestString();
		}

		public void SetOptions(string[] options)
		{
			int o = 0;

			for (int j = 0; j < DimJ; j++)
			{
				for (int i = 0; i < DimI; i++)
				{
					if (o < options.Length)
					{
						Options[j, i] = options[o];
						LongestString = Math.Max(LongestString, options[o].Length);
					}
					else
					{
						Options[j, i] = null;
					}
					o++;
				}
			}
		}

        private void UpdateLongestString()
        {
            foreach (var str in Options)
				LongestString = str != null ? Math.Max(LongestString, str.Length) : LongestString;
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
    }
}