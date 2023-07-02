namespace IO.Render
{
	abstract class Renderer
	{
		public const byte COLOR_WHITE = 15;
		public const byte COLOR_BLACK = 0;
		public static readonly (char, byte, byte) EMPTY_CHAR = (' ', COLOR_WHITE, COLOR_BLACK);
		public abstract int SizeJ
		{ get; }
		public abstract int SizeI
		{ get; }

		public abstract void Render(FrameBuffer buffer);

		public virtual void RenderToCache(FrameBuffer buffer)
		{

		}

		public virtual bool Validate()
		{
			return true;
		}

		public static void RenderText(FrameBuffer buffer, string str, byte textColor = COLOR_WHITE)
		{
			int i = 0;

			for (; i < str.Length; i++)
			{
				buffer.Char[0, i] = str[i];
				buffer.Foreground[0, i] = textColor;
			}
		}

		public static void RenderTextSingleLine(FrameBuffer buffer, string str, int length, byte textColor = COLOR_WHITE, byte bgColor = COLOR_BLACK)
		{
			int c = 0;

			for (; c < str.Length & c < length; c++)
				buffer[0, c] = (str[c], textColor, bgColor);

			for (; c < length; c++)
				buffer[0, c] = EMPTY_CHAR;
		}

		public static void RenderText(FrameBuffer buffer, IEnumerable<string> str, int length, byte textColor = COLOR_WHITE, byte fgColor = COLOR_BLACK)
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

	struct FrameBuffer
	{
		private readonly int _sizeJ;
		private readonly int _sizeI;
		private readonly int _offsetJ;
		private readonly int _offsetI;
		public OffsetBuffer<char> Char;
		public OffsetBuffer<byte> Foreground;
		public OffsetBuffer<byte> Background;

		public readonly int SizeJ
		{ get => _sizeJ - OffsetJ; }
		public readonly int SizeI
		{ get => _sizeI - OffsetI; }
		public readonly int OffsetJ
		{ get => _offsetJ; }
		public readonly int OffsetI
		{ get => _offsetI; }

		public (char, byte, byte) this[int j, int i]
		{
			readonly get
			{
				return (Char[j, i], Foreground[j, i], Background[j, i]);
			}
			set
			{
				Char[j, i] = value.Item1;
				Foreground[j, i] = value.Item2;
				Background[j, i] = value.Item3;
			}
		}

		public FrameBuffer(int sizeJ, int sizeI, int offsetJ = 0, int offsetI = 0)
		{
			_sizeJ = sizeJ;
			_sizeI = sizeI;
			_offsetJ = offsetJ;
			_offsetI = offsetI;
			Char = new OffsetBuffer<char>(sizeJ, sizeI, offsetJ, offsetI);
			Foreground = new OffsetBuffer<byte>(sizeJ, sizeI, offsetJ, offsetI);
			Background = new OffsetBuffer<byte>(sizeJ, sizeI, offsetJ, offsetI);
		}

		public FrameBuffer(FrameBuffer other, int offsetJ = 0, int offsetI = 0)
		{
			_sizeJ = other._sizeJ;
			_sizeI = other._sizeI;
			_offsetJ = other._offsetJ + offsetJ;
			_offsetI = other._offsetI + offsetI;
			Char = new OffsetBuffer<char>(other.Char, offsetJ, offsetI);
			Foreground = new OffsetBuffer<byte>(other.Foreground, offsetJ, offsetI);
			Background = new OffsetBuffer<byte>(other.Background, offsetJ, offsetI);
		}

		public static void Copy(FrameBuffer destination, FrameBuffer source)
		{
			var sizeJ = Math.Min(source.SizeJ, destination._sizeJ);
			var sizeI = Math.Min(source.SizeI, destination._sizeI);

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

		public struct OffsetBuffer<T>
		{
			private readonly T[,] _buffer;

			public readonly int SizeJ
			{ get => _buffer.GetLength(0); }
			public readonly int SizeI
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

			public readonly T this[int j, int i]
			{
				get { return _buffer[OffsetJ + j, OffsetI + i]; }
				set { _buffer[OffsetJ + j, OffsetI + i] = value; }
			}
		}
	}
}
