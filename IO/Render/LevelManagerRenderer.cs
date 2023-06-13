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
			MapRenderer = new MapRenderer(LevelManager.Map, LevelManager.Metadata);
		}

		public override void Render(ref FrameBuffer buffer)
		{
			// Render level
			MapRenderer.Render(ref buffer);

			// Render entities
			foreach (var entity in LevelManager.Entities)
			{
				RenderEntity(ref buffer, entity, 'x');
			}
		}

		public void RenderEntity(ref FrameBuffer buffer, MapEntity entity, char c = 'x', byte color = 15)
		{
			buffer.Char[entity.PosJ, entity.PosI * 2] = c;
			buffer.Foreground[entity.PosJ, entity.PosI * 2] = color;
			buffer.Char[entity.PosJ, entity.PosI * 2 +1] = c;
			buffer.Foreground[entity.PosJ, entity.PosI * 2+1] = color;
		}
	}
}
