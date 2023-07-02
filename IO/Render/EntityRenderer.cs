using Game.World;

namespace IO.Render
{
	class EntityRenderer : Renderer
	{
		public LevelRenderer LevelRenderer
		{ get; private set; }
		public Level Level
		{ get => LevelRenderer.Level; }
		public override int SizeJ
		{ get => LevelRenderer.SizeJ; }
		public override int SizeI
		{ get => LevelRenderer.SizeI; }

		public EntityRenderer(LevelRenderer levelRenderer)
		{
			LevelRenderer = levelRenderer;
		}

		public override void Render(FrameBuffer buffer)
		{
			RenderAllEntities(buffer);
		}

		private void RenderAllEntities(FrameBuffer buffer)
		{
			foreach (var entity in Level.Entities)
				RenderEntity(buffer, entity);
		}

		private static void RenderEntity(FrameBuffer buffer, LevelEntity entity)
		{
			if (entity.Moveable)
				RenderEntityMoving(buffer, entity);
			else
				RenderEntityStatic(buffer, entity);
		}

		private static void RenderEntityMoving(FrameBuffer buffer, LevelEntity entity)
		{
			VisualEntityInfo visualInfo = entity.Entity.VisualInfo;
			(int posJ, int posI) = MapRenderer.PointToCharPos(entity.Pos);
			RenderEntityAtTile(buffer, posJ, posI, visualInfo);
		}

		private static void RenderEntityStatic(FrameBuffer buffer, LevelEntity entity)
		{
			VisualEntityInfo visualInfo = entity.Entity.VisualInfo;
			(int posJ, int posI) = MapRenderer.PointToCharPos(entity.Pos);
			RenderEntityAtTile(buffer, posJ, posI, visualInfo);
			RenderEntityAtTile(buffer, posJ, posI + 1, visualInfo);
		}

		private static void RenderEntityAtTile(FrameBuffer buffer, int posJ, int posI, VisualEntityInfo visualInfo)
		{
			buffer.Char[posJ, posI] = visualInfo.character ?? buffer.Char[posJ, posI];
			buffer.Foreground[posJ, posI] = visualInfo.foregroundColor ?? buffer.Foreground[posJ, posI];
		}

		public struct VisualEntityInfo
		{
			public char? character;
			public byte? foregroundColor;

			public VisualEntityInfo(char? character, byte? foregroundColor)
			{
				this.character = character;
				this.foregroundColor = foregroundColor;
			}
		}
	}
}
