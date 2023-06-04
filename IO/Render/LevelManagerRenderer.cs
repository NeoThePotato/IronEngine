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
		{ get => LevelManager.Map.SizeJ; }
		public override int SizeI
		{ get => LevelManager.Map.SizeI; }

		public LevelManagerRenderer(LevelManager levelManager)
		{
			LevelManager = levelManager;
			MapRenderer = new MapRenderer(LevelManager.Map);
		}

		public override void Render(ref char[,] buffer)
		{
			// Render level
			MapRenderer.Render(ref buffer);

			// Render entities
			foreach (var entity in LevelManager.Entities)
				buffer[OffsetJ+entity.posJ, OffsetI+entity.posI] = '@';
		}
	}
}
