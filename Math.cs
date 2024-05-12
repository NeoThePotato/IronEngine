using static System.Math;

public static class Math
{
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
}
