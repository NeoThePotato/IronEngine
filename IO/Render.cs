namespace IronEngine.IO
{
	public interface IRenderer
	{
		void UpdateFrame();
	}

	public interface IRenderAble
	{
		IRenderer GetRenderer();
	}
}
