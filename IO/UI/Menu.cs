using IO.Render;

namespace IO.UI
{
    abstract class Menu
	{
		public GameUIManager ParentUIManager
		{ get; private set; }
		public PlayerInputManager InputManager
        { get; private set; }
        public abstract int LengthJ
        { get; }
        public abstract int LengthI
        { get; }
        public int Size
        { get => LengthJ * LengthI; }
        public abstract bool Exit
        { get; set; }

        public Menu(PlayerInputManager inputManager, GameUIManager parentUIManager)
		{
			InputManager = inputManager;
			ParentUIManager = parentUIManager;
		}

		public abstract void Start();

        public abstract string? Update();

        public abstract Renderer GetRenderer();
    }
}