namespace IronEngine.IO
{
	public interface IInput
	{
		public IActionable PickActionable(IEnumerable<IActionable> actionables);

		public Action PickAction(IEnumerable<Action> actions);
	}
}
