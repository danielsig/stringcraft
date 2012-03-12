using System;
using System.Reflection;

namespace StringCraft
{
	public static class XObject
	{
		public static bool HasMethod(this object obj, string methodName)
		{
			var type = obj.GetType();
			return type.GetMethod(methodName) != null;
		}
		public static System.Reflection.MethodInfo GetMethod(this object obj, string methodName)
		{
			var type = obj.GetType();
			return type.GetMethod(methodName);
		}
		public static T Call<T>(this object obj, string methodName, params object[] parameters)
		{
			var type = obj.GetType();
			System.Reflection.MethodInfo info = type.GetMethod(methodName);
			if(info != null)  return (T)info.Invoke(obj, parameters);
			return default(T);
		}
		public static object Call(this object obj, string methodName, params object[] parameters)
		{
			var type = obj.GetType();
			System.Reflection.MethodInfo info = type.GetMethod(methodName);
			if(info != null) return info.Invoke(obj, parameters);
			return null;
		}
		public static string GetShortTypeName(this object obj)
		{
			return obj.GetType().GetShortName();
		}
	}
}

