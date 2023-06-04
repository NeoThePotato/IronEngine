using Game.World;

namespace IO.Render
{
	class MapRenderer : Renderer
	{
		private Map Map
		{ get; set; }
		public override int SizeJ
		{ get => Map.SizeJ; }
		public override int SizeI
		{ get => Map.SizeI; }

		public MapRenderer(Map map)
		{
			Map = map;
		}

		public override void Render(ref char[,] buffer)
		{
			for (int j = 0; j < SizeJ; j++)
			{
				for (int i = 0; i < SizeI; i++)
				{
					buffer[OffsetJ+j, OffsetI+i] = Map.TileMap[j, i];
				}
			}
		}
	}
}
