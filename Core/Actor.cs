using System.Collections;

namespace IronEngine
{
	/// <summary>
	/// This class contains logic for sending commands to <see cref="ICommandAble"/> instances.
	/// A single player will be represented with an instance of this class.
	/// </summary>
	public class Actor : IEnumerable<IHasActor>, IDestroyable
	{
		internal HashSet<IHasActor> _myObjects = new(1);
		internal HashSet<ICommandAble> _myActionable = new(1);

		/// <summary>
		/// Returns all objects which belong to this <see cref="Actor"/>.
		/// </summary>
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

		/// <summary>
		/// Returns all objects which belong to this <see cref="Actor"/>.
		/// </summary>
		public IEnumerator<IHasActor> GetEnumerator() => _myObjects.GetEnumerator();

		/// <summary>
		/// Returns all objects which belong to this <see cref="Actor"/>.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator() => _myObjects.GetEnumerator();

		/// <summary>
		/// Destroys this <see cref="Actor"/> and removes them from the <see cref="Runtime"/>.
		/// </summary>
		public void Destroy()
		{
			_myObjects = null;
			_myActionable = null;
			Runtime.Instance?.RemoveActor(this);
		}

		/// <summary>
		/// Destroys this <see cref="Actor"/> and removes them from the <see cref="Runtime"/>.
		/// Additionally, destroys all children which belong to this <see cref="Actor"/>.
		/// <seealso cref="Destroy"/>
		/// </summary>
		public void DestroyWithChildren()
		{
			foreach (IHasActor child in _myObjects)
			{
				if (child is IDestroyable destroyable)
					destroyable.Destroy();
			}
			Destroy();
		}

		/// <summary>
		/// All <see cref="ICommandAble"/> objects which belong to this <see cref="Actor"/> get filtered through this function when being passed to the input.
		/// Override it to exclude or include <see cref="ICommandAble"/>s before passing them to the input handling.
		/// </summary>
		/// <param name="source">All <see cref="ICommandAble"/> instances passed by default.</param>
		/// <returns>A filtered <see cref="IEnumerable"/> of <paramref name="source"/>.</returns>
		protected virtual IEnumerable<ICommandAble> FilterCommandAble(IEnumerable<ICommandAble> source) => source;
		
		/// <summary>
		/// Callback for when this <see cref="Actor"/>'s turn starts.
		/// </summary>
		protected internal virtual void OnTurnStart() { }

		/// <summary>
		/// Callback for when this <see cref="Actor"/>'s turn ends.
		/// </summary>
		protected internal virtual void OnTurnOver() { }

		internal IEnumerable<ICommandAble> GetFilteredCommandAbleWithAvailableActions() => FilterCommandAble(_myActionable.Where(a => a.GetAvailableActions().Any()));
	}

	/// <summary>
	/// Interface for all objects which can optionally belong to an <see cref="Actor"/>.
	/// </summary>
	public interface IHasActor
	{
		/// <summary>
		/// The <see cref="Actor"/> to which this object belongs.
		/// </summary>
		Actor? Actor { get; internal set; }

		/// <summary>
		/// Returns whether this instance has an <see cref="Actor"/>. (Actor != <see langword="null"/>)
		/// </summary>
		bool HasActor => Actor != null;
	}

	/// <summary>
	/// Interface for all objects which, when belonging to an <see cref="Actor"/>, can receive <see cref="Command"/>s from.
	/// </summary>
	public interface ICommandAble : IHasActor
	{
		/// <returns>An <see cref="IEnumerable"/> of all the <see cref="Command"/>s this instance can perform at the time this function is called.</returns>
		IEnumerable<Command> GetAvailableActions();

		/// <summary>
		/// Data structure for passing commands to the <see cref="Runtime"/> in a manner that is user-readable and the engine can parse.
		/// </summary>
		/// <param name="action"><see cref="Action"/> to perform when this command is invoked.</param>
		/// <param name="description">Player-readable description of what this command does.</param>
		/// <param name="key">The key to select this command.</param>
		/// <param name="endsTurn">Tells the engine whether this command ends the <see cref="Actor"/>'s turn.</param>
		public readonly struct Command(Action action, string description, string? key = null, bool endsTurn = true) : IHasKey
		{
			private readonly string? key = key;
			public readonly string description = description;
			public readonly Action action = action;
			public readonly bool endsTurn = endsTurn;

			public readonly string? Key => key;
			public readonly string Description => description;

			public bool HasKey => !string.IsNullOrEmpty(Key);

			internal readonly void Invoke() => action.Invoke();

			public static readonly Command Return = new(() => { }, "Deselect currently-selected object", "Deselect", false);
		}

		/// <summary>
		/// Interface for instance which participate in <see cref="Command"/>s.
		/// Implement this interface for easy & convenient keying of objects when using <see cref="Command"/>s.
		/// </summary>
		public interface IHasKey
		{
			/// <summary>
			/// The key to use for this object.
			/// </summary>
			/// <example>A2</example>
			public string? Key { get; }

			/// <summary>
			/// The description of what the key does when selected.
			/// </summary>
			public string Description { get; }

			/// <summary>
			/// Returns whether <see cref="Key"/> exists.
			/// </summary>
			public bool HasKey => !string.IsNullOrEmpty(Key);
		}
	}
}
