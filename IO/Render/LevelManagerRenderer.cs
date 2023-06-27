﻿using Game;

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
			switch (LevelManager.State)
			{
				case LevelManager.LevelState.Encounter:
					RenderEncounter(buffer);
					break;
				case LevelManager.LevelState.World:
					RenderWorld(buffer);
					break;
			}
		}

		public override void Validate()
		{
			ValidateCurrentLevel();
		}

		private void RenderEncounter(FrameBuffer buffer)
		{
            // TODO I probably need a dedicated EncounterRenderer
        }

        private void RenderWorld(FrameBuffer buffer)
		{
			LevelRenderer.Render(new FrameBuffer(buffer, 1, 1));
		}

		private void ValidateCurrentLevel()
		{
			if (LevelRenderer.Level != LevelManager.Level)
				LevelRenderer = new LevelRenderer(LevelManager.Level);
		}
	}
}
