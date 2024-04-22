namespace IronEngine
{
	internal static class Utilities
	{
		public static T CloneShallow<T>(T original)
		{
			if (original == null)
				return default;
			T newObject = (T)Activator.CreateInstance(original.GetType());
			var properties = original.GetType().GetProperties();
			foreach (var originalProp in properties)
				originalProp.SetValue(newObject, originalProp.GetValue(original));
			return newObject;
		}

		public static T CloneDeep<T>(T original)
		{
			if (original == null)
				return default;
			T newObject = (T)Activator.CreateInstance(original.GetType());
			var properties = original.GetType().GetProperties();
			foreach (var originalProp in properties)
				originalProp.SetValue(newObject, CloneDeep(originalProp.GetValue(original)));
			return newObject;
		}
	}
}
