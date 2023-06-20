using System.Diagnostics.CodeAnalysis;

namespace Game.World
{
	struct Point2D
	{
		public const int POINTS_PER_TILE = 256;
		public int PointJ
		{ get; set; }
		public int PointI
		{ get; set; }
		public int TileJ
		{ get => PointToTile(PointJ); set => PointJ = TileToPoint(value) + PointRemainder(PointJ); }
		public int TileI
		{ get => PointToTile(PointI); set => PointI = TileToPoint(value) + PointRemainder(PointI); }

		public Point2D(int pointJ, int pointI)
		{
			PointJ = pointJ;
			PointI = pointI;
		}

		public Point2D((int, int) tuple)
		{
			PointJ = tuple.Item1;
			PointI = tuple.Item2;
		}

		public Point2D(Point2D other)
		{
			PointJ = other.PointJ;
			PointI = other.PointI;
		}

		public static Point2D Tile(int tileJ, int tileI)
		{
			return new Point2D(TileToPoint(tileJ), TileToPoint(tileI));
		}

		public static Point2D operator +(Point2D p)
			=> p;
									   
		public static Point2D operator -(Point2D p)
			=> new Point2D(-p.PointJ, -p.PointI);
									   
		public static Point2D operator +(Point2D p1, Point2D p2)
			=> new Point2D(p1.PointJ + p2.PointJ, p1.PointI + p2.PointI);
									   
		public static Point2D operator -(Point2D p1, Point2D p2)
			=> p1 - p2;
									   
		public static Point2D operator *(Point2D p, int num)
			=> new Point2D(p.PointJ * num, p.PointI * num);

		public static Point2D operator /(Point2D p, int num)
			=> new Point2D(p.PointJ / num, p.PointI / num);

		public static bool operator ==(Point2D p1, Point2D p2)
			=> SamePoint(p1, p2);

		public static bool operator !=(Point2D p1, Point2D p2)
			=> !SamePoint(p1, p2);

		public static bool SamePoint(Point2D p1, Point2D p2)
			=> (p1.PointJ == p2.PointJ) & (p1.PointI == p2.PointI);

		public static bool SameTile(Point2D p1, Point2D p2)
			=> (p1.TileJ == p2.TileJ) & (p1.TileI == p2.TileI);

		public static int DistanceAbs(Point2D p1, Point2D p2)
		{
			var p = p1 - p2;
			return (int)(Math.Pow(Math.Abs(p.TileI), 2) + Math.Pow(Math.Abs(p.TileJ), 2));
		}

		public static int DistanceSq(Point2D p1, Point2D p2)
		{
			return (int)Math.Sqrt(DistanceAbs(p1, p2));
		}

		private static int TileToPoint(int tile)
			=> tile * POINTS_PER_TILE;

		private static int PointToTile(int point)
			=> point / POINTS_PER_TILE;

		private static int PointRemainder(int point)
			=> point % POINTS_PER_TILE;

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() << 2;
		}

		public override string ToString()
		{
			return $"({TileJ}.{PointRemainder(PointJ)}/{POINTS_PER_TILE}, {TileI}.{PointRemainder(PointI)}/{POINTS_PER_TILE})";
		}
	}
}
