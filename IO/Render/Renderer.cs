using System.Text;

namespace IO.Render
{

	/// <summary>
	/// Handles rendering of elements into the console
	/// </summary>
	class Renderer
	{
		private char[,] _bufferCache;

		private char[,] BufferCache {
			get => _bufferCache;
			set => _bufferCache = value;
		}
		private Element[] Elements
		{ get; set; }
		private int CurrentElementCount
		{ get; set; }
		private int SizeJ
		{ get => GetSizeJ(BufferCache); }
		private int SizeI
		{ get => GetSizeI(BufferCache); }

		public Renderer(int elementsCount, int dimJ, int dimI)
		{
			BufferCache = new char[dimJ, dimI];
			Elements = new Element[elementsCount];
			CurrentElementCount = 0;
		}

		public void AddElement(Element element)
		{
			Elements[CurrentElementCount] = element;
			CurrentElementCount++;
		}

		public void RenderFrame()
		{
			UpdateBuffer();
			Console.CursorVisible = false;
			Console.SetCursorPosition(0, 0);
			Console.Write(BufferToString());
		}

		private string BufferToString()
		{
			StringBuilder sb = new(SizeJ * SizeI);

			for (int j = 0; j < SizeJ; j++)
			{
				for (int i = 0; i < SizeI; i++)
				{
					char c = BufferCache[j, i];
					sb.Append(c == 0 ? ' ' : c);
				}
				sb.Append('\n');
			}

			return sb.ToString();
		}

		private void CopyFrom(Element element)
		{
			CopyFrom(element.renderable.Render(), ref _bufferCache, element.offsetI, element.offsetJ);
		}

		private static void CopyFrom(char[,] source, ref char[,] destination, int offsetJ, int offsetI)
		{
			for (int j = 0; j < source.GetLength(0); j++) // Row iteration
			{
				for (int i = 0; i < source.GetLength(1); i++) // Char iteration
				{
					destination[j + offsetJ, i + offsetI] = source[j, i];
				}
			}
		}

		private void UpdateBuffer()
		{
			foreach (var element in Elements)
			{
				if (element.enabled)
					CopyFrom(element);
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

	/// <summary>
	/// Contains a renderable element with info of where it should be rendered on the screen-space
	/// </summary>
	struct Element
	{
		public bool enabled;
		public int offsetJ, offsetI;
		public IRenderable renderable;
	}

}
