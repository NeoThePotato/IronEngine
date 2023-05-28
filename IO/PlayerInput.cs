namespace IO
{
	static class PlayerInput
	{
		static readonly Dictionary<ConsoleKey, PlayerInputs> INPUT_BINDING = new Dictionary<ConsoleKey, PlayerInputs>(){
			{ConsoleKey.NumPad6,	PlayerInputs.Right},
			{ConsoleKey.NumPad9,	PlayerInputs.UpRight},
			{ConsoleKey.NumPad8,	PlayerInputs.Up},
			{ConsoleKey.NumPad7,	PlayerInputs.UpLeft},
			{ConsoleKey.NumPad4,	PlayerInputs.Left},
			{ConsoleKey.NumPad1,	PlayerInputs.DownLeft},
			{ConsoleKey.NumPad2,	PlayerInputs.Down},
			{ConsoleKey.NumPad3,	PlayerInputs.DownRight},
			{ConsoleKey.RightArrow,	PlayerInputs.Right},
			{ConsoleKey.UpArrow,	PlayerInputs.Up},
			{ConsoleKey.LeftArrow,	PlayerInputs.Left},
			{ConsoleKey.DownArrow,	PlayerInputs.Down},
			{ConsoleKey.Enter,		PlayerInputs.Confirm},
			{ConsoleKey.Backspace,	PlayerInputs.Back},
		};

		public static PlayerInputs WaitForKey()
		{
			return KeyToInput(Console.ReadKey().Key);
		}

		private static PlayerInputs KeyToInput(ConsoleKey consoleKey)
		{
			PlayerInputs input;

			return (INPUT_BINDING.TryGetValue(consoleKey, out input)) ? input : PlayerInputs.Any;
		}

		public enum PlayerInputs
		{
			Any,
			Right,
			UpRight,
			Up,
			UpLeft,
			Left,
			DownLeft,
			Down,
			DownRight,
			Confirm,
			Back,
		}
	}
}
