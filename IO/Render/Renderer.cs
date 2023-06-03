namespace IO.Render
{
	abstract class Renderer
	{
		private bool _sizeChanged;

		public bool Enabled
		{ get; set; }
		public int SizeJ
		{ get; private set; }
		public int SizeI
		{ get; private set; }
		public int OffsetJ
		{ get; private set; }
		public int OffsetI
		{ get; private set; }
		public bool SizeChanged
		{ 
			get => _sizeChanged;
			private set => _sizeChanged = ChangeSize(value);
		}
		public Renderer? Parent
		{ get; set; }
		private List<Renderer>? Children
		{ get; set; }
		public bool HasParent
		{
			get => Parent != null;
		}
		public bool HasChildren
		{
			get => Children != null && Children.Count > 0;
		}

		public Renderer(int childrenCount = 0, bool enabled = true, int offsetJ = 0, int offsetI = 0)
		{
			Children = (childrenCount > 0? new List<Renderer>(childrenCount) : null);
			Enabled = enabled;
			OffsetJ = offsetJ;
			OffsetI = offsetI;
			SizeChanged = true;
		}

		public void Render(ref char[,] buffer)
		{
			if (Enabled && HasChildren)
			{
				foreach (Renderer child in Children)
				{
					child.Render(ref buffer);
				}
			}
		}

		public void AddChildRenderer(Renderer child)
		{
			if (Children == null)
				Children = new List<Renderer>(1);
			Children.Add(child);
			child.Parent = this;
		}

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

		public static int GetSizeJ(char[,] arr)
		{
			return arr.GetLength(0);
		}

		public static int GetSizeI(char[,] arr)
		{
			return arr.GetLength(1);
		}

		private bool ChangeSize(bool changed)
		{
			if (HasParent && changed)
				Parent.SizeChanged = true;

			return changed;
		}
	}
}
