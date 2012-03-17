using System;
using StringCraft;

namespace StringCraftGameTest
{
	class MainClass
	{
		[STAThread]
		public static int Main(string[] args)
		{
			Engine.Init<GameCore>(50, -1, -1, ConsoleColor.Black);
			return 0;
		}
	}
}
