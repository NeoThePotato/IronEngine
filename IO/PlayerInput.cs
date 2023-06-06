using System.Runtime.Versioning;
using System.Windows.Input;
using static Game.World.Direction;

namespace IO
{
	static class PlayerInput
	{
		static readonly Dictionary<Key, PlayerInputs> INPUT_BINDING = new Dictionary<Key, PlayerInputs>()
		{
			{Key.NumPad6,   PlayerInputs.Right},
			{Key.NumPad9,   PlayerInputs.UpRight},
			{Key.NumPad8,   PlayerInputs.Up},
			{Key.NumPad7,   PlayerInputs.UpLeft},
			{Key.NumPad4,   PlayerInputs.Left},
			{Key.NumPad1,   PlayerInputs.DownLeft},
			{Key.NumPad2,   PlayerInputs.Down},
			{Key.NumPad3,   PlayerInputs.DownRight},
			{Key.Right,     PlayerInputs.Right},
			{Key.Up,        PlayerInputs.Up},
			{Key.Left,      PlayerInputs.Left},
			{Key.Down,      PlayerInputs.Down},
			{Key.Enter,     PlayerInputs.Confirm},
			{Key.Back,      PlayerInputs.Back},
		};

		static readonly Dictionary<PlayerInputs, Directions> DIRECTIONS = new Dictionary<PlayerInputs, Directions>()
		{
			{ PlayerInputs.Right,		Directions.E},
			{ PlayerInputs.UpRight,		Directions.NE},
			{ PlayerInputs.Up,			Directions.N },
			{ PlayerInputs.UpLeft,		Directions.NW },
			{ PlayerInputs.Left,		Directions.W },
			{ PlayerInputs.DownLeft,	Directions.SW },
			{ PlayerInputs.Down,		Directions.S },
			{ PlayerInputs.DownRight,	Directions.SE },
		};

		static private Dictionary<PlayerInputs, bool> KeyboardState = new Dictionary<PlayerInputs, bool>(INPUT_BINDING.Count);

		[SupportedOSPlatform("windows")]
		public static Dictionary<PlayerInputs, bool> PollKeyBoard()
		{
			foreach (var kvp in INPUT_BINDING)
				KeyboardState[kvp.Value] = Keyboard.IsKeyDown(kvp.Key);

			return KeyboardState;
		}

		public static Directions InputToDirection(Dictionary<PlayerInputs, bool> keyboardState)
		{
			(int, int) movementVector = (0, 0);

			foreach(var kvp in DIRECTIONS)
				movementVector = AddVectors(movementVector, keyboardState[kvp.Key] ? TranslateDirection(kvp.Value) : (0, 0));
			movementVector = NormalizeVector(movementVector);

			return TranslateDirection(movementVector);
		}

		private static (int, int) AddVectors((int, int) vector1, (int, int) vector2)
		{
			return (vector1.Item1 + vector2.Item1, vector1.Item2 + vector2.Item2);
		}

		private static (int, int) NormalizeVector((int, int) vector)
		{
			return (Normalize(vector.Item1), Normalize(vector.Item2));
		}

		private static int Normalize(int num)
		{
			return Utility.ClampRange(num, -1, 1);
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
