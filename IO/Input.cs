namespace IronEngine.IO
{
	public interface IInput
	{
		public IActionable PickActionable(IEnumerable<IActionable> actionables);

		public IActionable.Action PickAction(IEnumerable<IActionable.Action> actions);
	}
}
