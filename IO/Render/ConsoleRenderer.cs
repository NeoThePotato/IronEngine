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

		private FrameBuffer FrameBufferCurrent
		{ get; set; }
		private FrameBuffer FrameBufferCache
		{ get; set; }
		private StringBuilder StringBuffer
		{ get; set; }
		private Renderer ChildRenderer
		{ get; set; }
		private int BufferSizeJ
		{ get => FrameBufferCurrent.SizeJ; }
		private int BufferSizeI
		{ get => FrameBufferCurrent.SizeI; }
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
		public ulong CurrentTick
		{ get; private set; }

		public ConsoleRenderer(Renderer childRenderer)
		{
			ChildRenderer = childRenderer;
			UpdateFrameBufferSize();
			UpdateStringBufferCapacity();
			Validate();
			UpdateCacheFrameBuffer();

			if (IS_WINDOWS)
				Utility.EnableVirtualTerminalProcessing();
		}

		public void RenderFrame(ulong currentTick)
		{
			CurrentTick = currentTick;
			if (!Validate())
				UpdateCacheFrameBuffer();
			UpdateCurrentFrameBuffer();
			UpdateStringBuffer();
			PrepareConsoleWindow();
			WriteStringToConsole();
		}

		public override void Render(FrameBuffer buffer)
		{
			ChildRenderer.Render(buffer);
		}

		public override void RenderToCache(FrameBuffer buffer)
		{
			ChildRenderer.RenderToCache(buffer);
		}

		public override bool Validate()
		{
			bool valid = true;

			valid = valid & ChildRenderer.Validate();
			valid = valid & ValidateFrameBufferSize();
			ValidateStringBufferCapacity();
			valid = valid & ValidateConsoleWindow();
			Debug.WriteLineIf(!valid, "ConsoleRenderer was validated");

			return valid;
		}

		private void UpdateCacheFrameBuffer()
		{
			ChildRenderer.RenderToCache(FrameBufferCache);
		}

		private void UpdateCurrentFrameBuffer()
		{
			FrameBuffer.Copy(FrameBufferCurrent, FrameBufferCache);
			Render(FrameBufferCurrent);
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
					(char currentChar, byte currentFGColor, byte currentBGColor) = FrameBufferCurrent[j, i];

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
		private bool ValidateFrameBufferSize()
		{
			bool valid = BufferSize == Size;

			if (!valid)
				UpdateFrameBufferSize();

			return valid;
		}

		private void UpdateFrameBufferSize()
		{
			(int sizeJ, int sizeI) = Size;
			UpdateFrameBufferSize(sizeJ, sizeI);
			Debug.WriteLine($"Updated ConsoleRenderer.FrameBuffer.size to {BufferSizeJ}, {BufferSizeI}.");
		}

		private void UpdateFrameBufferSize(int sizeJ, int sizeI)
		{
			FrameBufferCurrent = new FrameBuffer(sizeJ, sizeI);
			FrameBufferCache = new FrameBuffer(sizeJ, sizeI);
		}

		private bool ValidateStringBufferCapacity()
		{
			bool invalid = StringBuffer == null || StringBuffer.Capacity < BufferLength;

			if (invalid)
				UpdateStringBufferCapacity();

			return !invalid;
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
		private bool ValidateConsoleWindow()
		{
			return ValidateConsoleWindowSize() & ValidateConsoleBufferSize();
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
		private bool ValidateConsoleWindowSize()
		{
			bool invalid = WindowHeight != SizeJ || WindowWidth != SizeI;

			if (invalid)
				UpdateConsoleWindowSize();

			return !invalid;
		}

		[SupportedOSPlatform("windows")]
		private void UpdateConsoleWindowSize()
		{
			UpdateConsoleWindowSize(SizeI, SizeJ);
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
			return WindowHeight < SizeJ || WindowHeight > LargestWindowHeight || WindowWidth < SizeI || WindowWidth > LargestWindowWidth;
		}

		[SupportedOSPlatform("windows")]
		private bool ValidateConsoleBufferSize()
		{
			bool invalid = BufferWidth != WindowWidth || BufferHeight != WindowHeight;

			if (invalid)
				UpdateConsoleBufferSize();

			return !invalid;
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
