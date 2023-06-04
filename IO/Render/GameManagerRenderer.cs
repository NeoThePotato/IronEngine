using Game;
using Game.World;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private GameManager Manager;
		public override int SizeJ
		{ get => Manager.LevelManager.Map.SizeJ; }
		public override int SizeI
		{ get => Manager.LevelManager.Map.SizeI; }

		public GameManagerRenderer(GameManager gameManager)
		{
			Manager = gameManager;
		}

		public override void Render(ref char[,] buffer)
		{
			// Render level
			for (int j = 0; j < SizeJ; j++)
			{
				for (int i = 0; i < SizeI; i++)
				{
					buffer[OffsetJ+j, OffsetI+i] = Manager.LevelManager.Map.TileMap[j, i];
				}
			}

			// Render entities
			foreach (var entity in Manager.Entities)
				buffer[OffsetJ+entity.posJ, OffsetI+entity.posI] = '@';
		}
	}
}
