using System.Diagnostics;
using Game.World;
using static Game.World.Map;
using Assets;

namespace IO.Render
{
	class MapRenderer : Renderer
	{
		public const int STRECH_I = 2;
		private FrameBuffer _mapCache;

		private Map Map
		{ get; set; }
		public override int SizeJ
		{ get => Map.TileSizeJ; }
		public override int SizeI
		{ get => Map.TileSizeI*STRECH_I; }
		public (int, int) Size
		{ get => (SizeJ, SizeI); }
		public (int, int) CacheSize
		{ get => (_mapCache.SizeJ, _mapCache.SizeI); }

		public MapRenderer(Map map)
		{
			Map = map;
			UpdateCacheSize();
			RenderToLocalCache();
		}

		public override void Render(FrameBuffer buffer)
		{
			FrameBuffer.Copy(buffer, _mapCache);
		}

		public override void RenderToCache(FrameBuffer buffer)
		{
			Render(buffer);
		}

		public override bool Validate()
		{
			return ValidateCacheSize();
		}

		public static (int, int) PointToCharPos(Point2D point)
		{
			int charPosJ = point.TileJ;
			int charPosI = (point.TileI * STRECH_I) + STRECH_I * Point2D.PointRemainder(point.PointI) / Point2D.POINTS_PER_TILE;

			return (charPosJ, charPosI);
		}

		private void RenderToLocalCache()
		{
			ValidateCacheSize();
			RenderTileDataToCache();
		}

		private void RenderTileDataToCache()
		{
			for (int j = 0; j < Map.TileSizeJ; j++)
			{
				for (int i = 0; i < Map.TileSizeI; i++)
				{
					RenderToCache(j, i, GetTileInfoAt(j, i));
				}
			}
		}

		private void RenderToCache(int j, int i, VisualTileInfo info)
		{
			_mapCache[j, i * STRECH_I] = VisualTileInfo.GetFrameBufferTuple(info);
			_mapCache[j, i * STRECH_I + 1] = VisualTileInfo.GetFrameBufferTuple(info);
		}

		private bool ValidateCacheSize()
		{
			bool valid = CacheSize == Size;

			if (!valid)
				UpdateCacheSize();

			return valid;
		}

		private void UpdateCacheSize()
		{
			_mapCache = new FrameBuffer(SizeJ, SizeI);
		}

		private VisualTileInfo GetTileInfoAt(int j, int i)
		{
			return GetTileInfoAt(Map, j, i);
		}

		public static VisualTileInfo GetTileInfoAt(Map map, int j, int i)
		{
			return VisualTileInfo.GetVisualTileInfo(map.TileMap[j, i]);
		}

		public static VisualTileInfo GetTileInfoAt(Map map, Point2D point)
		{
			return GetTileInfoAt(map, point.TileJ, point.TileI);
		}

		public struct VisualTileInfo
		{
			public TileInfo tileInfo;
			public char character;
			public byte foregroundColor;
			public byte backgroundColor;


			public VisualTileInfo(TileInfo tileInfo, char character, byte foregroundColor, byte backgroundColor)
			{
				this.tileInfo = tileInfo;
				this.character = character;
				this.foregroundColor = foregroundColor;
				this.backgroundColor = backgroundColor;
			}

			public readonly (char, byte, byte) GetFrameBufferTuple()
			{
				return GetFrameBufferTuple(this);
			}

			public static (char, byte, byte) GetFrameBufferTuple(VisualTileInfo info)
			{
				return (info.character, info.foregroundColor, info.backgroundColor);
			}

			public static VisualTileInfo GetVisualTileInfo(char c)
			{
				if (Tiles.VISUAL_TILE_INFO.TryGetValue(c, out var info))
					return info;
				else
				{
					Debug.WriteLine($"No VISUAL_TILE_INFO found for character '{c}'.");
					return Tiles.VISUAL_TILE_INFO['?'];
				}
			}
		}
	}
}
