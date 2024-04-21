namespace IronEngine
{
	struct Direction
	{
		public const double SQRT2OVER2 = 0.7071067812;
		public const int MAX_MAG = POINTS_PER_TILE;
		public const int MAX_DIAGONAL_MAG = (int)(MAX_MAG * SQRT2OVER2);
		private static readonly Direction[] DIRECTION_VECTORS = {
			new Direction(0, MAX_MAG),
			new Direction(-MAX_DIAGONAL_MAG, MAX_DIAGONAL_MAG),
			new Direction(-MAX_MAG, 0),
			new Direction(-MAX_DIAGONAL_MAG, -MAX_DIAGONAL_MAG),
			new Direction(0, -MAX_MAG),
			new Direction(MAX_DIAGONAL_MAG, -MAX_DIAGONAL_MAG),
			new Direction(MAX_MAG, 0),
			new Direction(MAX_DIAGONAL_MAG, MAX_DIAGONAL_MAG),
			new Direction(0, 0)
		};

		public Position Dir
		{ get; set; }
		public readonly int Mag
		{ get => Math.Magnitude((Dir.X, Dir.Y)); }

		public Direction(Position p)
		{
			Dir = p;
		}

		public Direction(Direction dir)
		{
			Dir = dir.Dir;
		}

		public Direction(Position source, Position target)
		{
			Dir = target - source;
		}

		public Direction((int, int) vector)
		{
			Dir = new Position(vector);
		}

		public Direction(int vecJ, int vecI)
		{
			Dir = new Position((vecJ, vecI));
		}

		public static Position operator +(Position p, Direction dir)
			=> p + dir.Dir;

		public static Position operator -(Position p, Direction dir)
			=> p - dir.Dir;

		public static Direction operator *(Direction dir, int mag)
			=> new(dir.Dir * mag);

		public static Direction operator /(Direction dir, int mag)
			=> new(dir.Dir / mag);

		public void ClampMagnitude()
		{
			this = ClampMagnitude(this);
		}

		public static Direction ClampMagnitude(Direction dir)
		{
			return ClampMagnitude(dir, MAX_MAG);
		}

		public static Direction ClampMagnitude(Direction dir, int mag)
		{
			return new(Math.ClampVectorMagnitude((dir.Dir.X, dir.Dir.Y), mag));
		}

		public static Direction TranslateDirection(Directions dir)
		{
			return DIRECTION_VECTORS[(int)dir];
		}

		public static Direction GetRandomDirection()
		{
			return TranslateDirection((Directions)Random.Shared.Next(0, Enum.GetNames(typeof(Directions)).Length));
		}

		public override readonly string ToString()
		{
			return Dir.ToString();
		}

		public enum Directions
		{
			E = 0,
			NE = 1,
			N = 2,
			NW = 3,
			W = 4,
			SW = 5,
			S = 6,
			SE = 7,
			None = 8
		}
	}
}