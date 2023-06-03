using Game;
using System.Text;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private char[,] _bufferCache;
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

		private void UpdateBufferCache()
		{
			foreach (var element in Elements)
			{
				if (element.enabled)
					CopyFrom(element);
			}
		}
	}
}
