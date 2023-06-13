using static Game.World.Map;
using static IO.Render.MapRenderer;

namespace Assets
{
	static class Tiles
	{
		public static readonly Dictionary<char, TileInfo> TILE_INFO = new Dictionary<char, TileInfo>(){
			{'?', new TileInfo("Missing TILE_INFO", false)},
			{' ', new TileInfo("Ground", true)},
			{'s', new TileInfo("Stone Brick Floor", true)},
			{'S', new TileInfo("Stone Brick Wall", false)},
			{'g', new TileInfo("Grass", true)},
			{'r', new TileInfo("Rock Ground", true)},
			{'R', new TileInfo("Rock Wall", false)},
			{'w', new TileInfo("Water (Shallow)", true)},
			{'W', new TileInfo("Water (Deep)", false)},
			{'p', new TileInfo("Wooden Plank", true)},
			{'P', new TileInfo("Wooden Plank Wall", false)}
			};

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
	}
}
