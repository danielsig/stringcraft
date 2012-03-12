using System;

namespace StringCraft
{
	public class StringCraftObject
	{
		public void Log(object message)
		{
			System.Console.WriteLine(this.ToString() + ": " + message);
		}
	}
}

