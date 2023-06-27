using IO.Render;

namespace Game.World
{
	abstract class Entity
	{
		public abstract string Name
		{ get; }
		public abstract int Level
		{ get; }
		public abstract bool Passable
		{ get; }
		public abstract bool Moveable
		{ get; }
		public abstract bool MarkForDelete
		{ get; }
		public abstract EncounterManager.EncounterType EncounterType
		{ get; }
		public virtual EntityRenderer.VisualEntityInfo VisualInfo
		{ get => Assets.EntitiesVisualInfo.DEFAULT; }

		public override string ToString()
		{
			return $"{Name} Lv{Level}";
		}
	}
}
