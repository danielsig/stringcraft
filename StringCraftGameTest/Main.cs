using System;
using StringCraft;

namespace StringCraftGameTest
{
	class MainClass
	{
		[STAThread]
		public static int Main(string[] args)
		{
			Engine.Init<GameCore>(30, 100, 50, ConsoleColor.Black);
			return 0;
		}
	}
}
