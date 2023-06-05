using System.Drawing;
using static IO.Render.FrameBuffer;

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

		public abstract void Render(ref FrameBuffer buffer);

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
		private readonly int _sizeJ;
        private readonly int _sizeI;
		private readonly int _offsetJ;
        private readonly int _offsetI;
		public OffsetBuffer<char> Char;
		public OffsetBuffer<ConsoleColor> Foreground;
		public OffsetBuffer<ConsoleColor> Background;

		public int SizeJ
		{ get => _sizeJ; }
		public int SizeI
		{ get => _sizeI; }
		public int OffsetJ
		{ get => _offsetJ; }
		public int OffsetI
		{ get => _offsetI; }

		public FrameBuffer(int sizeJ, int sizeI, int offsetJ = 0, int offsetI = 0)
		{
			_sizeJ = sizeJ;
			_sizeI = sizeI;
			_offsetJ = offsetJ;
			_offsetI = offsetI;
			Char = new OffsetBuffer<char>(sizeJ, sizeI, offsetJ, offsetI);
			Foreground = new OffsetBuffer<ConsoleColor>(sizeJ, sizeI, offsetJ, offsetI);
			Background = new OffsetBuffer<ConsoleColor>(sizeJ, sizeI, offsetJ, offsetI);
		}

		public FrameBuffer(FrameBuffer other, int offsetJ = 0, int offsetI = 0)
		{
            _sizeJ = other.SizeJ;
            _sizeI = other.SizeI;
            _offsetJ = other.OffsetJ + offsetJ;
            _offsetI = other.OffsetI + offsetI;
			Char = new OffsetBuffer<char>(other.Char, offsetJ, offsetI);
			Foreground = new OffsetBuffer<ConsoleColor>(other.Foreground, offsetJ, offsetI);
			Background = new OffsetBuffer<ConsoleColor>(other.Background, offsetJ, offsetI);
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
				get { return _buffer[OffsetJ + j, OffsetI + i]; }
				set { _buffer[OffsetJ + j, OffsetI + i] = value; }
			}
		}
	}
}
