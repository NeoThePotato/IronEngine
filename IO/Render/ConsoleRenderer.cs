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
		private FrameBuffer _frameBuffer;

		private FrameBuffer FrameBuffer {
			get => _frameBuffer;
			set => _frameBuffer = value;
		}
		private StringBuilder StringBuffer
		{ get; set; }
		private Renderer ChildRenderer
		{ get; set; }
		private int BufferSizeJ
		{ get => FrameBuffer.SizeJ; }
		private int BufferSizeI
		{ get => FrameBuffer.SizeI; }
		private (int, int) BufferSize
		{ get => (BufferSizeJ, BufferSizeI); }
		private int BufferLength
		{ get => BufferSizeJ * BufferSizeI; }
		public override int SizeJ
		{ get => ChildRenderer.SizeJ; }
		public override int SizeI
		{ get => ChildRenderer.SizeI; }
		public (int, int) Size
		{ get => (SizeJ, SizeI); }

		public ConsoleRenderer(Renderer childRenderer)
		{
			ChildRenderer = childRenderer;
			UpdateFrameBufferSize();
			UpdateStringBufferCapacity();
			AdjustConsoleBufferSize();
		}

		public void RenderFrame()
		{
			UpdateFrameBuffer();
			UpdateStringBuffer();
			AdjustConsoleWindow(); // TODO Find a way to fix the issue causing this to not render the first line
			Write(StringBuffer);
		}

		public override void Render(ref FrameBuffer buffer)
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
			ParseFrameBufferToStringBuffer();
		}

		private void ParseFrameBufferToStringBuffer()
		{
			StringBuffer.Clear();
			byte previousFGColor = FrameBuffer.Foreground[0, 0];
			byte previousBGColor = FrameBuffer.Background[0, 0];

			for (int j = 0; j < BufferSizeJ; j++)
			{
				for (int i = 0; i < BufferSizeI; i++)
				{
					(char currentChar, byte currentFGColor, byte currentBGColor) = FrameBuffer[j, i];

					if (currentFGColor != previousFGColor)
						StringBuffer.Append($"\x1b[38;5;{(int)currentFGColor}m");

					if (currentBGColor != previousBGColor)
						StringBuffer.Append($"\x1b[48;5;{(int)currentBGColor}m");

					StringBuffer.Append(currentChar != 0 ? currentChar : ' ');
					previousFGColor = currentFGColor;
					previousBGColor = currentBGColor;
				}
				StringBuffer.Append('\n');
			}
		}

		#region BUFFER_SIZE_MANIPULATION
		private void ValidateFrameBufferSize()
		{
			if (BufferSize != Size)
				UpdateFrameBufferSize();
		}

		private void UpdateFrameBufferSize()
		{
			(int sizeJ, int sizeI) = Size;
			UpdateFrameBufferSize(sizeJ, sizeI);
		}

		private void UpdateFrameBufferSize(int sizeJ, int sizeI)
		{
			FrameBuffer = new FrameBuffer(sizeJ, sizeI);
		}

		private void ValidateStringBufferCapacity()
		{
			if (StringBuffer == null || StringBuffer.Capacity < BufferLength)
				UpdateStringBufferCapacity();
		}

		private void UpdateStringBufferCapacity()
		{
			UpdateStringBufferCapacity(BufferLength+BufferSizeJ);
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
			int desiredWindowWidth = Math.Min(LargestWindowWidth, BufferSizeI);
			int desiredWindowHeight = Math.Min(LargestWindowHeight, BufferSizeJ);

			if (WindowWidth != desiredWindowWidth || WindowHeight != desiredWindowHeight)
				AdjustConsoleWindowSize(desiredWindowWidth, desiredWindowHeight);
		}

		[SupportedOSPlatform("windows")]
		private static void AdjustConsoleWindowSize(int sizeI, int sizeJ)
		{
			SetWindowSize(sizeI, sizeJ);
		}

		[SupportedOSPlatform("windows")]
		private void AdjustConsoleBufferSize()
		{
			int desiredBufferWidth = Math.Min(WindowWidth, BufferSizeI);
			int desiredBufferHeight = Math.Min(WindowHeight, BufferSizeJ);

			if (BufferWidth != desiredBufferWidth || BufferHeight != desiredBufferHeight)
				AdjustConsoleBufferSize(desiredBufferWidth, desiredBufferHeight);
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
