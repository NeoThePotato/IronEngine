namespace IronEngine.IO
{
	/// <summary>
	/// Interface for an object that can render instances of <see cref="IRenderAble"/>.
	/// </summary>
	public interface IRenderer
	{
		/// <summary>
		/// Update the frame.
		/// </summary>
		void UpdateFrame();
	}

	/// <summary>
	/// Interface for an object that can be rendered using <see cref="IRenderer"/>.
	/// </summary>
	public interface IRenderAble
	{
		/// <returns>The <see cref="IRenderer"/> instance responsible for rendering this object.</returns>
		IRenderer GetRenderer();
	}
}
