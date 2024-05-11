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
		public const byte COLOR_WHITE = 15;
		public const byte COLOR_BLACK = 0;
		private static readonly bool IS_WINDOWS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		internal static readonly (char, byte, byte) EMPTY_CHAR = (' ', COLOR_WHITE, COLOR_BLACK);
		public static int TileSizeX = 6;
		public static int TileSizeY = 3;

		internal static FrameBuffer Buffer { get; private set; }
		private StringBuilder StringBuffer { get; set; }
		private IConsoleRenderer ChildRenderer { get; set; }
		private static int BufferSizeX => Buffer.SizeX;
		private static int BufferSizeY => Buffer.SizeY;
		private static (int, int) BufferSize => (BufferSizeX, BufferSizeY);
		private static int BufferLength => BufferSizeX * BufferSizeY;
		public int SizeX => ChildRenderer.SizeX;
		public int SizeY => ChildRenderer.SizeY;
		internal (int, int) Size => (SizeX, SizeY);

		public ConsoleRenderer(TileMap tileMap)
		{
			if (tileMap is IRenderAble renderAble && renderAble.GetRenderer() is IConsoleRenderer consoleRenderer)
				ChildRenderer = consoleRenderer;
			else
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
			return valid;
		}

		private void UpdateStringBuffer()
		{
			ParseFrameBufferToStringBuffer();
		}

		private void WriteStringToConsole()
		{
			WriteLine(StringBuffer);
		}

		private void ParseFrameBufferToStringBuffer()
		{
			StringBuffer.Clear();
			byte previousFGColor = 0;
			byte previousBGColor = 0;

			for (int y = 0; y < BufferSizeY; y++)
			{
				for (int x = 0; x < BufferSizeX; x++)
				{
					(char currentChar, byte currentFGColor, byte currentBGColor) = Buffer[x, y];

					if (currentFGColor != previousFGColor)
						StringBuffer.Append($"\x1b[38;5;{(int)currentFGColor}m");

					if (currentBGColor != previousBGColor)
						StringBuffer.Append($"\x1b[48;5;{(int)currentBGColor}m");

					StringBuffer.Append(currentChar != 0 ? currentChar : ' ');
					previousFGColor = currentFGColor;
					previousBGColor = currentBGColor;
				}
				StringBuffer.Append($"\x1b[0m\n");
				previousFGColor = 0;
				previousBGColor = 0;
			}
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
			Debug.WriteLine($"Updated ConsoleRenderer.FrameBuffer.size to {BufferSizeX}, {BufferSizeY}.");
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
			UpdateStringBufferCapacity(BufferLength + BufferSizeX);
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
		private static void PrepareConsoleWindow()
		{
			Clear();
			try
			{
				CursorVisible = false;
				SetCursorPosition(0, 0);
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
			bool invalid = WindowHeight != SizeX || WindowWidth != SizeY;

			if (invalid)
				UpdateConsoleWindowSize();

			return !invalid;
		}

		[SupportedOSPlatform("windows")]
		private void UpdateConsoleWindowSize()
		{
			UpdateConsoleWindowSize(SizeY, SizeX);
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
			return WindowHeight < SizeX || WindowHeight > LargestWindowHeight || WindowWidth < SizeY || WindowWidth > LargestWindowWidth;
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
			private readonly int _sizeX;
			private readonly int _sizeY;
			private readonly int _offsetX;
			private readonly int _offsetY;
			internal OffsetBuffer<char> Char;
			internal OffsetBuffer<byte> Foreground;
			internal OffsetBuffer<byte> Background;

			internal readonly int SizeX => _sizeX - OffsetX;
			internal readonly int SizeY => _sizeY - OffsetY;
			internal readonly int OffsetX => _offsetX;
			internal readonly int OffsetY => _offsetY;

			internal (char, byte, byte) this[int x, int y]
			{
				readonly get => (Char[x, y], Foreground[x, y], Background[x, y]);
				set
				{
					Char[x, y] = value.Item1;
					Foreground[x, y] = value.Item2;
					Background[x, y] = value.Item3;
				}
			}

			internal FrameBuffer(int sizeX, int sizeY, int offsetX = 0, int offsetY = 0)
			{
				_sizeX = sizeX;
				_sizeY = sizeY;
				_offsetX = offsetX;
				_offsetY = offsetY;
				Char = new OffsetBuffer<char>(sizeX, sizeY, offsetX, offsetY);
				Foreground = new OffsetBuffer<byte>(sizeX, sizeY, offsetX, offsetY);
				Background = new OffsetBuffer<byte>(sizeX, sizeY, offsetX, offsetY);
			}

			internal FrameBuffer(FrameBuffer other, int offsetX = 0, int offsetY = 0)
			{
				_sizeX = other._sizeX;
				_sizeY = other._sizeY;
				_offsetX = other._offsetX + offsetX;
				_offsetY = other._offsetY + offsetY;
				Char = new OffsetBuffer<char>(other.Char, offsetX, offsetY);
				Foreground = new OffsetBuffer<byte>(other.Foreground, offsetX, offsetY);
				Background = new OffsetBuffer<byte>(other.Background, offsetX, offsetY);
			}

			internal static void Copy(FrameBuffer destination, FrameBuffer source)
			{
				var sizeX = Math.ClampMax(source.SizeX, destination._sizeX);
				var sizeY = Math.ClampMax(source.SizeY, destination._sizeY);

				for (int y = 0; y < sizeY; y++)
				{
					for (int x = 0; x < sizeX; x++)
					{
						destination[x, y] = source[x, y];
					}
				}
			}

			public override readonly string ToString()
			{
				return $"X: {_offsetX}:{_sizeX}, Y: {_offsetY}:{_sizeY}";
			}

			internal struct OffsetBuffer<T>
			{
				private readonly T[,] _buffer;

				internal readonly int SizeX => _buffer.GetLength(1);
				internal readonly int SizeY => _buffer.GetLength(0);
				internal int OffsetX { get; private set; }
				internal int OffsetY { get; private set; }


				internal OffsetBuffer(int sizeX, int sizeY, int offsetX = 0, int offsetY = 0)
				{
					_buffer = new T[sizeY, sizeX];
					OffsetX = offsetX;
					OffsetY = offsetY;
				}

				internal OffsetBuffer(OffsetBuffer<T> other, int offsetX = 0, int offsetY = 0)
				{
					_buffer = other._buffer;
					OffsetX = other.OffsetX + offsetX;
					OffsetY = other.OffsetY + offsetY;
				}

				internal readonly T this[int x, int y]
				{
					get => _buffer[OffsetY + y, OffsetX + x];
					set => _buffer[OffsetY + y, OffsetX + x] = value;
				}
			}
		}
		#endregion

		#region CONSOLE_RENDERER_INTERFACE
		internal interface IConsoleRenderer : IRenderer
		{
			internal int SizeX { get; }
			internal int SizeY { get; }

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
