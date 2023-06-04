using Game;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private GameManager Manager;
		new private int SizeJ
		{ get => GetSizeJ(BufferCache); }
		new private int SizeI
		{ get => GetSizeI(BufferCache); }

		public GameManagerRenderer(GameManager gameManager)
		{
			Manager = gameManager;
		}
	}
}
