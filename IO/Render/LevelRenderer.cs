using Game.World;

namespace IO.Render
{
	class LevelRenderer : Renderer
	{
		private Level Level
		{ get; set; }
		private MapRenderer MapRenderer
		{ get; set; }
		public override int SizeJ
		{ get => MapRenderer.SizeJ; }
		public override int SizeI
		{ get => MapRenderer.SizeI; }

		public LevelRenderer(Level levelManager)
		{
			Level = levelManager;
			MapRenderer = new MapRenderer(Level.Map, Level.Metadata);
		}

		public override void Render(FrameBuffer buffer)
		{
			// Render level
			MapRenderer.Render(buffer);

			// Render entities
			foreach (var entity in Level.Entities)
			{
				RenderEntity(buffer, entity, 'x');
			}
		}

		public void RenderEntity(FrameBuffer buffer, MapEntity entity, char c = 'x', byte color = 15)
		{
			int posJ = entity.PosJ;
			int posI = entity.PosI * MapRenderer.STRECH_I;
			buffer.Char[posJ, posI] = c;
			buffer.Char[posJ, posI + 1] = c;
			buffer.Foreground[posJ, posI] = color;
			buffer.Foreground[posJ, posI + 1] = color;
		}
	}
}
