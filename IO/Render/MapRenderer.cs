using Game.World;
using System.Diagnostics;
using System.Drawing;
using static Game.World.Map;

namespace IO.Render
{
	class MapRenderer : Renderer
	{
		private FrameBuffer _mapCache;
		private Map Map
		{ get; set; }
		public override int SizeJ
		{ get => Map.SizeJ; }
		public override int SizeI
		{ get => Map.SizeI*2; }
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

		public override void Render(ref FrameBuffer buffer)
		{
			FrameBuffer.Copy(buffer, _mapCache);
		}

		private void RenderToCache()
		{
			ValidateCacheSize();

			for (int j = 0; j < Map.SizeJ; j++)
			{
				for (int i = 0; i < Map.SizeI; i++)
				{
					var info = VisualTileInfo.GetFrameBufferTuple(VisualTileInfo.GetVisualTileInfo(Map.TileMap[j, i]));
					_mapCache[j, i * 2] = info;
					_mapCache[j, i * 2 + 1] = info;
				}
			}
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
