using System.Windows.Input;

namespace IO
{
	class PlayerInputManager
	{
		private static readonly Dictionary<Key, PlayerInputs> INPUT_BINDING = new Dictionary<Key, PlayerInputs>()
		{
			{Key.Right,		PlayerInputs.Right},
			{Key.Up,		PlayerInputs.Up},
			{Key.Left,		PlayerInputs.Left},
			{Key.Down,		PlayerInputs.Down},
			{Key.Enter,		PlayerInputs.Confirm},
			{Key.Space,		PlayerInputs.Confirm},
			{Key.X,			PlayerInputs.Confirm},
			{Key.Back,		PlayerInputs.Back},
			{Key.Z,			PlayerInputs.Back},
			{Key.C,			PlayerInputs.Start},
			{Key.Escape,	PlayerInputs.Start},
		};
		private static readonly Dictionary<PlayerInputs, (int, int)> INPUT_TO_VECTOR = new Dictionary<PlayerInputs, (int, int)>()
		{
			{ PlayerInputs.Right,       (0, 1)},
			{ PlayerInputs.Up,          (-1, 0)},
			{ PlayerInputs.Left,        (0, -1)},
			{ PlayerInputs.Down,        (1, 0)},
		};
		private Dictionary<PlayerInputs, bool> _previousKeyboardState;
		private Dictionary<PlayerInputs, bool> _currentKeyboardState;

		public PlayerInputManager()
		{
			var emptyState = GetEmptyKeyboardState();
			_previousKeyboardState = new Dictionary<PlayerInputs, bool>(emptyState);
			_currentKeyboardState = new Dictionary<PlayerInputs, bool>(emptyState);
		}

		public void PollKeyBoard()
		{
			UpdatePreviousState();
			UpdateCurrentState();
		}

		public (int, int) GetMovementVector(int mag)
		{
			return NormalizeMovementVector(InputToVector(), mag);
		}

		public (int, int) GetMenuVector()
		{
			return NormalizeMenuVector(InputToVector());
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
			ResetKeyboardState(_currentKeyboardState);

			foreach (var kvp in INPUT_BINDING)
				_currentKeyboardState[kvp.Value] = _currentKeyboardState[kvp.Value] | Keyboard.IsKeyDown(kvp.Key);
		}

		private void UpdatePreviousState()
		{
			Utility.Swap(ref _previousKeyboardState, ref _currentKeyboardState);
		}

		private Dictionary<PlayerInputs, bool> GetEmptyKeyboardState()
		{
			var emptyState = new Dictionary<PlayerInputs, bool>();

			foreach (var kvp in INPUT_BINDING)
				emptyState[kvp.Value] = false;

			return emptyState;
		}

		private static void ResetKeyboardState(Dictionary<PlayerInputs, bool> keyboardState)
		{
			foreach (var kvp in keyboardState)
				keyboardState[kvp.Key] = false;
		}

		private (int, int) InputToVector()
		{
			(int, int) totalMovementVector = (0, 0);

			foreach (var kvp in INPUT_TO_VECTOR)
				totalMovementVector = AddVectors(totalMovementVector, IsInputDown(kvp.Key) ? kvp.Value : (0, 0));

			return totalMovementVector;
		}

		private static (int, int) AddVectors((int, int) vector1, (int, int) vector2)
		{
			return (vector1.Item1 + vector2.Item1, vector1.Item2 + vector2.Item2);
		}

		private static (int, int) NormalizeMovementVector((int, int) vector, int maxMagnitude)
		{
			return Utility.NormalizeVector(vector, maxMagnitude);
		}

		private static (int, int) NormalizeMenuVector((int, int) vector)
		{
			return (Utility.ClampRange(vector.Item1, -1, 1), Utility.ClampRange(vector.Item2, -1, 1));
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
			Start
		}
	}
}
