﻿using System.Collections;

namespace IronEngine
{
	public class Actor : IEnumerable<IHasActor>
	{
		internal HashSet<IHasActor> _myObjects;
		internal HashSet<IActionable> _myActionables;

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

		public IEnumerator<IHasActor> GetEnumerator() => (_myObjects as IEnumerable<IHasActor>).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => (_myObjects as IEnumerable).GetEnumerator();
	}

	public interface IHasActor
	{
		Actor? Actor { get; internal set; }

		bool HasActor => Actor != null;
	}

	public interface IActionable : IHasActor
	{
		IEnumerable<Func<bool>>? GetAvailableActions();
	}
}
