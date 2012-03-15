using System;

namespace StringCraft
{
	public static class XChar
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
		public static ConsoleColor ToColor(this char source)
		{
			switch(source)
			{
				case '0':
					return ConsoleColor.Black;
				case '#':
					return ConsoleColor.Black;
				case '1':
					return ConsoleColor.DarkGray;
				case '+':
					return ConsoleColor.DarkGray;
				case '2':
					return ConsoleColor.Gray;
				case '.':
					return ConsoleColor.Gray;
				case '3':
					return ConsoleColor.White;
				case ' ':
					return ConsoleColor.White;
				case 'R':
					return ConsoleColor.Red;
				case 'G':
					return ConsoleColor.Green;
				case 'B':
					return ConsoleColor.Blue;
				case 'r':
					return ConsoleColor.DarkRed;
				case 'g':
					return ConsoleColor.DarkGreen;
				case 'b':
					return ConsoleColor.DarkBlue;
				case 'C':
					return ConsoleColor.Cyan;
				case 'M':
					return ConsoleColor.Magenta;
				case 'Y':
					return ConsoleColor.Yellow;
				case 'c':
					return ConsoleColor.DarkCyan;
				case 'm':
					return ConsoleColor.DarkMagenta;
				case 'y':
					return ConsoleColor.DarkYellow;
				case '4':
					return ConsoleColor.DarkRed;
				case '5':
					return ConsoleColor.DarkGreen;
				case '6':
					return ConsoleColor.DarkBlue;
				case '7':
					return ConsoleColor.DarkCyan;
				case '8':
					return ConsoleColor.DarkMagenta;
				case '9':
					return ConsoleColor.DarkYellow;
				case '!':
					return ConsoleColor.DarkGray;
				default:
					return ConsoleColor.Black;
			}
		}
	}
}

