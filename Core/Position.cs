using System.Diagnostics;
using static System.Math;

namespace IronEngine
{
	public readonly struct Position : IEquatable<Position>
	{
		public readonly int x;
		public readonly int y;

		public Position(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Position(Position other)
		{
			x = other.x;
			y = other.y;
		}

		public readonly void Deconstruct(out int x, out int y)
		{
			x = this.x;
			y = this.y;
		}

		public static Position Origin => new(0, 0);

		public static Position OutOfBounds => new(-1, -1);

		#region OPERATORS
		public static Position operator +(Position p) => p;
									   
		public static Position operator -(Position p) => new(-p.x, -p.y);
									   
		public static Position operator +(Position p1, Position p2) => new(p1.x + p2.x, p1.y + p2.y);
									   
		public static Position operator -(Position p1, Position p2) => new(p1.x - p2.x, p1.y - p2.y);
									   
		public static Position operator *(Position p, int num) => new(p.x * num, p.y * num);

		public static Position operator /(Position p, int num) => new(p.x / num, p.y / num);

		public static bool operator ==(Position p1, Position p2) => p1.Equals(p2);

		public static bool operator !=(Position p1, Position p2) => !p1.Equals(p2);
		#endregion

		#region MAGNITUDE
		private readonly double MagnitudeEuclidean => Sqrt(x * x + y * y);

		private readonly int MagnitudeChebyshev => Abs(Max(x, y));

		private readonly int MagnitudeTaxicab => Abs(x) + Abs(y);
		#endregion

		#region DISTANCE
		public static double Distance(Position p1, Position p2) => DistanceEuclidean(p1, p2);

		public static double DistanceEuclidean(Position p1, Position p2) => (p2 - p1).MagnitudeEuclidean;

		public static int DistanceChebyshev(Position p1, Position p2) => (p2 - p1).MagnitudeChebyshev;

		public static int DistanceTaxicab(Position p1, Position p2) => (p2 - p1).MagnitudeTaxicab;
		#endregion

		#region OVERRIDES
		public readonly bool Equals(Position other) => (x == other.x) && (y == other.y);

		public override bool Equals(object obj) => obj is Position position && Equals(position);

		public override readonly int GetHashCode() => (x, y).GetHashCode();

		public override readonly string ToString() => $"(X: {x}, Y: {y})";
		#endregion
	}
	
	public interface IPositionable
	{
		TileMap TileMap { get; }

		Tile CurrentTile { get; }

		Position Position { get; }

		#region VALIDTY_CHECKS
		public bool CheckHasTileMap()
		{
			bool hasTileMap = TileMap != null;
			Debug.WriteLineIf(!hasTileMap, $"{this} is not on a TileMap.");
			return hasTileMap;
		}

		public bool CheckWithinTileMap(Position position)
		{
			bool withinTileMap = TileMap.WithinBounds(position);
			Debug.WriteLineIf(!withinTileMap, $"{position} is not within the bounds of TileMap.");
			return withinTileMap;
		}

		public bool CheckSameTileMap(Tile tile)
		{
			bool sameTileMap = CurrentTile.SameTileMap(tile);
			Debug.WriteLineIf(!sameTileMap, $"{tile} is not on the same TileMap as {this}.");
			return sameTileMap;
		}
		#endregion
	}
}
