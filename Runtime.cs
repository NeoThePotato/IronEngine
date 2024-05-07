using System.Collections;
using IronEngine.IO;

namespace IronEngine
{
	public abstract class Runtime : IDestroyable
	{
		#region SINGLETON
		private static Runtime _instance;
		internal static Runtime Instance => _instance;
		#endregion

		#region TILEMAP
		protected TileMap TileMap { get; private set; }
		#endregion

		#region ACTORS
		private List<Actor> _actors;
		protected Actor CurrentActor => _turnCounter.Current;
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

		protected uint Turn => _turnCounter._turnCounter;

		public Runtime()
		{
			_instance = this;
			_actors = CreateActors().ToList();
			TileMap = CreateTileMap();
			_turnCounter = new(_actors);
		}

		public void Run()
		{
			PlayerLoop();
			PlayerExit();
		}

		private void PlayerLoop()
		{
			while (_turnCounter.MoveNext() && !ExitCondition)
			{
				bool advanceTurn;
				do
				{
					var actionable = Input.PickActionable(CurrentActor._myActionables);
					var action = Input.PickAction(actionable.GetAvailableActions());
					advanceTurn = action.Invoke();
				}
				while (!advanceTurn);
			}
		}

		private void PlayerExit() => OnExit();

		public void Destroy()
		{
			TileMap.Destroy();
			_instance = null;
		}
		#endregion

		#region ABSTRACT
		protected abstract bool ExitCondition { get; }

		protected abstract IInput Input { get; }

		protected abstract IEnumerable<Actor> CreateActors();

		protected abstract TileMap CreateTileMap();

		protected abstract void OnExit();
		#endregion

		#region TURN_ENUMERATOR
		private class TurnEnumerator : IEnumerator<Actor>
		{
			private List<Actor> _actors;
			internal Actor _currentActor;
			private int _currentActorIndex = -1;
			internal uint _turnCounter = 0;
			private int CurrentActorIndex { get => _currentActorIndex; set => _currentActorIndex = value % _actors.Count; }

			public TurnEnumerator(List<Actor> actors)
			{
				_actors = actors;
			}

			public Actor Current => _currentActor;

			object IEnumerator.Current => Current;

			public void Dispose() { }

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
