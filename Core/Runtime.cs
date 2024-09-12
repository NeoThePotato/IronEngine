using System.Collections;
using IronEngine.IO;

namespace IronEngine
{
	/// <summary>
	/// This class handles the core of the engine's runtime.
	/// Implement this to create your game's rules.
	/// Call <see cref="Run"/> to run this.
	/// </summary>
	public abstract class Runtime : IDestroyable
	{
		#region SINGLETON
		public static Runtime Instance { get; private set; }
		#endregion

		#region TILEMAP
		/// <summary>
		/// The <see cref="TileMap"/> which is managed by this <see cref="Runtime"/>.
		/// </summary>
		public TileMap TileMap { get; private set; }
		#endregion

		#region ACTORS
		private List<Actor> _actors;

		/// <summary>
		/// The <see cref="Actor"/> which is currently acting.
		/// </summary>
		public Actor CurrentActor => _turnCounter.Current;

		/// <summary>
		/// All <see cref="Actor"/>s which the <see cref="Runtime"/> currently manages.
		/// </summary>
		protected IEnumerable<Actor> Actors => _actors;

		internal bool RemoveActor(Actor actor)
		{
			if (actor == null || !_actors.Contains(actor)) return false;
			_actors.Remove(actor);
			return true;
		}
		#endregion

		#region PLAYER
		private TurnEnumerator _turnCounter;

		/// <summary>
		/// Current turn count.
		/// </summary>
		public uint Turn => _turnCounter._turnCounter;

		public Runtime()
		{
			Instance = this;
			_actors = CreateActors().ToList();
			TileMap = CreateTileMap();
			Renderer = CreateRenderer();
			_turnCounter = new(_actors);
		}

		/// <summary>
		/// Entry point for running this <see cref="Runtime"/>.
		/// </summary>
		public void Run()
		{
			OnGameStart();
			PlayerLoop();
			PlayerExit();
		}

		private void PlayerLoop()
		{
			while (_turnCounter.MoveNext() && !ExitCondition)
			{
				CurrentActor.OnTurnStart();
				bool advanceTurn;
				do
				{
					ICommandAble selectedCommandAble;
					var commandAbles = CurrentActor.GetFilteredCommandAbleWithAvailableActions();
					if (!commandAbles.Any())
						break;
					selectedCommandAble = Input.PickCommandAble(commandAbles);
					var command = Input.PickCommand(selectedCommandAble.GetAvailableActions());
					OnCommandSelected(ref command);
					command.Invoke();
					advanceTurn = command.endsTurn;
					Renderer.UpdateFrame();
				}
				while (!advanceTurn);
				CurrentActor.OnTurnOver();
			}
		}

		private void PlayerExit() => OnExit();

		/// <summary>
		/// Destroys this <see cref="Runtime"/> and release all managed resources.
		/// </summary>
		public void Destroy()
		{
			TileMap.Destroy();
			Instance = null;
		}
		#endregion

		#region RENDERER
		/// <summary>
		/// The <see cref="IRenderer"/> responsible for rendering frames to the user.
		/// </summary>
		public IRenderer Renderer { get; private set; }
		#endregion

		#region ABSTRACT
		/// <summary>
		/// Condition for the <see cref="Runtime"/> to finish executing.
		/// </summary>
		public abstract bool ExitCondition { get; }

		/// <summary>
		/// The <see cref="IInput"/> responsible for receiving input from the <see cref="Actor"/>s.
		/// </summary>
		public abstract IInput Input { get; }

		/// <summary>
		/// Called when the <see cref="Runtime"/> is constructed.
		/// Implement this to create <see cref="Actor"/>s for the <see cref="Runtime"/> to manage.
		/// </summary>
		/// <returns>The created <see cref="Actor"/>s for the <see cref="Runtime"/> to manage.</returns>
		protected abstract IEnumerable<Actor> CreateActors();

		/// <summary>
		/// Called when the <see cref="Runtime"/> is constructed.
		/// Implement this to create a <see cref="TileMap"/> for the <see cref="Runtime"/> to manage.
		/// </summary>
		/// <returns>The created <see cref="TileMap"/> for the <see cref="Runtime"/> to manage.</returns>
		protected abstract TileMap CreateTileMap();

		/// <summary>
		/// Called when the <see cref="Runtime"/> is constructed.
		/// Implement this to create a <see cref="IRenderer"/> for the <see cref="Runtime"/> to manage.
		/// </summary>
		/// <returns>The created <see cref="IRenderer"/> for the <see cref="Runtime"/> to manage.</returns>
		protected abstract IRenderer CreateRenderer();

		/// <summary>
		/// Callback for when the game starts.
		/// </summary>
		/// <seealso cref="Run"/>
		protected virtual void OnGameStart() { }

		/// <summary>
		/// Callback for when the game ends.
		/// </summary>
		protected virtual void OnExit() { }

		/// <summary>
		/// Callback for when a <see cref="ICommandAble.Command"/> is selected.
		/// </summary>
		/// <param name="command">The selected <see cref="ICommandAble.Command"/>.</param>
		protected virtual void OnCommandSelected(ref ICommandAble.Command command) { }
		#endregion

		#region TURN_ENUMERATOR
		private struct TurnEnumerator(List<Actor> actors) : IEnumerator<Actor>
		{
			private List<Actor> _actors = actors;
			internal Actor _currentActor;
			private int _currentActorIndex = -1;
			internal uint _turnCounter = 0;
			private int CurrentActorIndex { get => _currentActorIndex; set => _currentActorIndex = value % _actors.Count; }

			public Actor Current => _currentActor;

			object IEnumerator.Current => Current;

			public readonly void Dispose() { }

			public bool MoveNext()
			{
				if (_actors.Count == 0)
					return false;
				int indexOfCurrentActor = _actors.IndexOf(_currentActor);
				if (indexOfCurrentActor == CurrentActorIndex)
					CurrentActorIndex++;
				else if (indexOfCurrentActor != -1)
					CurrentActorIndex = indexOfCurrentActor + 1;
				else
					CurrentActorIndex = CurrentActorIndex;
				_currentActor = _actors[CurrentActorIndex];
				_turnCounter++;
				return true;
			}

			public void Reset() => _currentActorIndex = -1;
		}
		#endregion
	}
}
