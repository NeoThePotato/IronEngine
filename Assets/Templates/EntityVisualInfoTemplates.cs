using static IO.Render.EntityRenderer;

namespace Assets.Templates
{
	static class EntitiesVisualInfo
	{
		public static readonly VisualEntityInfo DEFAULT =		new('?', 163);
		public static readonly VisualEntityInfo INVISIBLE =		new(null, null);
		public static readonly VisualEntityInfo UNIT_PLAYER =	new('@', 15);
		public static readonly VisualEntityInfo UNIT_ENEMY =	new('x', 15);
		public static readonly VisualEntityInfo TRAP_ARMED =	INVISIBLE;
		public static readonly VisualEntityInfo TRAP_UNARMED =	new('T', 1);
		public static readonly VisualEntityInfo CHEST =			new('C', 11);
		public static readonly VisualEntityInfo DOOR =			new('D', 15);
		public static readonly VisualEntityInfo PORTAL_ENTRY =	new('E', 2);
		public static readonly VisualEntityInfo PORTAL_EXIT =	new('X', 1);
	}
}
