using System;

namespace StringCraft
{
	
	public struct Stringel
	{
		public static readonly Stringel DEFAULT = new Stringel(' ', ' ', '?');
		private static readonly char[] _lightHue =	new char[]{'.',' ','.',' ','.','.','.'};
		private static readonly char[] _hue =		new char[]{'R','Y','G','C','B','M','R'};
		private static readonly char[] _darkHue =	new char[]{'r','y','g','c','b','m','r'};
		private static readonly char[] _blackHue =	new char[]{'#','+','#','+','#','#','#'};
		
		public readonly char Text;
		public readonly char TextColor;
		public readonly char BackColor;
		
		/**
		 * List of all possable outcomes for * operator
		 * */
		
		//Outchar
		
		private static double Hue(double red, double green, double blue)
		{
			double invMax = 1 / Math.Max(Math.Max(red, green), blue);
			double min = Math.Min(Math.Min(red, green), blue);
			red = (red - min) * invMax;
			green = (green - min) * invMax;
			blue = (blue - min) * invMax;
			
			if(blue == 0)	return 			(red == 1	? green	/ 6 : (2 - red)		/ 6);
			if(red == 0)	return (1.0/3) +	(green == 1	? blue	/ 6 : (2 - green)	/ 6);
			if(green == 0)	return (2.0/3) +	(blue == 1	? red	/ 6 : (2 - blue)	/ 6);
			return 0.0111144444;
			/*double invMax = 0.333333333333333 / Math.Max(Math.Max(red, green), blue);
			if(green > blue)
				return green * invMax + blue * invMax * 2;
			else
				return green * invMax + blue * invMax * 2 + red * invMax * 3;*/
		}
		private static double Lightness(double red, double green, double blue)
		{
			double min = Math.Min(Math.Min(red, green), blue);
			red -= min;
			green -= min;
			blue -= min;
			
			return 0.3 + ((red + green + blue) * 0.99 - 0.3) * 0.3;
		}
		
		public Stringel(double red, double green, double blue)
		{
			double hue = Hue(red, green, blue);
			double light = Lightness (red, green, blue);
			
			if(light < 0.075)
			{
				Text = ' ';
				TextColor = ' ';
				BackColor = '#';
				return;
			}
			if(light > 0.95 )
			{
				Text = ' ';
				TextColor = ' ';
				BackColor = ' ';
				return;
			}
			
			double hueIndex = hue * (_hue.Length - 1);
			int hueFloor = (int)Math.Floor(hueIndex);
			int hueCeil = (int)Math.Ceiling(hueIndex);
			double ratio = (hueIndex - hueFloor);
			
			char[] textColors;
			char[] backColors;
			if(light > 0.5)
			{
				if(light > 0.9)//0.875 - 1.00
				{
					textColors = _hue;
					backColors = _lightHue;
				}
				else if(light > 0.85)//0.75 - 0.875
				{
					textColors = _lightHue;
					backColors = _hue;
				}
				else if(light > 0.6)//0.6 - 0.85
				{
					textColors = _hue;
					backColors = _hue;
				}
				else//0.5 - 0.6
				{
					textColors = _darkHue;
					backColors = _hue;
				}
			}
			else if(light > 0.375)//0.375 - 0.50
			{
				textColors = _hue;
				backColors = _darkHue;
			}
			else if(light > 0.25)//0.25 - 0.375
			{
				textColors = _darkHue;
				backColors = _darkHue;
			}
			else if(light > 0.125)//0.125 - 0.25
			{
				textColors = _blackHue;
				backColors = _darkHue;
			}
			else//0.00 - 0.125
			{
				textColors = _darkHue;
				backColors = _blackHue;
			}
			
			
			if(ratio <= 0.125)
			{
				Text = light > 0.5 ? '\u00B0' : '\u00B1';
				TextColor = textColors[hueCeil];
				BackColor = backColors[hueFloor];
			}
			else if(ratio >= 0.875)
			{
				Text = light > 0.5 ? '\u00B0' : '\u00B1';
				TextColor = textColors[hueCeil];
				BackColor = backColors[hueCeil];
			}
			else if(ratio < 0.375)
			{
				Text = '\u00B0';
				TextColor = textColors[hueCeil];
				BackColor = backColors[hueFloor];
			}
			else if(ratio > 0.625)
			{
				Text = '\u00B0';
				TextColor = textColors[hueFloor];
				BackColor = backColors[hueCeil];
			}
			else
			{
				Text = '\u00B1';
				TextColor = textColors[hueCeil];
				BackColor = backColors[hueFloor];
			}
		}
		
		public Stringel(char text, char textColor, char backColor)
		{
			Text = text;
			TextColor = textColor;
			BackColor = backColor;
		}
		public void WriteToConsole(Vector2 position)
		{
			ConsoleWriter.Write(new Rectangle(position, Vector2.ONE, false), new CharInfo[]{GetBuffer()});
		}
		
		public CharInfo GetBuffer()
		{
			return new CharInfo
			{
				Attributes = (short)((int)TextColor.ToColor() + ((int)BackColor.ToColor() << 4)),
				Char = new CharUnion{ UnicodeChar = Text }
			};
		}
		
		public void Print()
		{
			System.Console.BackgroundColor = BackColor.ToColor();
			System.Console.ForegroundColor = TextColor.ToColor();
			System.Console.Write(Text);
		}
		
		public static Line operator *(Stringel str, int length)
		{
			return new Line
			(
				str.Text.Times(length),
				str.TextColor.Times(length),
				str.BackColor.Times(length)
			);
		}
		public static Stringel operator *(Stringel c1, Stringel c2)
		{
			char textColor;
			char backColor;
			char text;
			Stringel c3 = new Stringel();
			
			//Base case check if c2 is transparent
			if(c2.TextColor == '?')
			{	
				if(c2.BackColor == '?')
				{
					return c1;
				}
			}
			else if(c1.TextColor == '?')
			{	
				if(c1.BackColor == '?')
				{
					return c2;
				}
			}
			//Finds the back color
			if(c2.BackColor == '?')
			{
				backColor=c1.BackColor;
			}
			else 
			{
			}
			
			//Finds the text Color
			
			//Finds the text
			text=c3.TextMulti(c1.Text,c2.Text);
			
			return c2;
		}
		
		// Function that takes in 2 char values and puts them together if possiable and returns the result
		// If not it returns the firsChar
		public char TextMulti(char firstChar, char SecondChar)
		{
			int y=0;
			int x=0;
			char[,] textMulti=
			{{'?','/','\\','|','_','-','P','A','E','F','(',')','V','D','c','.'},
			{'/','?','x','?','?','?','?','?','?','?','?','?','?','?','?','?'},
			{'\\','x','?','?','?','?','R','?','?','?','?','?','?','?','?','?'},
			{'|','?','?','?','L','+','?','?','?','?','?','D','?','?','d','i'},
			{'_','?','?','L','?','?','?','?','?','E','?','?','?','?','?','?'},
			{'-','?','?','+','?','?','?','?','?','?','?','?','?','Ð','?','?'},
			{'P','?','R','?','?','?','?','?','?','?','?','?','?','?','?','?'},
			{'A','?','?','?','?','?','?','?','Æ','?','?','?','?','?','?','?'},
			{'E','?','?','?','?','?','?','Æ','?','?','?','?','?','?','?','?'},
			{'F','?','?','?','E','?','?','?','?','?','?','?','?','?','?','?'},
			{'(','?','?','?','?','?','?','?','?','?','?','O','?','?','?','?'},
			{')','?','?','D','?','?','?','?','?','?','O','?','?','?','?','?'},
			{'V','?','?','?','?','?','?','?','?','?','?','?','W','?','?','?'},
			{'D','?','?','?','?','Ð','?','?','?','?','?','?','?','?','?','?'},
			{'c','?','?','d','?','?','?','?','?','?','?','?','?','?','?','?'},
			{'.','?','?','i','?','?','?','?','?','?','?','?','?','?','?',':'}};
			
			for(int i=1; i<16; i++)
			{
				if(firstChar == textMulti[0,i])
				{
					y=i;
				}
				
				if(firstChar == textMulti[i,0])
				{
					x=i;
				}
			}
			
			if((x!=0)&&(y!=0))
			{
				if(textMulti[x,y]!='?')
				{
					return textMulti[x,y];
				}
			}
			
			return firstChar;
		}
	}
}

