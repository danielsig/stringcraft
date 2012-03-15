using System;

namespace StringCraft
{
	public static class XConsoleColor
	{
		/*
			0123 = grayscale 0 is black, 3 is white
				optional..
				# = black
				+ = dark gray
				. = light gray
				  = white (space)
			RGB = red, green and blue
			CMY = cyan, magenta and yellow
			
			rgb = dark red, green and blue
			cmy = dark cyan, magenta and yellow
			
			?   = transparent
			!   = transparent white
			@   = transparent black
			
			456 = transparent red, green and blue
			789 = transparent cyan, magenta and yellow
		*/
		public static char ToChar(this ConsoleColor color)
		{
			switch(color)
			{
				case ConsoleColor.Black:
					return '#';
				case ConsoleColor.DarkGray:
					return '+';
				case ConsoleColor.Gray:
					return '.';
				case ConsoleColor.White:
					return ' ';
				case ConsoleColor.Red:
					return 'R';
				case ConsoleColor.Green:
					return 'G';
				case ConsoleColor.Blue:
					return 'B';
				case ConsoleColor.DarkRed:
					return 'r';
				case ConsoleColor.DarkGreen:
					return 'g';
				case ConsoleColor.DarkBlue:
					return 'b';
				case ConsoleColor.Cyan:
					return 'C';
				case ConsoleColor.Magenta:
					return 'M';
				case ConsoleColor.Yellow:
					return 'Y';
				case ConsoleColor.DarkCyan:
					return 'c';
				case ConsoleColor.DarkMagenta:
					return 'm';
				case ConsoleColor.DarkYellow:
					return 'y';
				default:
					return '#';//black
			}
		}
	}
}

