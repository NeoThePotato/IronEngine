using Game.World;
using System.Diagnostics;
using static Game.World.Map;

namespace IO.Render
{
	class MapRenderer : Renderer
	{
		private Map Map
		{ get; set; }
		public override int SizeJ
		{ get => Map.SizeJ; }
		public override int SizeI
		{ get => Map.SizeI*2; }

		public MapRenderer(Map map)
		{
			Map = map;
		}

		public override void Render(ref FrameBuffer buffer)
		{
			for (int j = 0; j < Map.SizeJ; j++)
			{
				for (int i = 0; i < Map.SizeI; i++)
				{
					var info = VisualTileInfo.GetFrameBufferTuple(VisualTileInfo.GetVisualTileInfo(Map.TileMap[j, i]));
					buffer[j, i*2] = info;
					buffer[j, i*2+1] = info;
				}
			}
		}

		public struct VisualTileInfo
		{
			public static readonly Dictionary<char, VisualTileInfo> VISUAL_TILE_INFO = new Dictionary<char, VisualTileInfo>(){
			{'?', new VisualTileInfo(new TileInfo("Missing VISUAL_TILE_INFO", false), '▒', 163, 0)},
			{' ', new VisualTileInfo(TileInfo.GetTileInfo(' '), ' ', 15, 0)},
			{'w', new VisualTileInfo(TileInfo.GetTileInfo('w'), '█', 237, 242)}
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
					Debug.Write($"No VISUAL_TILE_INFO found for character '{c}'.");
					return VISUAL_TILE_INFO['?'];
				}
			}
		}
	}
}
