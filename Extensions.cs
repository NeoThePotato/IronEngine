﻿using System.Text.RegularExpressions;

namespace IronEngine
{
	public static class Extensions
	{
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
