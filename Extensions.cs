﻿using System.Text.RegularExpressions;

namespace IronEngine
{
	public static class Extensions
	{
		/// <summary>
		/// Swaps <paramref name="obj1"/> and <paramref name="obj2"/>.
		/// </summary>
		public static void Swap<T>(ref T obj1, ref T obj2) => (obj2, obj1) = (obj1, obj2);

		/// <summary>
		/// Swaps elements from <paramref name="list"/> at indexes <paramref name="idx1"/> and <paramref name="idx2"/>.
		/// </summary>
		public static void Swap<T>(IList<T> list, int idx1, int idx2) => (list[idx2], list[idx1]) = (list[idx1], list[idx2]);

		/// <returns>A random element from <paramref name="array"/>.</returns>
		public static T GetRandom<T>(this T[] array) => array.GetRandom(Random.Shared);

		/// <returns>A random element from <paramref name="array"/>.</returns>
		public static T GetRandom<T>(this T[] array, Random random) => array[random.Next(array.Length)];

		/// <returns>A random element from <paramref name="enumerable"/>.</returns>
		public static T GetRandom<T>(this IEnumerable<T> enumerable, Random random) => enumerable.ElementAt(random.Next(enumerable.Count()));

		/// <returns>A random element from <paramref name="enumerable"/>.</returns>
		public static T GetRandom<T>(this IEnumerable<T> enumerable) => enumerable.GetRandom(Random.Shared);

		/// <summary>
		/// Shuffles the elements of <paramref name="list"/> to be in a random order.
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			if (list.IsReadOnly)
				return;
			for (int i = 0; i < list.Count; i++)
				Swap(list, i, Random.Shared.Next(list.Count));
		}

		/// <summary>
		/// Returns whether <paramref name="position"/> is within the bounds of <paramref name="tileMap"/>.
		/// </summary>
		/// <param name="tileMap"><see cref="TileMap"/> to check.</param>
		/// <param name="position"><see cref="Position"/> to check.</param>
		/// <returns>Whether <paramref name="position"/> is within the bounds of <paramref name="tileMap"/>.</returns>
		public static bool WithinBounds(this TileMap tileMap, Position position) => position.x >= 0 && position.x < tileMap.SizeX && position.y >= 0 && position.y < tileMap.SizeY;

		/// <summary>
		/// Returns a random <see cref="Position"/> within the bounds of <paramref name="tileMap"/>.
		/// Uses the <see cref="Random.Shared"/> instance.
		/// </summary>
		/// <param name="tileMap"><see cref="TileMap"/> to return a <see cref="Position"/> from.</param>
		/// <returns>A random <see cref="Position"/> within the bounds of <paramref name="tileMap"/>.</returns>
		public static Position GetRandomPosition(this TileMap tileMap) => tileMap.GetRandomPosition(Random.Shared);

		/// <summary>
		/// Returns a random <see cref="Position"/> within the bounds of <paramref name="tileMap"/>.
		/// </summary>
		/// <param name="tileMap"><see cref="TileMap"/> to return a <see cref="Position"/> from.</param>
		/// <param name="random">Instance of <see cref="Random"/> to use.</param>
		/// <returns>A random <see cref="Position"/> within the bounds of <paramref name="tileMap"/>.</returns>
		public static Position GetRandomPosition(this TileMap tileMap, Random random) => new(random.Next(tileMap.SizeX), random.Next(tileMap.SizeY));

		/// <summary>
		/// Returns a random <see cref="Tile"/> from <paramref name="tileMap"/>.
		/// May return <see langword="null"/> if <paramref name="tileMap"/> contains <see langword="null"/> tiles.
		/// Uses the <see cref="Random.Shared"/> instance.
		/// </summary>
		/// <param name="tileMap"><see cref="TileMap"/> to return a <see cref="Tile"/> from.</param>
		/// <returns>A random <see cref="Tile"/> from <paramref name="tileMap"/>.</returns>
		public static Tile GetRandomTile(this TileMap tileMap) => tileMap.GetRandomTile(Random.Shared);

		/// <summary>
		/// Returns a random <see cref="Tile"/> from <paramref name="tileMap"/>.
		/// May return <see langword="null"/> if <paramref name="tileMap"/> contains <see langword="null"/> tiles.
		/// </summary>
		/// <param name="tileMap"><see cref="TileMap"/> to return a <see cref="Tile"/> from.</param>
		/// <param name="random">Instance of <see cref="Random"/> to use.</param>
		/// <returns>A random <see cref="Tile"/> from <paramref name="tileMap"/>.</returns>
		public static Tile GetRandomTile(this TileMap tileMap, Random random) => tileMap[tileMap.GetRandomPosition(random)];

		/// <summary>
		/// Tries to return <paramref name="tile"/>'s child object as <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Type to cast to.</typeparam>
		/// <param name="tile">Tile to check.</param>
		/// <param name="obj">Child object as <typeparamref name="T"/>.</param>
		/// <returns>If the cast was successful.</returns>
		public static bool TryGetObject<T>(this Tile? tile, out T? obj) where T : TileObject
		{
			if (tile != null && tile.HasObject && tile.Object is T child)
			{
				obj = child;
				return true;
			}
			obj = null;
			return false;
		}

		/// <summary>
		/// Returns an <see cref="IEnumerable"/> with no <see langword="null"/> elements.
		/// </summary>
		/// <typeparam name="T">Type of enumerable.</typeparam>
		/// <param name="source">Source to skip nulls for.</param>
		/// <returns>An <see cref="IEnumerable"/> with no <see langword="null"/> elements.</returns>
		public static IEnumerable<T> SkipNull<T>(this IEnumerable<T> source) => source.Where(s => s != null);

		/// <summary>
		/// Returns an <see cref="IEnumerable"/> of all tiles in <paramref name="tileMap"/> based on <paramref name="positions"/>.
		/// </summary>
		/// <param name="positions">Positions to index.</param>
		/// <param name="tileMap"><see cref="TileMap"/> to query tiles form.</param>
		/// <returns>An <see cref="IEnumerable"/> of indexed tiles.</returns>
		public static IEnumerable<Tile?> ToTiles(this IEnumerable<Position> positions, TileMap tileMap) => positions.Select(p => tileMap[p]);

		/// <summary>
		/// Return a <see cref="Position"/> in the horizontally-flipped position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to flip.</param>
		/// <param name="pivot">Pivot to flip around.</param>
		/// <returns><see cref="Tile"/> with flipped position.</returns>
		public static Position FlipX(this Position source, Position pivot) => new(Flip(source.x, pivot.x), source.y);

		/// <summary>
		/// Return a <see cref="Position"/> in the vertically-flipped position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to flip.</param>
		/// <param name="pivot">Pivot to flip around.</param>
		/// <returns><see cref="Tile"/> with flipped position.</returns>
		public static Position FlipY(this Position source, Position pivot) => new(source.x, Flip(source.y, pivot.y));

		/// <summary>
		/// Return a <see cref="Position"/> in both the horizontally and vertically flipped position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to flip.</param>
		/// <param name="pivot">Pivot to flip around.</param>
		/// <returns><see cref="Tile"/> with flipped position.</returns>
		public static Position FlipXY(this Position source, Position pivot) => source.FlipX(pivot).FlipY(pivot);

		private static int Flip(int source, int pivot) => pivot + pivot - source;

		/// <summary>
		/// Return a <see cref="Position"/> in the horizontally-mirrored position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to generate a mirror for.</param>
		/// <param name="pivot">Pivot to mirror around.</param>
		/// <returns><see cref="IEnumerable"/> with both original and mirrored positions.</returns>
		public static IEnumerable<Position> MirrorX(this Position source, Position pivot)
		{
			yield return source;
			var mirrored = source.FlipX(pivot);
			yield return mirrored;
		}

		/// <summary>
		/// Return a <see cref="Position"/> in the vertically-mirrored position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to generate a mirror for.</param>
		/// <param name="pivot">Pivot to mirror around.</param>
		/// <returns><see cref="IEnumerable"/> with both original and mirrored positions.</returns>
		public static IEnumerable<Position> MirrorY(this Position source, Position pivot)
		{
			yield return source;
			var mirrored = source.FlipY(pivot);
			yield return mirrored;
		}

		/// <summary>
		/// Return 4 positions in both the vertically and horizontally mirrored position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to generate a mirror for.</param>
		/// <param name="pivot">Pivot to mirror around.</param>
		/// <returns><see cref="IEnumerable"/> with both original and mirrored positions.</returns>
		public static IEnumerable<Position> MirrorXY(this Position source, Position pivot)
		{
			return source.MirrorX(pivot).MirrorY(pivot);
		}

		/// <summary>
		/// For each element in <paramref name="source"/>, also return a <see cref="Position"/> in the horizontally-mirrored position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to generate a mirror for.</param>
		/// <param name="pivot">Pivot to mirror around.</param>
		/// <returns><see cref="IEnumerable"/> with both original and mirrored positions.</returns>
		public static IEnumerable<Position> MirrorX(this IEnumerable<Position> source, Position pivot)
		{
			foreach (var pos1 in source)
			{
				foreach (var pos2 in pos1.MirrorX(pivot))
					yield return pos2;
			}
		}

		/// <summary>
		/// For each element in <paramref name="source"/>, also return a <see cref="Position"/> in the vertically-mirrored position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to generate a mirror for.</param>
		/// <param name="pivot">Pivot to mirror around.</param>
		/// <returns><see cref="IEnumerable"/> with both original and mirrored positions.</returns>
		public static IEnumerable<Position> MirrorY(this IEnumerable<Position> source, Position pivot)
		{
			foreach (var pos1 in source)
			{
				foreach (var pos2 in pos1.MirrorY(pivot))
					yield return pos2;
			}
		}

		/// <summary>
		/// For each element in <paramref name="source"/>, also return 3 <see cref="Position"/> in both the vertically and horizontally mirrored position around <paramref name="pivot"/>'s position.
		/// </summary>
		/// <param name="source">Source to generate a mirror for.</param>
		/// <param name="pivot">Pivot to mirror around.</param>
		/// <returns><see cref="IEnumerable"/> with both original and mirrored positions.</returns>
		public static IEnumerable<Position> MirrorXY(this IEnumerable<Position> source, Position pivot) => source.MirrorX(pivot).MirrorY(pivot);

		/// <param name="origin">Starting position.</param>
		/// <param name="clockwise">Whether to iterate clockwise.</param>
		/// <returns>Returns spiral enumerable of all positions starting from <paramref name="origin"/>.</returns>
		public static IEnumerable<Position> Spiral(this Position origin, bool clockwise = true) => Spiral(origin, new(-1, 0), clockwise);

		/// <param name="origin">Starting position.</param>
		/// <param name="startDirection">Direction to start with.</param>
		/// <param name="clockwise">Whether to iterate clockwise.</param>
		/// <returns>Returns a clockwise spiral enumerable of all positions starting from <paramref name="origin"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">When <paramref name="startDirection"/> has a Taxicab magnitude different than 1.</exception>
		public static IEnumerable<Position> Spiral(this Position origin, Position startDirection, bool clockwise = true) => SpiralInternal(origin, startDirection, clockwise ? RotateCW : RotateCCW);

		private static IEnumerable<Position> SpiralInternal(Position position, Position direction, Func<Position, Position> rotateFunc)
		{
			if (direction.MagnitudeTaxicab != 1)
				throw new ArgumentOutOfRangeException("Argument direction must have a Taxicab magnitude of exactly 1. I.E be either (1, 0), (-1, 0), (0, 1), or (0, -1).");
			for (int chain = 1; ; chain++)
			{
				for (int repetitions = 0; repetitions < 2; repetitions++)
				{
					for (int movements = 0; movements < chain; movements++)
					{
						position += direction;
						yield return position;
					}
					direction = rotateFunc(direction);
				}
			}
		}

		private static Position RotateCW(Position position) => new(-position.y, position.x);

		private static Position RotateCCW(Position position) => new(position.y, -position.x);

		/// <summary>
		/// Filters an <see cref="IPositionable"/> enumerable to only include 1 instance every <paramref name="offset"/> iterations.
		/// </summary>
		/// <param name="source">Source enumerable.</param>
		/// <param name="every">Iteration count.</param>
		/// <param name="offset">Start at this index.</param>
		/// <returns>A filtered enumerable.</returns>
		public static IEnumerable<IPositionable> TakeOneEvery(this IEnumerable<IPositionable> source, uint every, uint offset = 0) => source.Where(s => FilterEvery(s, every, offset));

		/// <summary>
		/// Filters an <see cref="IPositionable"/> enumerable to only include 1 instance every other iteration.
		/// So that it matches a checkerboard pattern.
		/// </summary>
		/// <param name="source">Source enumerable.</param>
		/// <param name="offset">Start at this index.</param>
		/// <returns>A checkerboard-filtered enumerable.</returns>
		public static IEnumerable<IPositionable> Checkerboard(this IEnumerable<IPositionable> source, uint offset = 0) => source.TakeOneEvery(2, offset);

		private static bool FilterEvery(IPositionable source, uint every, uint offset = 0) => (source.Position.x + source.Position.y + offset) % every == 0;

		/// <summary>
		/// Trims all whitespace and makes all characters lowercase.
		/// </summary>
		/// <param name="str">Source <see langword="string"/>.</param>
		/// <returns>Simplified <see langword="string"/>.</returns>
		public static string Simplify(this string str) => str.Trim().ToLower();

		/// <summary>
		/// Removes all spaces from <paramref name="str"/>.
		/// </summary>
		/// <param name="str">Source <see langword="string"/>.</param>
		/// <returns><paramref name="str"/> with no spaces.</returns>
		public static string RemoveSpaces(this string str) => Regex.Replace(str, @"\s+", "");

		/// <summary>
		/// Indicates whether the specified <see cref="Array"/> is <see langword="null"/> or is empty (Has Length of 0).
		/// </summary>
		/// <param name="array">Source <see cref="Array"/>.</param>
		/// <returns>Whether <paramref name="array"/> is <see langword="null"/> or empty.</returns>
		public static bool IsNullOrEmpty(this Array? array) => array == null || array.Length <= 0;
	}
}
