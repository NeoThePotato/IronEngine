using static Game.World.Map;
using static IO.Render.MapRenderer;
using static Game.World.Map.TileInfo;

namespace Assets
{
	static class Tiles
	{
		public static readonly Dictionary<char, TileInfo> TILE_INFO = new()
		{
			{'?', new("Missing TILE_INFO", false)},
			{' ', new("Ground", true)},
			{'s', new("Stone Brick Floor", true)},
			{'S', new("Stone Brick Wall", false)},
			{'g', new("Grass", true)},
			{'r', new("Road", true)},
			{'R', new("Rock Wall", false)},
			{'w', new("Water (Shallow)", true)},
			{'W', new("Water (Deep)", false)},
			{'p', new("Wooden Plank", true)},
			{'P', new("Wooden Plank Wall", false)}
		};

		public static readonly Dictionary<char, VisualTileInfo> VISUAL_TILE_INFO = new()
		{
			{'?', new(GetTileInfo('?'), '▒', 163, 0)},
			{' ', new(GetTileInfo(' '), ' ', 15, 0)},
			{'s', new(GetTileInfo('s'), '▓', 244, 239)},
			{'S', new(GetTileInfo('S'), '█', 237, 242)},
			{'g', new(GetTileInfo('g'), '░', 34, 28)},
			{'r', new(GetTileInfo('r'), '░', 250, 247)},
			{'R', new(GetTileInfo('R'), '⌂', 241, 238)},
			{'w', new(GetTileInfo('w'), '░', 69, 80)},
			{'W', new(GetTileInfo('W'), '≈', 33, 32)},
			{'p', new(GetTileInfo('p'), '≡', 137, 180)},
			{'P', new(GetTileInfo('P'), '▒', 52, 137)},
		};
	}
}
