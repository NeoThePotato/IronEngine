namespace IO.Render
{

	/// <summary>
	/// Interface which can be rendered into a Renderer
	/// </summary>
	interface IRenderable
	{
		public int SizeI { get; }
		public int SizeJ { get; }
		public char[,] Render();
	}
}
