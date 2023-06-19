using Game;

namespace IO.Render
{
	class LevelManagerRenderer : Renderer
	{
		public LevelManager LevelManager
		{ get; private set; }
		public LevelRenderer LevelRenderer
		{ get; private set; }
		public GameUIManagerRenderer UIManagerRenderer
		{ get; private set; }
		public ContainerMenuManagerRenderer ContainerMenuManagerRenderer
		{ get; private set; }
		public override int SizeJ
		{ get => LevelRenderer.SizeJ; }
		public override int SizeI
		{ get => LevelRenderer.SizeI; }

		public LevelManagerRenderer(LevelManager levelManager, GameUIManagerRenderer uiManagerRenderer)
		{
			UIManagerRenderer = uiManagerRenderer;
			LevelManager = levelManager;
			LevelRenderer = new LevelRenderer(LevelManager.Level);
		}

		public override void Render(FrameBuffer buffer)
		{
			switch (LevelManager.State)
			{
				case LevelManager.GameState.Encounter:
					RenderEncounter(buffer);
					break;
				case LevelManager.GameState.World:
					RenderWorld(buffer);
					break;
			}

			UIManagerRenderer.Render(buffer);
		}

		private void RenderEncounter(FrameBuffer buffer) // TODO I probably need an EncounterRenderer
		{
			if (LevelManager.EncounterManager._encounterType == Game.World.EncounterManager.EncounterType.Container)
			{
				ContainerMenuManagerRenderer = new ContainerMenuManagerRenderer(LevelManager.EncounterManager._containerMenuManager);
				ContainerMenuManagerRenderer.Render(buffer);
			}
		}

		private void RenderWorld(FrameBuffer buffer)
		{
			RenderLevelAndEntities(new FrameBuffer(buffer, 1, 1));
		}

		private void RenderLevelAndEntities(FrameBuffer buffer)
		{
			LevelRenderer.Render(buffer);
			RenderPlayer(buffer);
		}

		private void RenderPlayer(FrameBuffer buffer)
		{
			LevelRenderer.RenderEntity(buffer, LevelManager.PlayerEntity, '@', 15);
		}
	}
}
