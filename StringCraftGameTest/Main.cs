using System;
using StringCraft;

namespace StringCraftGameTest
{
	class MainClass
	{
		public static int Main(string[] args)
		{
			Engine.Init<GameCore>(20);
			return 0;
		}
	}
}
