namespace IronEngine
{
	public class Actor
	{

	}

	public interface IHasActor
	{
		Actor? Actor { get; }

		bool HasActor => Actor != null;
	}
}
