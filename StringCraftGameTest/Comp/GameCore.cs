using System;
using StringCraft;

namespace StringCraftGameTest
{
	public class GameCore : Component
	{
		public GameCore()
		{
			Symbol.DefineSymbol
		 	( "MyRobot", false,
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
			Symbol.DefineSymbol
		 	( "Dot", false,
				 " \u00B2\u00B1\u00B2 ", " GGRG", "GRRGR",
				 " \u00B0\u00B1\u00B0 ", " RGGG", "GGRRR"
			);
			Symbol.DefineSymbol
		 	( "Colors", false, 25,
				0xF88, 0xFA8, 0xFC8, 0xFD8, 0xFF8, 0xDF8, 0xCF8, 0xAF8, 0x8F8, 0x8FA, 0x8FC, 0x8FD, 0x8FF,
				0x8DF, 0x8CF, 0x8AF, 0x88F, 0xA8F, 0xC8F, 0xD8F, 0xF8F, 0xF8D, 0xF8C, 0xF8A, 0xF88,
				0xF00, 0xF40, 0xF80, 0xFC0, 0xFF0, 0xCF0, 0x8F0, 0x4F0, 0x0F0, 0x0F4, 0x0F8, 0x0FC, 0x0FF,
				0x0CF, 0x08F, 0x04F, 0x00F, 0x40F, 0x80F, 0xC0F, 0xF0F, 0xF0C, 0xF08, 0xF04, 0xF00,
				0x800, 0x820, 0x840, 0x860, 0x880, 0x680, 0x480, 0x280, 0x080, 0x082, 0x084, 0x086, 0x088,
				0x068, 0x048, 0x028, 0x008, 0x208, 0x408, 0x608, 0x808, 0x806, 0x804, 0x802, 0x800,
				0x400, 0x410, 0x420, 0x430, 0x440, 0x340, 0x240, 0x140, 0x040, 0x041, 0x042, 0x043, 0x044,
				0x034, 0x024, 0x014, 0x004, 0x104, 0x204, 0x304, 0x404, 0x403, 0x402, 0x401, 0x400,
				 
				0x000, 0x100, 0x200, 0x300, 0x400, 0x500, 0x600, 0x700, 0x800, 0x900, 0xA00, 0xB00, 0xC00,
				0xD00, 0xE00, 0xF00, 0xF11, 0xF22, 0xF44, 0xF66, 0xF88, 0xFAA, 0xFCC, 0xFDD, 0xFFF,
				 
				0x000, 0x010, 0x020, 0x030, 0x040, 0x050, 0x060, 0x070, 0x080, 0x090, 0x0A0, 0x0B0, 0x0C0,
				0x0D0, 0x0E0, 0x0F0, 0x1F1, 0x2F2, 0x4F4, 0x6F6, 0x8F8, 0xAFA, 0xCFC, 0xDFD, 0xFFF,
				 
				0x000, 0x001, 0x002, 0x003, 0x004, 0x005, 0x006, 0x007, 0x008, 0x009, 0x00A, 0x00B, 0x00C,
				0x00D, 0x00E, 0x00F, 0x11F, 0x22F, 0x44F, 0x66F, 0x88F, 0xAAF, 0xCCF, 0xDDF, 0xFFF
			);
			Symbol.DefineSymbol
		 	( "Mario", false, 12,
				 0x000, 0x000, 0x000, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0x000, 0x000, 0x000, 0x000,
				 0x000, 0x000, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0x000,
				 0x000, 0x000, 0x440, 0x440, 0x440, 0x880, 0x880, 0x440, 0x880, 0x000, 0x000, 0x000,
				 0x000, 0x440, 0x880, 0x440, 0x880, 0x880, 0x880, 0x440, 0x880, 0x880, 0x880, 0x000,
				 0x000, 0x440, 0x880, 0x440, 0x440, 0x880, 0x880, 0x880, 0x440, 0x880, 0x880, 0x880,
				 0x000, 0x440, 0x440, 0x880, 0x880, 0x880, 0x880, 0x440, 0x440, 0x440, 0x440, 0x000,
				 0x000, 0x000, 0x000, 0x880, 0x880, 0x880, 0x880, 0x880, 0x880, 0x880, 0x000, 0x000,
				 0x000, 0x000, 0x440, 0x440, 0x440, 0x440, 0x440, 0x440, 0x000, 0x000, 0x000, 0x000,
				 
				 0x000, 0x440, 0x440, 0x440, 0xF00, 0x440, 0x440, 0xF00, 0x440, 0x440, 0x440, 0x000,
				 0x440, 0x440, 0x440, 0x440, 0xF00, 0xF00, 0xF00, 0xF00, 0x440, 0x440, 0x440, 0x440,
				 0x880, 0x880, 0x440, 0xF00, 0x880, 0xF00, 0xF00, 0x880, 0xF00, 0x440, 0x880, 0x880,
				 0x880, 0x880, 0x880, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0x880, 0x880, 0x880,
				 0x880, 0x880, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0xF00, 0x880, 0x880,
				 0x000, 0x000, 0xF00, 0xF00, 0xF00, 0x000, 0x000, 0xF00, 0xF00, 0xF00, 0x000, 0x000,
				 0x000, 0x440, 0x440, 0x440, 0x000, 0x000, 0x000, 0x000, 0x440, 0x440, 0x440, 0x000,
				 0x440, 0x440, 0x440, 0x440, 0x000, 0x000, 0x000, 0x000, 0x440, 0x440, 0x440, 0x440
			);
			Symbol.DefineSymbol
		 	( "Ball", false,
				 "         ", "         ", "?????????",
				 "         ", "         ", "??!!!!!??",
				 "         ","         ", "?!!!!!!!?",
				 "         ", "         ", "!!!!!!!!!",
				 "         ", "         ", "!!!!!!!!!",
				 "         ", "         ", "!!!!!!!!!",
				 "         ", "         ", "?!!!!!!!?",
				 "         ", "         ", "??!!!!!??",
				 "         ", "         ", "?????????"
			);
			Symbol.DefineSymbol
		 	( "RedBall", false,
				 "         ", "?????????", "?????????",
				 "         ", "??!!!!!??", "??44444??",
				 "         ","?!!!!!!!?", "?4444444?",
				 "         ", "!!!!!!!!!", "444444444",
				 "         ", "!!!!!!!!!", "444444444",
				 "         ", "!!!!!!!!!", "444444444",
				 "         ", "?!!!!!!!?", "?4444444?",
				 "         ", "??!!!!!??", "??44444??",
				 "         ", "?????????", "?????????"
			);
			Symbol.DefineSymbol
		 	( "GreenBall", false,
				 "         ", "?????????", "?????????",
				 "         ", "??!!!!!??", "??55555??",
				 "         ","?!!!!!!!?", "?5555555?",
				 "         ", "!!!!!!!!!", "555555555",
				 "         ", "!!!!!!!!!", "555555555",
				 "         ", "!!!!!!!!!", "555555555",
				 "         ", "?!!!!!!!?", "?5555555?",
				 "         ", "??!!!!!??", "??55555??",
				 "         ", "?????????", "?????????"
			);
			Symbol.DefineSymbol
		 	( "BlueBall", false,
				 "         ", "?????????", "?????????",
				 "         ", "??!!!!!??", "??66666??",
				 "         ","?!!!!!!!?", "?6666666?",
				 "         ", "!!!!!!!!!", "666666666",
				 "         ", "!!!!!!!!!", "666666666",
				 "         ", "!!!!!!!!!", "666666666",
				 "         ", "?!!!!!!!?", "?6666666?",
				 "         ", "??!!!!!??", "??66666??",
				 "         ", "?????????", "?????????"
			);
		}
		
		private void Awake ()
		{
			Rigidbody.WorldArea = Camera.MainCamera.WorldScreen;
			Rigidbody.Gravity.Y = 3;
		}
		private void Start ()
		{
			GameObject test = new GameObject("Test");
			test.AddComponent<MarioController>();
			test.Position = new Vector2(60, 25);
			
			string ballName = "Ball";
			
			for(int i = 0; i < 80; i++)
			{
				GameObject ball = new GameObject("Ball" + i);
				Renderer renderer = ball.AddComponent<Renderer>();
				Rigidbody body = ball.AddComponent<Rigidbody>();
				Bouncer bouncer = ball.AddComponent<Bouncer>();
				
				renderer.SetSymbolByName(ballName);
				renderer.Anchor = Anchor.Center;
				
				body.LocalCollider = new Rectangle(Anchor.Center, 0, 0, 9, 9);
				ball.Position = Camera.MainCamera.WorldScreen.GetRandomVectorInsideRect();
				
				bouncer.Amount = 0.2;
				
				if(i % 40 == 10) ballName = "RedBall";
				if(i % 40 == 20) ballName = "GreenBall";
				if(i % 40 == 30) ballName = "BlueBall";
			}
		}
		
		public int Counter = 0;
	}
}

