using System;
using System.Text;

namespace StringCraft
{
	public static class XString
	{
		public static string Times(this string source, int multiplier)
		{
		   StringBuilder sb = new StringBuilder(multiplier * source.Length);
		   for (int i = 0; i < multiplier; i++)
		   {
		       sb.Append(source);
		   }
		
		   return sb.ToString();
		}
		public static string Times(this char source, int multiplier)
		{
		   StringBuilder sb = new StringBuilder(multiplier);
		   for (int i = 0; i < multiplier; i++)
		   {
		       sb.Append(source);
		   }
		
		   return sb.ToString();
		}
		public static string ToLength(this string source, int length)
		{
			if(length < source.Length) return source.Substring(0, length);
			return source.Times(length / source.Length);
		}
		public static string ToLength(this char source, int length)
		{
		    return source.Times(length);
		}
	}
}

