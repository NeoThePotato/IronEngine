using Game.World;

namespace IO.Render
{
	class LevelManagerRenderer : Renderer
	{
		private LevelManager LevelManager
		{ get; set; }
		private MapRenderer MapRenderer
		{ get; set; }
		public override int SizeJ
		{ get => MapRenderer.SizeJ; }
		public override int SizeI
		{ get => MapRenderer.SizeI; }

		public LevelManagerRenderer(LevelManager levelManager)
		{
			LevelManager = levelManager;
			MapRenderer = new MapRenderer(LevelManager.Map);
		}

		public override void Render(ref FrameBuffer buffer)
		{
			// Render level
			MapRenderer.Render(ref buffer);

			// Render entities
			foreach (var entity in LevelManager.Entities)
			{
				buffer.Char[entity.posJ, entity.posI] = '@';
				buffer.Foreground[entity.posJ, entity.posI] = 15;
			}
		}
	}
}
