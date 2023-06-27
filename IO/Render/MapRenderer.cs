using System.Diagnostics;
using Game.World;
using Assets;
using static Game.World.Map;

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
			RenderToCache();
		}

		public override void Render(FrameBuffer buffer)
		{
			FrameBuffer.Copy(buffer, _mapCache);
		}

		private void RenderToCache()
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
					var info = VisualTileInfo.GetVisualTileInfo(Map.TileMap[j, i]);
					RenderToCache(j, i, info);
				}
			}
		}

		private void RenderToCache(int j, int i, VisualTileInfo info)
		{
			_mapCache[j, i * STRECH_I] = VisualTileInfo.GetFrameBufferTuple(info);
			_mapCache[j, i * STRECH_I + 1] = VisualTileInfo.GetFrameBufferTuple(info);
		}

		private void ValidateCacheSize()
		{
			if (CacheSize != Size)
				UpdateCacheSize();
		}

		private void UpdateCacheSize()
		{
			_mapCache = new FrameBuffer(SizeJ, SizeI);
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

			public (char, byte, byte) GetFrameBufferTuple()
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
