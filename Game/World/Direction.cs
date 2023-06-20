namespace Game.World
{
	struct Direction
	{
		public const int MAX_MAG = Point2D.POINTS_PER_TILE;
		private static readonly Direction[] DIRECTION_VECTORS = {
			new Direction(0, MAX_MAG),
			new Direction(-MAX_MAG, MAX_MAG),
			new Direction(-MAX_MAG, 0),
			new Direction(-MAX_MAG, -MAX_MAG),
			new Direction(0, -MAX_MAG),
			new Direction(MAX_MAG, -MAX_MAG),
			new Direction(MAX_MAG, 0),
			new Direction(MAX_MAG, MAX_MAG),
			new Direction(0, 0)
		};

		public Point2D Dir
		{ get; set; }
		public int Mag
		{ get => Utility.Magnitude((Dir.PointJ, Dir.PointI)); }

		public Direction(Point2D p)
		{
			Dir = p;
		}

		public Direction(Direction dir)
		{
			Dir = dir.Dir;
		}

		public Direction((int, int) vector)
		{
			Dir = new Point2D(vector);
		}

		public Direction(int vecJ, int vecI)
		{
			Dir = new Point2D((vecJ, vecI));
		}

		public static Point2D operator +(Point2D p, Direction dir)
			=> p + dir.Dir;

		public static Point2D operator -(Point2D p, Direction dir)
			=> p - dir.Dir;

		public static Direction operator *(Direction dir, int mag)
			=> new Direction(dir.Dir * mag);

		public static Direction TranslateDirection(Directions dir)
		{
			return DIRECTION_VECTORS[(int)dir];
		}

		public static Direction GetRandomDirection()
		{
			return TranslateDirection((Directions)Random.Shared.Next(0, Enum.GetNames(typeof(Directions)).Length));
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