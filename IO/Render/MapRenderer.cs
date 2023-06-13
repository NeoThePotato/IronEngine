using Game.World;
using System.Diagnostics;
using static Game.World.Map;
using static IO.Render.MapRenderer.VisualTileInfo;

namespace IO.Render
{
	class MapRenderer : Renderer
	{
		private const char ENTRY_CHAR = 'E';
		private const byte ENTRY_COLOR = 2;
		private const char EXIT_CHAR = 'X';
		private const byte EXIT_COLOR = 1;
		private const int STRECH_I = 2;
		private FrameBuffer _mapCache;

		private Map Map
		{ get; set; }
		private LevelMetadata LevelMetadata
		{ get; set; }
		public override int SizeJ
		{ get => Map.SizeJ; }
		public override int SizeI
		{ get => Map.SizeI*STRECH_I; }
		public (int, int) Size
		{ get => (SizeJ, SizeI); }
		public (int, int) CacheSize
		{ get => (_mapCache.SizeJ, _mapCache.SizeI); }

		public MapRenderer(Map map, LevelMetadata levelMetadata)
		{
			Map = map;
			LevelMetadata = levelMetadata;
			UpdateCacheSize();
			RenderToCache();
		}

		public override void Render(ref FrameBuffer buffer)
		{
			FrameBuffer.Copy(buffer, _mapCache);
		}

		private void RenderToCache()
		{
			ValidateCacheSize();
			RenderTileDataToCache();
			RenderEntryExitToCache();
		}

		private void RenderTileDataToCache()
		{
			for (int j = 0; j < Map.SizeJ; j++)
			{
				for (int i = 0; i < Map.SizeI; i++)
				{
					var info = GetVisualTileInfo(Map.TileMap[j, i]);
					RenderToCache(j, i, info);
				}
			}
		}

		private void RenderEntryExitToCache()
		{
			(int entJ, int entI) = LevelMetadata.entryTile;
			(int extJ, int extI) = LevelMetadata.exitTile;
			RenderToCache(entJ, entI, ENTRY_CHAR, ENTRY_COLOR);
			RenderToCache(extJ, extI, EXIT_CHAR, EXIT_COLOR);
		}

		private void RenderToCache(int j, int i, VisualTileInfo info)
		{
			_mapCache[j, i * STRECH_I] = GetFrameBufferTuple(info);
			_mapCache[j, i * STRECH_I + 1] = GetFrameBufferTuple(info);
		}

		private void RenderToCache(int j, int i, char c, byte fg)
		{
			RenderToCacheC(j, i, c);
			RenderToCacheFG(j, i, fg);
		}

		private void RenderToCacheC(int j, int i, char c)
		{
			_mapCache.Char[j, i * STRECH_I] = c;
			_mapCache.Char[j, i * STRECH_I + 1] = c;
		}

		private void RenderToCacheFG(int j, int i, byte fg)
		{
			_mapCache.Foreground[j, i * STRECH_I] = fg;
			_mapCache.Foreground[j, i * STRECH_I + 1] = fg;
		}

		private void RenderToCacheBG(int j, int i, byte bg)
		{
			_mapCache.Background[j, i * STRECH_I] = bg;
			_mapCache.Background[j, i * STRECH_I + 1] = bg;
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
			public static readonly Dictionary<char, VisualTileInfo> VISUAL_TILE_INFO = new Dictionary<char, VisualTileInfo>(){
			{'?', new VisualTileInfo(TileInfo.GetTileInfo('?'), '▒', 163, 0)},
			{' ', new VisualTileInfo(TileInfo.GetTileInfo(' '), ' ', 15, 0)},
			{'s', new VisualTileInfo(TileInfo.GetTileInfo('s'), '▓', 244, 239)},
			{'S', new VisualTileInfo(TileInfo.GetTileInfo('S'), '█', 237, 242)},
			{'g', new VisualTileInfo(TileInfo.GetTileInfo('g'), '░', 34, 28)},
			{'r', new VisualTileInfo(TileInfo.GetTileInfo('r'), '░', 250, 247)},
			{'R', new VisualTileInfo(TileInfo.GetTileInfo('R'), '⌂', 241, 238)},
			{'w', new VisualTileInfo(TileInfo.GetTileInfo('w'), '░', 91, 80)},
			{'W', new VisualTileInfo(TileInfo.GetTileInfo('W'), '≈', 33, 32)},
			{'p', new VisualTileInfo(TileInfo.GetTileInfo('p'), '≡', 240, 180)},
			{'P', new VisualTileInfo(TileInfo.GetTileInfo('P'), '▒', 52, 137)},
			};

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
				if (VISUAL_TILE_INFO.TryGetValue(c, out var info))
					return info;
				else
				{
					Debug.WriteLine($"No VISUAL_TILE_INFO found for character '{c}'.");
					return VISUAL_TILE_INFO['?'];
				}
			}
		}
	}
}
