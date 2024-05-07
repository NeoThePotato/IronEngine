using System.Collections;

namespace IronEngine
{
	public class Actor : IEnumerable<IHasActor>, IDestroyable
	{
		internal HashSet<IHasActor> _myObjects = new(1);
		internal HashSet<ICommandAble> _myActionable = new(1);

		public IEnumerable<IHasActor> MyObjects => _myObjects;

		internal void AddChild(IHasActor child)
		{
			_myObjects.Add(child);
			if (child is ICommandAble actionable)
				_myActionable.Add(actionable);
		}

		internal void RemoveChild(IHasActor child)
		{
			_myObjects.Remove(child);
			if (child is ICommandAble actionable)
				_myActionable.Remove(actionable);
		}

		public IEnumerator<IHasActor> GetEnumerator() => _myObjects.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _myObjects.GetEnumerator();

		public void Destroy()
		{
			_myObjects = null;
			_myActionable = null;
			Runtime.Instance?.RemoveActor(this);
		}

		public void DestroyWithChildren()
		{
			foreach (IHasActor child in _myObjects)
			{
				if (child is IDestroyable destroyable)
					destroyable.Destroy();
			}
			Destroy();
		}

		internal IEnumerable<ICommandAble> GetActionableWithAvailableActions() => _myActionable.Where(a => a.GetAvailableActions().Any());
	}

	public interface IHasActor
	{
		Actor? Actor { get; internal set; }

		bool HasActor => Actor != null;
	}

	public interface ICommandAble : IHasActor
	{
		IEnumerable<Command> GetAvailableActions();

		public readonly struct Command(Action action, string description, string? key = null, bool endsTurn = true)
		{
			public readonly string? key = key;
			public readonly string description = description;
			public readonly Action action = action;
			public readonly bool endsTurn = endsTurn;

			internal readonly void Invoke() => action.Invoke();
		}
	}
}
