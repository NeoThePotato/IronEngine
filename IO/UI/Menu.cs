namespace IO.UI
{
    abstract class Menu
    {
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

        public Menu(PlayerInputManager inputManager)
        {
            InputManager = inputManager;
        }

        public abstract void Start();

        public abstract string? Update();
    }
}