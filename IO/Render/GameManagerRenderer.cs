using Game;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private GameManager GameManager
		{ get; set; }
		private LevelManagerRenderer LevelManagerRenderer
		{ get; set; }
		public override int SizeJ
		{ get => LevelManagerRenderer.SizeJ; }
		public override int SizeI
		{ get => LevelManagerRenderer.SizeI; }

		public GameManagerRenderer(GameManager gameManager)
		{
			GameManager = gameManager;
			LevelManagerRenderer = new LevelManagerRenderer(GameManager.LevelManager);
		}

		public override void Render(ref char[,] buffer)
		{
			LevelManagerRenderer.Render(ref buffer);
		}
	}
}
