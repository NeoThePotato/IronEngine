using static System.Math;

public static class Math
{
	/// <summary>
	/// Swaps <paramref name="obj1"/> and <paramref name="obj2"/>.
	/// </summary>
	public static void Swap<T>(ref T obj1, ref T obj2) => (obj2, obj1) = (obj1, obj2);

	/// <summary>
	/// Swaps elements from <paramref name="list"/> at indexes <paramref name="idx1"/> and <paramref name="idx2"/>.
	/// </summary>
	public static void Swap<T>(IList<T> list, int idx1, int idx2) => (list[idx2], list[idx1]) = (list[idx1], list[idx2]);

	/// <summary>
	/// Clamps <paramref name="val"/> so it doesn't go below <paramref name="min"/>.
	/// </summary>
	public static float ClampMin(float val, float min) => Max(val, min);

	/// <summary>
	/// Clamps <paramref name="val"/> so it doesn't go above <paramref name="max"/>.
	/// </summary>
	public static float ClampMax(float val, float max) => Min(val, max);

	/// <summary>
	/// Clamps <paramref name="val"/> so it doesn't go below <paramref name="min"/> or above <paramref name="max"/>.
	/// </summary>
	public static float ClampRange(float val, float min, float max) => ClampMin(ClampMax(val, max), min);

	/// <summary>
	/// Clamps <paramref name="val"/> so it doesn't go below <paramref name="min"/>.
	/// </summary>
	public static int ClampMin(int val, int min) => Max(val, min);

	/// <summary>
	/// Clamps <paramref name="val"/> so it doesn't go above <paramref name="max"/>.
	/// </summary>
	public static int ClampMax(int val, int max) => Min(val, max);

	/// <summary>
	/// Clamps <paramref name="val"/> so it doesn't go below <paramref name="min"/> or above <paramref name="max"/>.
	/// </summary>
	public static int ClampRange(int val, int min, int max) => ClampMin(ClampMax(val, max), min);

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
}