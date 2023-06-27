using static IO.Render.EntityRenderer;

namespace Assets
{
	static class EntitiesVisualInfo
	{
		public static readonly VisualEntityInfo DEFAULT			= new VisualEntityInfo('?', 163);
		public static readonly VisualEntityInfo INVISIBLE		= new VisualEntityInfo(null, null);
		public static readonly VisualEntityInfo UNIT_PLAYER		= new VisualEntityInfo('@', 15);
		public static readonly VisualEntityInfo UNIT_ENEMY		= new VisualEntityInfo('x', 15);
		public static readonly VisualEntityInfo TRAP_ARMED		= INVISIBLE;
		public static readonly VisualEntityInfo TRAP_UNARMED	= new VisualEntityInfo('T', 1);
		public static readonly VisualEntityInfo CHEST			= new VisualEntityInfo('C', 11);
		public static readonly VisualEntityInfo DOOR			= new VisualEntityInfo('D', 15);
		public static readonly VisualEntityInfo PORTAL_ENTRY	= new VisualEntityInfo('E', 2);
		public static readonly VisualEntityInfo PORTAL_EXIT		= new VisualEntityInfo('X', 1);
	}
}
