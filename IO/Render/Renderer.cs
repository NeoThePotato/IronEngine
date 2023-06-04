using System.Drawing;

namespace IO.Render
{
	abstract class Renderer
	{
		public abstract int SizeJ
		{ get; }
		public abstract int SizeI
		{ get; }
		public int OffsetJ
		{ get; private set; }
		public int OffsetI
		{ get; private set; }

		public Renderer(int offsetJ = 0, int offsetI = 0)
		{
			OffsetJ = offsetJ;
			OffsetI = offsetI;
		}

		public abstract void Render(ref char[,] buffer);

		public static void CopyFrom(char[,] source, ref char[,] destination, int offsetJ, int offsetI)
		{
			for (int j = 0; j < source.GetLength(0); j++) // Row iteration
			{
				for (int i = 0; i < source.GetLength(1); i++) // Char iteration
				{
					destination[j + offsetJ, i + offsetI] = source[j, i];
				}
			}
		}

		public (int, int) RequiredBufferSize()
		{
			return (OffsetJ + SizeJ, OffsetI + SizeI);
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

	struct FrameBuffer
	{
		private OffsetBuffer<char>? _charFrame;
		private OffsetBuffer<ConsoleColor>? _foregroundColorFrame;
		private OffsetBuffer<ConsoleColor>? _backgroundColorFrame;
		private readonly int _sizeJ;
        private readonly int _sizeI;
		private readonly int _offsetJ;
        private readonly int _offsetI;

		public OffsetBuffer<char>? Char
		{ get => _charFrame; set => _charFrame = value; }
		public OffsetBuffer<ConsoleColor>? Foreground
		{ get => _foregroundColorFrame; set => _foregroundColorFrame = value; }
		public OffsetBuffer<ConsoleColor>? Background
		{ get => _backgroundColorFrame; set => _backgroundColorFrame = value; }
		public int SizeJ
		{ get => _sizeJ; }
		public int SizeI
		{ get => _sizeI; }
		public int OffsetJ
		{ get => _offsetJ; }
		public int OffsetI
		{ get => _offsetI; }

		public FrameBuffer(int sizeJ, int sizeI, int offsetJ = 0, int offsetI = 0, BufferType bufferType = BufferType.Full)
		{
			_sizeJ = sizeJ;
			_sizeI = sizeI;
			_offsetJ = offsetJ;
			_offsetI = offsetI;
			_charFrame = bufferType.HasFlag(BufferType.Character) ? new OffsetBuffer<char>(sizeJ, sizeI, offsetJ, offsetI) : null;
			_foregroundColorFrame = bufferType.HasFlag(BufferType.Foreground) ? new OffsetBuffer<ConsoleColor>(sizeJ, sizeI, offsetJ, offsetI) : null;
			_backgroundColorFrame = bufferType.HasFlag(BufferType.Background) ? new OffsetBuffer<ConsoleColor>(sizeJ, sizeI, offsetJ, offsetI) : null;
		}

		public FrameBuffer(FrameBuffer other, int offsetJ = 0, int offsetI = 0)
		{
            _sizeJ = other.SizeJ;
            _sizeI = other.SizeI;
            _offsetJ = other.OffsetJ + offsetJ;
            _offsetI = other.OffsetI + offsetI;
			_charFrame = other._charFrame != null ? new OffsetBuffer<char>(other._charFrame.Value, offsetJ, offsetI) : null;
			_foregroundColorFrame = other._foregroundColorFrame != null ? new OffsetBuffer<ConsoleColor>(other._foregroundColorFrame.Value, offsetJ, offsetI) : null;
			_backgroundColorFrame = other._backgroundColorFrame != null ? new OffsetBuffer<ConsoleColor>(other._backgroundColorFrame.Value, offsetJ, offsetI) : null;
		}

		[Flags]
		public enum BufferType
		{
			Character = 1,
			Foreground = 2,
			Background = 4,
			Full = 8
		}

		public struct OffsetBuffer<T>
		{
			private T[,] _buffer;

            public int SizeJ
            { get => _buffer.GetLength(0); }
            public int SizeI
            { get => _buffer.GetLength(1); }
            public int OffsetJ
			{ get; private set; }
			public int OffsetI
			{ get; private set; }


			public OffsetBuffer(int sizeJ, int sizeI, int offsetJ = 0, int offsetI = 0)
			{
				_buffer = new T[sizeJ, sizeI];
				OffsetJ = offsetJ;
				OffsetI = offsetI;
			}

			public OffsetBuffer(OffsetBuffer<T> other, int offsetJ = 0, int offsetI = 0)
			{
				_buffer = other._buffer;
				OffsetJ = other.OffsetJ + offsetJ;
				OffsetI = other.OffsetI + offsetI;
			}

			public T this[int j, int i]
			{
				get { return _buffer[j + OffsetJ, i + OffsetI]; }
				set { _buffer[j + OffsetJ, i + OffsetI] = value; }
			}
		}
	}
}
