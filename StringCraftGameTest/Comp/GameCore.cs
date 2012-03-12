using System;
using StringCraft;

namespace StringCraftGameTest
{
	public class GameCore : Component
	{
		public GameCore()
		{
			Symbol.DefineSymbol
		 	( "myRobot", false,
				 "   .-\"\"-.   ", "BB#### ###BB", "###+....+###",
				 "  /[] _|_\\  ",  "BB##### ##BB", "##+BB....+##",
				 " _|_o_LII|_ ",   "B###+BB B##B", "##.RB..BB.##",
				 "/ | ==== | \\",  "###BBBBBB###", ".          .",
				 "|_| ==== |_|",   "B##BBBBBB##B", "            ",
				 "  |\" ||  |  ",  "B##BBBBBB##B", "#          #",
				 "  |LI  o |  ",   "B##BBBBBB##B", "#          #",
				 "  |'----'|  ",   "B##########B", "#  ......  #",
				 "/__|    |__\\",  " ########## ", ".   ####   ."
			);
		}
		
		public void Awake ()
		{
		}
		public void Start ()
		{
			GameObject test = new GameObject("Test", StringCraft.GameObject.WORLD);
			test.AddComponent<Renderer>().SetSymbolByName("myRobot");
			test.AddComponent<TestComponent>();
			test.Position = new Vector2(60, 25);
		}
		
		public int Counter = 0;
	}
}

