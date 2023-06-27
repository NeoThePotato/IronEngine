using Game.World;
using System;

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

		private void RenderEntity(FrameBuffer buffer, MapEntity entity)
		{
			if (entity.Moveable)
				RenderEntityMoving(buffer, entity);
			else
				RenderEntityStatic(buffer, entity);
		}

		private void RenderEntityMoving(FrameBuffer buffer, MapEntity entity)
		{
			VisualEntityInfo visualInfo = entity.Entity.VisualInfo;
			int posJ = entity.Pos.TileJ;
			int posI = (entity.Pos.TileI * MapRenderer.STRECH_I);
			int subTileI = MapRenderer.STRECH_I * Point2D.PointRemainder(entity.Pos.PointI) / Point2D.POINTS_PER_TILE;
			posI += subTileI;
			RenderEntityAtTile(buffer, posJ, posI, visualInfo);
		}

		private void RenderEntityStatic(FrameBuffer buffer, MapEntity entity)
		{
			VisualEntityInfo visualInfo = entity.Entity.VisualInfo;
			int posJ = entity.Pos.TileJ;
			int posI = (entity.Pos.TileI * MapRenderer.STRECH_I);
			RenderEntityAtTile(buffer, posJ, posI, visualInfo);
			RenderEntityAtTile(buffer, posJ, posI + 1, visualInfo);
		}

		private void RenderEntityAtTile(FrameBuffer buffer, int posJ, int posI, VisualEntityInfo visualInfo)
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
