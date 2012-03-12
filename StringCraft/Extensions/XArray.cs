using System;
using System.Text;
using System.Collections;
using System.Linq;

namespace StringCraft
{
	public static class XArray
	{
		public static string Join<T>(this T[] arr)
		{
			return arr.Join<T>(", ");
		}
		public static string Join<T>(this T[] arr, String separator)
		{
			if(arr != null)
			{
				separator = separator ?? "";
				StringBuilder builder = new StringBuilder("[", arr.Length * 5);
				foreach(T element in arr)
				{
					builder.Append(element.ToString());
					builder.Append(separator);
				}
				if(builder.Length > separator.Length)
				{
					builder.Length -= separator.Length;
				}
				builder.Append("]");
				return builder.ToString();
			}
			return "null";
		}
	}
}