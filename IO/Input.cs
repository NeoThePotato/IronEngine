namespace IronEngine.IO
{
	public interface IInput
	{
		public string GetString(string prompt);

		public IActionable PickActionable(IEnumerable<IActionable> actionables);

		public IActionable.Action PickAction(IEnumerable<IActionable.Action> actions);
	}
}
