using System.Collections;

namespace IronEngine
{
	public class Actor : IEnumerable<IHasActor>, IDestroyable
	{
		internal HashSet<IHasActor> _myObjects = new(1);
		internal HashSet<IActionable> _myActionables = new(1);

		public IEnumerable<IHasActor> MyObjects => _myObjects;

		internal void AddChild(IHasActor child)
		{
			_myObjects.Add(child);
			if (child is IActionable actionable)
				_myActionables.Add(actionable);
		}

		internal void RemoveChild(IHasActor child)
		{
			_myObjects.Remove(child);
			if (child is IActionable actionable)
				_myActionables.Remove(actionable);
		}

		public IEnumerator<IHasActor> GetEnumerator() => _myObjects.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _myObjects.GetEnumerator();

		public void Destroy()
		{
			_myObjects = null;
			_myActionables = null;
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
	}

	public interface IHasActor
	{
		Actor? Actor { get; internal set; }

		bool HasActor => Actor != null;
	}

	public interface IActionable : IHasActor
	{
		IEnumerable<Action> GetAvailableActions();

		public struct Action
		{
			public string description;
			public Func<bool> action;

			public Action(string description, Func<bool> action)
			{
				this.description = description;
				this.action = action;
			}

			internal bool Invoke() => action();
		}
	}
}
