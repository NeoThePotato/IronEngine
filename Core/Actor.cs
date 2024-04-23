namespace IronEngine
{
	public class Actor
	{

	}

	public interface IActionable
	{
		Actor? Actor { get; }

		bool HasActor => Actor != null;

		IEnumerable<Func<bool>>? GetAvailableActions();
	}
}
