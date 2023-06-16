namespace IO.UI
{
    class MenuManager
    {
        private int _cursorJ = 0;
        private int _cursorI = 0;

        public PlayerInputManager InputManager
        { get; private set; }
        public string?[,] Options
        { get; private set; }
        public int DimJ
        { get => Options.GetLength(0); }
        public int DimI
        { get => Options.GetLength(1); }
        public int DimSize
        { get => DimJ * DimI; }
        public int LongestString
        { get; private set; }
        public int CursorJ
        {
            get => _cursorJ;
            private set => _cursorJ = value % DimJ;
        }
        public int CursorI
        {
            get => _cursorI;
            private set => _cursorI = value % DimI;
		}
		public bool CursorJValidPosition
		{ get => 0 <= CursorJ && CursorJ < DimJ; }
		public bool CursorIValidPosition
		{ get => 0 <= CursorI && CursorI < DimI; }
		public bool CursorValidPosition
        { get => CursorIValidPosition && CursorJValidPosition; }

        public MenuManager(PlayerInputManager inputManager, string[] options, int dimJ, int dimI)
        {
            InputManager = inputManager;
            if (dimJ * dimI >= options.Length)
            {
                Options = new string[dimJ, dimI];
                SetOptions(options);
            }
            else
            {
                throw new ArgumentException($"2D array of size {dimJ}*{dimI} cannot contain all strings in options.");
            }
        }

        public string? Update()
        {
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

        private void MoveCursor((int, int) vector)
        {
            do
            {
                CursorJ += vector.Item1;
                CursorI += vector.Item2;
            }
            while (GetOptionAtCursor != null);
        }

        private void SetOptions(string[] options)
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
    }
}