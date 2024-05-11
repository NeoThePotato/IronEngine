using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using static System.Console;
using IronEngine.IO;

namespace IronEngine.DefaultRenderer
{
	/// <summary>
	/// Handles rendering of elements into the console
	/// </summary>
	public class ConsoleRenderer : ConsoleRenderer.IConsoleRenderer
	{
		internal const byte COLOR_WHITE = 15;
		internal const byte COLOR_BLACK = 0;
		private static readonly bool IS_WINDOWS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		internal static readonly (char, byte, byte) EMPTY_CHAR = (' ', COLOR_WHITE, COLOR_BLACK);

		internal static FrameBuffer Buffer { get; private set; }
		private StringBuilder StringBuffer { get; set; }
		private TileMapRenderer ChildRenderer { get; set; }
		private static int BufferSizeJ => Buffer.SizeJ;
		private static int BufferSizeI => Buffer.SizeI;
		private static (int, int) BufferSize => (BufferSizeJ, BufferSizeI);
		private static int BufferLength => BufferSizeJ * BufferSizeI;
		public int SizeJ => ChildRenderer.SizeJ;
		public int SizeI => ChildRenderer.SizeI;
		internal (int, int) Size => (SizeJ, SizeI);

		public ConsoleRenderer(TileMap tileMap)
		{
			ChildRenderer = new TileMapRenderer(tileMap);
			UpdateFrameBufferSize();
			UpdateStringBufferCapacity();
			UpdateFrame();
			if (IS_WINDOWS)
				EnableVirtualTerminalProcessing();
		}

		#region RENDERING_LOGIC

		public void UpdateFrame()
		{
			Validate();
			ChildRenderer.UpdateFrame();
			UpdateStringBuffer();
			PrepareConsoleWindow();
			WriteStringToConsole();
		}

		private bool Validate()
		{
			bool valid = true;
			valid &= ValidateFrameBufferSize();
			ValidateStringBufferCapacity();
			valid &= ValidateConsoleWindow();
			return valid;
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
					(char currentChar, byte currentFGColor, byte currentBGColor) = Buffer[j, i];

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
			StringBuffer.Remove(StringBuffer.Length - 1, 1);
		}

		#endregion

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
			Buffer = new FrameBuffer(sizeJ, sizeI);
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
			UpdateStringBufferCapacity(BufferLength + BufferSizeJ);
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

		private static void PrepareConsoleWindow()
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
			catch (IOException)
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
		private static bool ValidateConsoleBufferSize()
		{
			bool invalid = BufferWidth != WindowWidth || BufferHeight != WindowHeight;

			if (invalid)
				UpdateConsoleBufferSize();

			return !invalid;
		}

		[SupportedOSPlatform("windows")]
		private static void UpdateConsoleBufferSize()
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

		[SupportedOSPlatform("windows")]
		public static void EnableVirtualTerminalProcessing()
		{
			const int STD_OUTPUT_HANDLE = -11;
			const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

			var handle = GetStdHandle(STD_OUTPUT_HANDLE);
			GetConsoleMode(handle, out uint mode);
			mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;

			try
			{
				SetConsoleMode(handle, mode);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}

		[SupportedOSPlatform("windows")]
		[DllImport("kernel32.dll")]
		static private extern nint GetStdHandle(int nStdHandle);

		[SupportedOSPlatform("windows")]
		[DllImport("kernel32.dll")]
		static private extern bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

		[SupportedOSPlatform("windows")]
		[DllImport("kernel32.dll")]
		static private extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);
		#endregion

		#region FRAME_BUFFER
		public struct FrameBuffer
		{
			private readonly int _sizeJ;
			private readonly int _sizeI;
			private readonly int _offsetJ;
			private readonly int _offsetI;
			internal OffsetBuffer<char> Char;
			internal OffsetBuffer<byte> Foreground;
			internal OffsetBuffer<byte> Background;

			internal readonly int SizeJ => _sizeJ - OffsetJ;
			internal readonly int SizeI => _sizeI - OffsetI;
			internal readonly int OffsetJ => _offsetJ;
			internal readonly int OffsetI => _offsetI;

			internal (char, byte, byte) this[int j, int i]
			{
				readonly get => (Char[j, i], Foreground[j, i], Background[j, i]);
				set
				{
					Char[j, i] = value.Item1;
					Foreground[j, i] = value.Item2;
					Background[j, i] = value.Item3;
				}
			}

			internal FrameBuffer(int sizeJ, int sizeI, int offsetJ = 0, int offsetI = 0)
			{
				_sizeJ = sizeJ;
				_sizeI = sizeI;
				_offsetJ = offsetJ;
				_offsetI = offsetI;
				Char = new OffsetBuffer<char>(sizeJ, sizeI, offsetJ, offsetI);
				Foreground = new OffsetBuffer<byte>(sizeJ, sizeI, offsetJ, offsetI);
				Background = new OffsetBuffer<byte>(sizeJ, sizeI, offsetJ, offsetI);
			}

			internal FrameBuffer(FrameBuffer other, int offsetJ = 0, int offsetI = 0)
			{
				_sizeJ = other._sizeJ;
				_sizeI = other._sizeI;
				_offsetJ = other._offsetJ + offsetJ;
				_offsetI = other._offsetI + offsetI;
				Char = new OffsetBuffer<char>(other.Char, offsetJ, offsetI);
				Foreground = new OffsetBuffer<byte>(other.Foreground, offsetJ, offsetI);
				Background = new OffsetBuffer<byte>(other.Background, offsetJ, offsetI);
			}

			internal static void Copy(FrameBuffer destination, FrameBuffer source)
			{
				var sizeJ = Math.ClampMax(source.SizeJ, destination._sizeJ);
				var sizeI = Math.ClampMax(source.SizeI, destination._sizeI);

				for (int j = 0; j < sizeJ; j++)
				{
					for (int i = 0; i < sizeI; i++)
					{
						destination[j, i] = source[j, i];
					}
				}
			}

			public override readonly string ToString()
			{
				return $"J: {_offsetJ}:{_sizeJ}, I: {_offsetI}:{_sizeI}";
			}

			internal struct OffsetBuffer<T>
			{
				private readonly T[,] _buffer;

				internal readonly int SizeJ => _buffer.GetLength(0);
				internal readonly int SizeI => _buffer.GetLength(1);
				internal int OffsetJ { get; private set; }
				internal int OffsetI { get; private set; }


				internal OffsetBuffer(int sizeJ, int sizeI, int offsetJ = 0, int offsetI = 0)
				{
					_buffer = new T[sizeJ, sizeI];
					OffsetJ = offsetJ;
					OffsetI = offsetI;
				}

				internal OffsetBuffer(OffsetBuffer<T> other, int offsetJ = 0, int offsetI = 0)
				{
					_buffer = other._buffer;
					OffsetJ = other.OffsetJ + offsetJ;
					OffsetI = other.OffsetI + offsetI;
				}

				internal readonly T this[int j, int i]
				{
					get => _buffer[OffsetJ + j, OffsetI + i];
					set => _buffer[OffsetJ + j, OffsetI + i] = value;
				}
			}
		}
		#endregion

		#region CONSOLE_RENDERER_INTERFACE
		internal interface IConsoleRenderer : IRenderer
		{
			internal int SizeJ { get; }
			internal int SizeI { get; }

			public void WriteLine(string str)
			{
				RenderText(Buffer, str);
			}

			internal static void RenderText(FrameBuffer buffer, string str, byte textColor = COLOR_WHITE)
			{
				int i = 0;
				for (; i < str.Length; i++)
				{
					buffer.Char[0, i] = str[i];
					buffer.Foreground[0, i] = textColor;
				}
			}

			internal static void RenderTextSingleLine(FrameBuffer buffer, string str, int length, byte textColor = COLOR_WHITE, byte bgColor = COLOR_BLACK)
			{
				int c = 0;

				for (; c < str.Length & c < length; c++)
					buffer[0, c] = (str[c], textColor, bgColor);

				for (; c < length; c++)
					buffer[0, c] = EMPTY_CHAR;
			}

			internal static void RenderText(FrameBuffer buffer, IEnumerable<string> str, int length, byte textColor = COLOR_WHITE, byte fgColor = COLOR_BLACK)
			{
				int j = 0;

				foreach (var line in str)
				{
					var fb = new FrameBuffer(buffer, j, 0);
					RenderTextSingleLine(fb, line, length, textColor, fgColor);
					j++;
				}
			}
		}
		#endregion
	}
}
