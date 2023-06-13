﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

static class Utility
{
	public static void Swap<T>(ref T obj1, ref T obj2)
	{
		var temp = obj1;
		obj1 = obj2;
		obj2 = temp;
	}

	public static float ClampMin(float val, float min)
	{
		return Math.Max(val, min);
	}

	public static float ClampMax(float val, float max)
	{
		return Math.Min(val, max);
	}

	public static float ClampRange(float val, float min, float max)
	{
		return ClampMin(ClampMax(val, max), min);
	}

	public static int ClampMin(int val, int min)
	{
		return Math.Max(val, min);
	}

	public static int ClampMax(int val, int max)
	{
		return Math.Min(val, max);
	}

	public static int ClampRange(int val, int min, int max)
	{
		return ClampMin(ClampMax(val, max), min);
	}

	public static void BlockUntilKeyDown()
	{
		Console.ReadKey();
	}

	public static T[] RemoveDuplicates<T>(T[] objArr)
	{
		return objArr.Distinct().ToArray();
	}

	public static void PrintAll<T>(T[] objArr, Separator separator)
	{
		string separatorStr;

		switch (separator)
		{
			case Separator.Comma:
				separatorStr = ", ";
				break;
			case Separator.Space:
				separatorStr = " ";
				break;
			case Separator.NewLine:
				separatorStr = "\n";
				break;
			default:
				separatorStr = ", ";
				break;
		}

		for (int i = 0; i < objArr.Length-1; i++)
		{
			Console.Write(objArr[i] + separatorStr);
		}

		Console.WriteLine(objArr[objArr.Length-1]);
	}

	[SupportedOSPlatform("windows")]
	public static void EnableVirtualTerminalProcessing()
	{
		const int STD_OUTPUT_HANDLE = -11;
		const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

		var handle = GetStdHandle(STD_OUTPUT_HANDLE);
		GetConsoleMode(handle, out uint mode);
		mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;

		try
		{
			SetConsoleMode(handle, mode);
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex.ToString());
		}
	}

	[SupportedOSPlatform("windows")]
	[DllImport("kernel32.dll")]
	static private extern IntPtr GetStdHandle(int nStdHandle);

	[SupportedOSPlatform("windows")]
	[DllImport("kernel32.dll")]
	static private extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

	[SupportedOSPlatform("windows")]
	[DllImport("kernel32.dll")]
	static private extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
}
enum Separator
{
	Comma,
	Space,
	NewLine,
}