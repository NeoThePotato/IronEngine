using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
		private static readonly bool IS_WINDOWS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		private static readonly int MAX_RES_WIDTH = 200; // TODO Make this an option
		private static readonly int MAX_RES_HEIGHT = 60;

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
		{ get => MaxSizeJ; }
		public override int SizeI
		{ get => MaxSizeI; }
		public (int, int) Size
		{ get => (SizeJ, SizeI); }
		public override int MaxSizeJ
		{ get => new[] { MAX_RES_HEIGHT, ChildRenderer.MaxSizeJ, WindowHeight}.Min(); }
		public override int MaxSizeI
		{ get => new[] { MAX_RES_WIDTH, ChildRenderer.MaxSizeI, WindowWidth}.Min(); }
		public override int MinSizeJ
		{ get => ChildRenderer.MinSizeJ; }
		public override int MinSizeI
		{ get => ChildRenderer.MinSizeI; }
		public ulong CurrentTick
		{ get; private set; }

		public ConsoleRenderer(Renderer childRenderer)
		{
			ChildRenderer = childRenderer;
			UpdateFrameBufferSize();
			UpdateStringBufferCapacity();

			if (IS_WINDOWS)
				Utility.EnableVirtualTerminalProcessing();
		}

		public void RenderFrame(ulong currentTick)
		{
			CurrentTick = currentTick;
			Validate();
			UpdateFrameBuffer();
			UpdateStringBuffer();
			PrepareConsoleWindow();
			WriteStringToConsole();
		}

		public override void Render(FrameBuffer buffer)
		{
			ChildRenderer.Render(buffer);
		}

		public override void Validate()
		{
			ChildRenderer.Validate();
			ValidateFrameBufferSize();
			ValidateStringBufferCapacity();
			ValidateConsoleWindow();
		}

		private void UpdateFrameBuffer()
		{
			Render(_frameBuffer);
		}

		private void UpdateStringBuffer()
		{
			ParseFrameBufferToStringBuffer();
		}

		private void WriteStringToConsole()
		{
			Write(StringBuffer);
		}

		private void ParseFrameBufferToStringBuffer()
		{
			StringBuffer.Clear();
			byte previousFGColor = 0;
			byte previousBGColor = 0;

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
			StringBuffer.Remove(StringBuffer.Length-1, 1);
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
			Debug.WriteLine($"Updated ConsoleRenderer.FrameBuffer.size to {BufferSizeJ}, {BufferSizeI}.");
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
			{
				StringBuffer.Capacity = capacity;
				Debug.WriteLine($"Updated ConsoleRenderer.StringBuffer.capacity to {StringBuffer.Capacity}.");
			}
			else
			{
				StringBuffer = new StringBuilder(capacity);
			}
		}
		#endregion

		#region CONSOLE_WINDOW_MANIPULATION
		[SupportedOSPlatform("windows")]
		private void ValidateConsoleWindow()
		{
			ValidateConsoleWindowSize();
			ValidateConsoleBufferSize();
		}

		private void PrepareConsoleWindow()
		{
			try
			{
				CursorVisible = false;
				SetCursorPosition(0, 0);
				SetWindowPosition(0, 0);
			}
			catch (ArgumentOutOfRangeException)
			{

			}
		}

		[SupportedOSPlatform("windows")]
		private void ValidateConsoleWindowSize()
		{
			if (ConsoleWindowSizeOutOfBounds())
				UpdateConsoleWindowSize();
		}

		[SupportedOSPlatform("windows")]
		private void UpdateConsoleWindowSize()
		{
			int width = Utility.ClampRange(WindowWidth, MinSizeI, MaxSizeI);
			int height = Utility.ClampRange(WindowHeight, MinSizeJ, MaxSizeJ);
			UpdateConsoleWindowSize(width, height);
		}

		[SupportedOSPlatform("windows")]
		private static void UpdateConsoleWindowSize(int sizeI, int sizeJ)
		{
			try
			{
				SetWindowSize(sizeI, sizeJ);
				Debug.WriteLine($"Updated Console.Window's size to {WindowHeight}, {WindowWidth}.");
			}
			catch
			{
				Debug.WriteLine($"Failed to set Console.Window's size to {WindowHeight}, {WindowWidth}.");
			}
		}

		[SupportedOSPlatform("windows")]
		private bool ConsoleWindowSizeOutOfBounds()
		{
			return WindowHeight < MinSizeJ || WindowHeight > LargestWindowHeight || WindowWidth < MinSizeI || WindowWidth > LargestWindowWidth;
		}

		[SupportedOSPlatform("windows")]
		private void ValidateConsoleBufferSize()
		{
			if (BufferWidth != WindowWidth || BufferHeight != WindowHeight)
				UpdateConsoleBufferSize();
		}

		[SupportedOSPlatform("windows")]
		private void UpdateConsoleBufferSize()
		{
			UpdateConsoleBufferSize(WindowWidth, WindowHeight);
		}

		[SupportedOSPlatform("windows")]
		private static void UpdateConsoleBufferSize(int sizeI, int sizeJ)
		{
			if (sizeI < CursorLeft || sizeJ < CursorTop)
				SetCursorPosition(0, 0);

			if (sizeI < WindowWidth || sizeJ < WindowHeight)
				UpdateConsoleWindowSize(sizeI, sizeJ);

			try
			{
				SetBufferSize(sizeI, sizeJ);
				Debug.WriteLine($"Updated Console.Buffer's size to {BufferHeight}, {BufferWidth}.");
			}
			catch
			{
				Debug.WriteLine($"Failed to set Console.Buffer's size to {BufferHeight}, {BufferWidth}.");
			}
		}
		#endregion
	}
}
