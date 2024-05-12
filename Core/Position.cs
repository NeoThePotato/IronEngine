using System.Diagnostics;
using static System.Math;

namespace IronEngine
{
	/// <summary>
	/// Contains a position in 2D space.
	/// </summary>
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
		/// <seealso cref="DistanceEuclidean"/>
		/// <returns>Euclidean distance between <paramref name="p1"/> and <paramref name="p2"/> Alias of <see cref="DistanceEuclidean"/>.</returns>
		public static double Distance(Position p1, Position p2) => DistanceEuclidean(p1, p2);

		/// <returns>Euclidean distance between <paramref name="p1"/> and <paramref name="p2"/>.</returns>
		public static double DistanceEuclidean(Position p1, Position p2) => (p2 - p1).MagnitudeEuclidean;

		/// <returns>Chebyshev distance between <paramref name="p1"/> and <paramref name="p2"/>.</returns>
		public static int DistanceChebyshev(Position p1, Position p2) => (p2 - p1).MagnitudeChebyshev;

		/// <returns>Taxicab distance between <paramref name="p1"/> and <paramref name="p2"/>.</returns>
		public static int DistanceTaxicab(Position p1, Position p2) => (p2 - p1).MagnitudeTaxicab;
		#endregion

		#region OVERRIDES
		public readonly bool Equals(Position other) => (x == other.x) && (y == other.y);

		public override bool Equals(object obj) => obj is Position position && Equals(position);

		public override readonly int GetHashCode() => (x, y).GetHashCode();

		public override readonly string ToString() => $"(X: {x}, Y: {y})";
		#endregion
	}
	
	/// <summary>
	/// Interface for all instances which have a <see cref="Position"/> on a <see cref="Tile"/> inside a <see cref="TileMap"/>.
	/// </summary>
	public interface IPositionable
	{
		/// <summary>
		/// The <see cref="TileMap"/> of this instance.
		/// </summary>
		TileMap TileMap { get; }

		/// <summary>
		/// The <see cref="Tile"/> of this instance.
		/// </summary>
		Tile CurrentTile { get; }

		/// <summary>
		/// The <see cref="Position"/> of this instance on the <see cref="TileMap"/>.
		/// </summary>
		Position Position { get; }
	}
}
