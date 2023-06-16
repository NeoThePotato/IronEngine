using System.Runtime.Versioning;
using System.Windows.Input;
using static Game.World.Direction;

namespace IO
{
	static class PlayerInput
	{
		static readonly Dictionary<Key, PlayerInputs> INPUT_BINDING = new Dictionary<Key, PlayerInputs>()
		{
			{Key.Right,     PlayerInputs.Right},
			{Key.PageUp,	PlayerInputs.UpRight},
			{Key.Up,        PlayerInputs.Up},
			{Key.Home,		PlayerInputs.UpLeft},
			{Key.Left,      PlayerInputs.Left},
			{Key.End,		PlayerInputs.DownLeft},
			{Key.Down,      PlayerInputs.Down},
			{Key.PageDown,	PlayerInputs.DownRight},
			{Key.Enter,		PlayerInputs.Confirm},
			{Key.Back,		PlayerInputs.Back},
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
		static private Dictionary<PlayerInputs, bool> _previousKeyboardState = new Dictionary<PlayerInputs, bool>(INPUT_BINDING.Count);
		static private Dictionary<PlayerInputs, bool> _currentKeyboardState = new Dictionary<PlayerInputs, bool>(INPUT_BINDING.Count);

		[SupportedOSPlatform("windows")]
		public static void PollKeyBoard()
		{
			_previousKeyboardState = _currentKeyboardState;

			foreach (var kvp in INPUT_BINDING)
				_currentKeyboardState[kvp.Value] = Keyboard.IsKeyDown(kvp.Key);
		}

		public static Directions GetMovementDirection()
		{
			return TranslateDirection(GetMovementVector());
		}

		public static (int, int) GetMovementVector()
		{
			(int, int) movementVector = (0, 0);

			foreach (var kvp in DIRECTIONS)
				movementVector = AddVectors(movementVector, IsInputPressed(kvp.Key)? TranslateDirection(kvp.Value) : (0, 0));
			movementVector = NormalizeVector(movementVector);

			return movementVector;
		}

		public static Directions GetMenuDirection()
		{
			return TranslateDirection(GetMenuVector());
		}

		public static (int, int) GetMenuVector()
		{
			(int, int) movementVector = (0, 0);

			foreach (var kvp in DIRECTIONS)
				movementVector = AddVectors(movementVector, IsInputDown(kvp.Key) ? TranslateDirection(kvp.Value) : (0, 0));
			movementVector = NormalizeVector(movementVector);

			return movementVector;
		}

		public static bool IsInputPressed(PlayerInputs playerInput)
		{
			return _currentKeyboardState[playerInput];
		}

		public static bool IsInputDown(PlayerInputs playerInput)
		{
			return IsInputPressed(playerInput) & !_previousKeyboardState[playerInput];
		}

		public static bool IsInputUp(PlayerInputs playerInput)
		{
			return !IsInputPressed(playerInput) & _previousKeyboardState[playerInput];
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
