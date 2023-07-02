using Game;

namespace IO.Render
{
	class LevelManagerRenderer : Renderer
	{
		private GameManager GameManager
		{ get; set; }
		public LevelManager LevelManager
		{ get => GameManager.LevelManager; }
		public LevelRenderer LevelRenderer
		{ get; private set; }
		public override int SizeJ
		{ get => LevelRenderer.SizeJ; }
		public override int SizeI
		{ get => LevelRenderer.SizeI; }

		public LevelManagerRenderer(GameManager gameManager)
		{
			GameManager = gameManager;
			LevelRenderer = new LevelRenderer(LevelManager.Level);
		}

		public override void Render(FrameBuffer buffer)
		{
			LevelRenderer.Render(buffer);
		}

		public override void RenderToCache(FrameBuffer buffer)
		{
			LevelRenderer.RenderToCache(buffer);
		}

		public override bool Validate()
		{
			return ValidateCurrentLevel();
		}

		private void RenderEncounter(FrameBuffer buffer)
		{
			// TODO I probably need a dedicated EncounterRenderer
		}

		private bool ValidateCurrentLevel()
		{
			bool invalid = LevelRenderer.Level != LevelManager.Level;

			if (invalid)
				LevelRenderer = new LevelRenderer(LevelManager.Level);

			return !invalid;
		}
	}
}
