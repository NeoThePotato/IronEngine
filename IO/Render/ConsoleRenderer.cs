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
		private char[,] _frameBuffer;

		private char[,] FrameBuffer {
			get => _frameBuffer;
			set => _frameBuffer = value;
		}
		private StringBuilder StringBuffer
		{ get; set; }
		private Renderer ChildRenderer
		{ get; set; }
		private int BufferSizeJ
		{ get => GetSizeJ(FrameBuffer); }
		private int BufferSizeI
		{ get => GetSizeI(FrameBuffer); }
		private (int, int) BufferSize
		{ get => (BufferSizeJ, BufferSizeI); }
		private int BufferLength
		{ get => BufferSizeJ * BufferSizeI; }
		public override int SizeJ
		{ get => Size.Item1; }
		public override int SizeI
		{ get => Size.Item2; }
		public (int, int) Size
		{ get => ChildRenderer.RequiredBufferSize(); }

		public ConsoleRenderer(Renderer childRenderer)
		{
			ChildRenderer = childRenderer;
			UpdateFrameBufferSize();
			ValidateStringBufferCapacity();
			AdjustConsoleBufferSize();
		}

		public void RenderFrame()
		{
			UpdateFrameBuffer();
			UpdateStringBuffer();
			AdjustConsoleWindow(); // TODO Find a way to fix the issue causing this to not render the first line
			Write(StringBuffer);
		}

		public override void Render(ref char[,] buffer)
		{
			ChildRenderer.Render(ref buffer);
		}

		private void UpdateFrameBuffer()
		{
			ValidateFrameBufferSize();
			Render(ref _frameBuffer);
		}

		private void UpdateStringBuffer()
		{
			ValidateStringBufferCapacity();
			StringBuffer.Clear();

			for (int j = 0; j < BufferSizeJ; j++)
			{
				for (int i = 0; i < BufferSizeI; i++)
				{
					char c = FrameBuffer[j, i];
					StringBuffer.Append(c == 0 ? ' ' : c);
				}
				StringBuffer.Append('\n');
			}
		}

		#region BUFFER_SIZE_MANIPULATION
		private void ValidateFrameBufferSize()
		{
			if (FrameBuffer == null || BufferSize != Size)
				UpdateFrameBufferSize();
		}

		private void UpdateFrameBufferSize()
		{
			(int sizeJ, int sizeI) = Size;
			UpdateFrameBufferSize(sizeJ, sizeI);
		}

		private void UpdateFrameBufferSize(int sizeJ, int sizeI)
		{
			(int newSizeJ, int newSizeI) = Size;
			FrameBuffer = new char[newSizeJ, newSizeI];
		}

		private void ValidateStringBufferCapacity()
		{
			if (StringBuffer == null || StringBuffer.Capacity < BufferLength)
				UpdateStringBufferCapacity();
		}

		private void UpdateStringBufferCapacity()
		{
			UpdateStringBufferCapacity(BufferLength);
		}

		private void UpdateStringBufferCapacity(int capacity)
		{
			if (StringBuffer != null)
				StringBuffer.Capacity = capacity;
			else
				StringBuffer = new StringBuilder(capacity);
		}
		#endregion

		#region CONSOLE_WINDOW_MANIPULATION
		[SupportedOSPlatform("windows")]
		private void AdjustConsoleWindow()
		{
			CursorVisible = false;
			AdjustConsoleWindowSize();
			SetCursorPosition(0, 0);
			AdjustConsoleBufferSize();
			SetWindowPosition(0, 0);
		}

		[SupportedOSPlatform("windows")]
		private void AdjustConsoleWindowSize()
		{
			if (WindowWidth != BufferSizeI || WindowHeight != BufferSizeJ)
				AdjustConsoleWindowSize(BufferSizeI, BufferSizeJ);
		}

		[SupportedOSPlatform("windows")]
		private static void AdjustConsoleWindowSize(int sizeI, int sizeJ)
		{
			SetWindowSize(sizeI, sizeJ);
		}

		[SupportedOSPlatform("windows")]
		private void AdjustConsoleBufferSize()
		{
			if (BufferWidth != BufferSizeI || BufferHeight != BufferSizeJ)
				AdjustConsoleBufferSize(BufferSizeI, BufferSizeJ);
		}
		
		[SupportedOSPlatform("windows")]
		private static void AdjustConsoleBufferSize(int sizeI, int sizeJ)
		{
			if (sizeI < WindowWidth || sizeJ < WindowHeight || sizeI < CursorLeft || sizeJ < CursorTop)
			{
				AdjustConsoleWindowSize(sizeI, sizeJ);
				SetCursorPosition(0, 0);
			}
			SetBufferSize(sizeI, sizeJ);
		}
		#endregion
	}
}
