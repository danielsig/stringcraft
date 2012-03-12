using System;
using System.Reflection;

namespace StringCraft
{
	public static class XType
	{
		public static string GetShortName(this Type type)
		{
			string name = type.ToString();
			int index = name.LastIndexOf(".") + 1;
			return name.Substring(index < 0 ? 0 : index);
		}
	}
}

