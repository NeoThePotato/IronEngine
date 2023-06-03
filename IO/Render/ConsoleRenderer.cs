using System.Runtime.Versioning;
using System.Text;
using static System.Console;

namespace IO.Render
{

	/// <summary>
	/// Handles rendering of elements into the console
	/// </summary>
	class ConsoleRenderer : Renderer
	{
		private char[,] _bufferCache;
		private StringBuilder _bufferString;

		private char[,] BufferCache {
			get => _bufferCache;
			set => _bufferCache = value;
		}
		private StringBuilder BufferString
		{
			get => _bufferString;
			set => _bufferString = value;
		}
		private int SizeJ
		{ get => GetSizeJ(BufferCache); }
		private int SizeI
		{ get => GetSizeI(BufferCache); }

		public ConsoleRenderer(int elementsCount, int dimJ, int dimI)
		{
			BufferCache = new char[dimJ, dimI];
			BufferString = new StringBuilder(SizeJ*SizeI);
			Elements = new List<Element>(elementsCount);
			AdjustBufferSize();
		}

		public void RenderFrame()
		{
			UpdateBufferCache();
			UpdateBufferString();
			AdjustConsole(); // TODO Find a way to fix the issue causing this to not render the first line
			Write(BufferString);
		}

		private void UpdateBufferCache()
		{
			foreach (var child in Children)
			{
				if (child.Enabled)
					CopyFrom(child);
			}
		}

		private void UpdateBufferString()
		{
			BufferString.Clear();

			for (int j = 0; j < SizeJ; j++)
			{
				for (int i = 0; i < SizeI; i++)
				{
					char c = BufferCache[j, i];
					BufferString.Append(c == 0 ? ' ' : c);
				}
				BufferString.Append('\n');
			}
		}

		[SupportedOSPlatform("windows")]
		private void AdjustConsole()
		{
			CursorVisible = false;
			AdjustWindowSize();
			SetCursorPosition(0, 0);
			AdjustBufferSize();
			SetWindowPosition(0, 0);
		}

		[SupportedOSPlatform("windows")]
		private void AdjustWindowSize()
		{
			if (WindowWidth != SizeI || WindowHeight != SizeJ)
				AdjustWindowSize(SizeI, SizeJ);
		}

		[SupportedOSPlatform("windows")]
		private static void AdjustWindowSize(int sizeI, int sizeJ)
		{
			SetWindowSize(sizeI, sizeJ);
		}

		[SupportedOSPlatform("windows")]
		private void AdjustBufferSize()
		{
			if (BufferWidth != SizeI || BufferHeight != SizeJ)
				AdjustBufferSize(SizeI, SizeJ);
		}
		
		[SupportedOSPlatform("windows")]
		private static void AdjustBufferSize(int sizeI, int sizeJ)
		{
			if (sizeI < WindowWidth || sizeJ < WindowHeight || sizeI < CursorLeft || sizeJ < CursorTop)
			{
				AdjustWindowSize(sizeI, sizeJ);
				SetCursorPosition(0, 0);
			}
			SetBufferSize(sizeI, sizeJ);
		}
	}
}
