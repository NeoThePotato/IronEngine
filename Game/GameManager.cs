using World;

namespace Game
{
	class GameManager
	{
		public MapManager MapManager { get; private set; }

		public GameManager()
		{
			LoadMap("../../../Assets/Maps/TestMap.txt");
		}

		public void Start()
		{

		}

		private void Encounter(MapEntity player, MapEntity other)
		{
			throw new NotImplementedException();
		}

		private bool LoadMap(string pathName)
		{
			var charData = IO.File.LoadMapCharData(pathName);

			if (charData != null)
			{
				MapManager = new MapManager(new Map(charData));
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
