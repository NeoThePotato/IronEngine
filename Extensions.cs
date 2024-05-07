namespace IronEngine
{
	public static class Extensions
	{
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
		public static bool TryGetObject<T>(this Tile tile, out T? obj) where T : TileObject
		{
			if (tile != null && tile.HasObject && tile.Object is T child)
			{
				obj = child;
				return true;
			}
			obj = null;
			return false;
		}
	}
}
