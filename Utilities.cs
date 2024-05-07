using System.Diagnostics;

namespace IronEngine
{
	internal static class Utilities
	{
		internal static T CloneShallow<T>(T original)
		{
			if (original == null)
				return default;
			T newObject = (T)Activator.CreateInstance(original.GetType());
			var properties = original.GetType().GetProperties();
			foreach (var originalProp in properties)
			{
				try
				{
					originalProp.SetValue(newObject, originalProp.GetValue(original));
				}
				catch (ArgumentException) { }
			}
			return newObject;
		}

		internal static T CloneDeep<T>(T original)
		{
			if (original == null)
				return default;
			T newObject = (T)Activator.CreateInstance(original.GetType());
			var properties = original.GetType().GetProperties();
			foreach (var originalProp in properties)
				originalProp.SetValue(newObject, CloneDeep(originalProp.GetValue(original)));
			return newObject;
		}

		#region VALIDTY_CHECKS
		internal static bool CheckHasTileMap(this IPositionable positionable)
		{
			bool hasTileMap = positionable.TileMap != null;
			Debug.WriteLineIf(!hasTileMap, $"{positionable} is not on a TileMap.");
			return hasTileMap;
		}

		internal static bool CheckWithinTileMap(this IPositionable positionable, Position position)
		{
			bool withinTileMap = positionable.TileMap.WithinBounds(position);
			Debug.WriteLineIf(!withinTileMap, $"{position} is not within the bounds of TileMap.");
			return withinTileMap;
		}

		internal static bool CheckSameTileMap(this IPositionable positionable, Tile tile)
		{
			bool sameTileMap = positionable.CurrentTile.SameTileMap(tile);
			Debug.WriteLineIf(!sameTileMap, $"{tile} is not on the same TileMap as {positionable}.");
			return sameTileMap;
		}
		#endregion
	}
}
