using Game;
using System.Text;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private char[,] _bufferCache;

		private char[,] BufferCache
		{
			get => _bufferCache;
			set => _bufferCache = value;
		}
		private List<Element> Elements
		{ get; set; }
		private int SizeJ
		{ get => GetSizeJ(BufferCache); }
		private int SizeI
		{ get => GetSizeI(BufferCache); }

		public GameManagerRenderer(GameManager gameManager)
		{
			BufferCache = new char[dimJ, dimI];
			BufferString = new StringBuilder(SizeJ * SizeI);
			Elements = new List<Element>(elementsCount);
			AdjustBufferSize();
		}

		public void AddElement(Element element)
		{
			Elements.Add(element);
		}

		public void RenderFrame()
		{
			UpdateBufferCache();
			UpdateBufferString();
			AdjustConsole(); // TODO Find a way to fix the issue causing this to not render the first line
			Write(BufferString);
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

		private void UpdateBufferCache()
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
}
