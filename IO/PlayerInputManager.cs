using System.Runtime.Versioning;
using System.Windows.Input;
using static Game.World.Direction;

namespace IO
{
	class PlayerInputManager
	{
		private static readonly Dictionary<Key, PlayerInputs> INPUT_BINDING = new Dictionary<Key, PlayerInputs>()
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
		private static readonly Dictionary<PlayerInputs, Directions> DIRECTIONS = new Dictionary<PlayerInputs, Directions>()
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
		private Dictionary<PlayerInputs, bool> _previousKeyboardState;
		private Dictionary<PlayerInputs, bool> _currentKeyboardState;

		public PlayerInputManager()
		{
			var emptyState = GetEmptyKeyboardState();
			_previousKeyboardState = new Dictionary<PlayerInputs, bool>(emptyState);
			_currentKeyboardState = new Dictionary<PlayerInputs, bool>(emptyState);
		}

		[SupportedOSPlatform("windows")]
		public void PollKeyBoard()
		{
			UpdatePreviousState();
			UpdateCurrentState();
		}

		public Directions GetMovementDirection()
		{
			return TranslateDirection(GetMovementVector());
		}

		public (int, int) GetMovementVector()
		{
			(int, int) movementVector = (0, 0);

			foreach (var kvp in DIRECTIONS)
				movementVector = AddVectors(movementVector, IsInputPressed(kvp.Key)? TranslateDirection(kvp.Value) : (0, 0));
			movementVector = NormalizeVector(movementVector);

			return movementVector;
		}

		public Directions GetMenuDirection()
		{
			return TranslateDirection(GetMenuVector());
		}

		public (int, int) GetMenuVector()
		{
			(int, int) movementVector = (0, 0);

			foreach (var kvp in DIRECTIONS)
				movementVector = AddVectors(movementVector, IsInputDown(kvp.Key) ? TranslateDirection(kvp.Value) : (0, 0));
			movementVector = NormalizeVector(movementVector);

			return movementVector;
		}

		public bool IsInputPressed(PlayerInputs playerInput)
		{
			return _currentKeyboardState[playerInput];
		}

		public bool IsInputDown(PlayerInputs playerInput)
		{
			return IsInputPressed(playerInput) & !_previousKeyboardState[playerInput];
		}

		public bool IsInputUp(PlayerInputs playerInput)
		{
			return !IsInputPressed(playerInput) & _previousKeyboardState[playerInput];
		}

		private void UpdateCurrentState()
		{
			foreach (var kvp in INPUT_BINDING)
				_currentKeyboardState[kvp.Value] = Keyboard.IsKeyDown(kvp.Key);
		}

		private void UpdatePreviousState()
		{
			foreach (var kvp in _currentKeyboardState)
				_previousKeyboardState[kvp.Key] = kvp.Value;
		}

		private Dictionary<PlayerInputs, bool> GetEmptyKeyboardState()
		{
			var emptyState = new Dictionary<PlayerInputs, bool>();

			foreach (var kvp in INPUT_BINDING)
				emptyState[kvp.Value] = false;

			return emptyState;
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
