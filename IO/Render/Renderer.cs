namespace IO.Render
{
	abstract class Renderer
	{
		public int SizeJ
		{ get; private set; }
		public int SizeI
		{ get; private set; }
		public int OffsetJ
		{ get; private set; }
		public int OffsetI
		{ get; private set; }

		public Renderer(int offsetJ = 0, int offsetI = 0)
		{
			OffsetJ = offsetJ;
			OffsetI = offsetI;
		}

		public abstract void Render(ref char[,] buffer);

		public static void CopyFrom(char[,] source, ref char[,] destination, int offsetJ, int offsetI)
		{
			for (int j = 0; j < source.GetLength(0); j++) // Row iteration
			{
				for (int i = 0; i < source.GetLength(1); i++) // Char iteration
				{
					destination[j + offsetJ, i + offsetI] = source[j, i];
				}
			}
		}

		public static int GetSizeJ(char[,] arr)
		{
			return arr.GetLength(0);
		}

		public static int GetSizeI(char[,] arr)
		{
			return arr.GetLength(1);
		}
	}
}
