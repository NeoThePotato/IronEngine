using Game.World;

namespace IO.Render
{
	class LevelRenderer : Renderer
	{
		public Level Level
		{ get; private set; }
		private MapRenderer MapRenderer
		{ get; set; }
		private EntityRenderer EntityRenderer
		{ get; set; }
		public override int SizeJ
		{ get => MapRenderer.SizeJ; }
		public override int SizeI
		{ get => MapRenderer.SizeI; }

		public LevelRenderer(Level level)
		{
			Level = level;
			MapRenderer = new MapRenderer(Level.Map);
			EntityRenderer = new EntityRenderer(this);
		}

		public override void Render(FrameBuffer buffer)
		{
			EntityRenderer.Render(buffer);
		}

		public override void RenderToCache(FrameBuffer buffer)
		{
			MapRenderer.RenderToCache(buffer);
		}
	}
}
