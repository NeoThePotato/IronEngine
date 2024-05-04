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
		private int _currentActorIndex = 0; // TODO Replace with custom Enumerator
		private int CurrentActorIndex { get => _currentActorIndex; set => _currentActorIndex = value % _actors.Count; }
		protected Actor CurrentActor => _actors[CurrentActorIndex];
		protected IEnumerable<Actor> Actors => _actors;

		internal bool RemoveActor(Actor actor)
		{
			if (actor == null || !_actors.Contains(actor)) return false;
			if (_actors.IndexOf(actor) >= CurrentActorIndex) CurrentActorIndex--;
			_actors.Remove(actor);
			return true;
		}
		#endregion

		#region PLAYER
		public Runtime()
		{
			_instance = this;
			_actors = CreateActors().ToList();
			TileMap = CreateTileMap();
		}

		public void Run()
		{
			PlayerLoop();
			PlayerExit();
		}

		private void PlayerLoop()
		{
			while (!ExitCondition)
			{
				// TODO Turn loop
				CurrentActorIndex++;
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

		protected abstract IEnumerable<Actor> CreateActors();

		protected abstract TileMap CreateTileMap();

		protected abstract void OnExit();
		#endregion
	}
}
