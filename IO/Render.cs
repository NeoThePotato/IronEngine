namespace IronEngine.IO
{
	public interface IRenderer
	{
		void UpdateFrame();

		void UpdateFrameFull();

		void WriteLine(string str);
	}

	public interface IRenderAble
	{
		bool isStatic { get; }

		void Render();
	}
}
