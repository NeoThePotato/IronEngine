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
				RenderEntity(ref buffer, entity);
			}
		}

		public void RenderEntity(ref FrameBuffer buffer, MapEntity entity)
		{
			buffer.Char[entity.posJ, entity.posI * 2] = '@';
			buffer.Foreground[entity.posJ, entity.posI * 2] = 15;
			buffer.Char[entity.posJ, entity.posI * 2 +1] = '@';
			buffer.Foreground[entity.posJ, entity.posI * 2+1] = 15;
		}
	}
}
