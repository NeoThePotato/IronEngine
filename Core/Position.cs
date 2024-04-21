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
		private readonly double MagnitudeEuclidean => Math.Sqrt(x * x + y * y);

		private readonly int MagnitudeChebyshev => Math.Abs(Math.Max(x, y));

		private readonly int MagnitudeTaxicab => Math.Abs(x) + Math.Abs(y);
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
}
