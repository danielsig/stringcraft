using System;

namespace StringCraft
{
	
	public struct Stringel
	{
		public static readonly Stringel DEFAULT = new Stringel(' ', ' ', '?');
		private static readonly char[] _lightHue =	new char[]{'.',' ',' ',' ','C','.','.'};
		private static readonly char[] _hue =		new char[]{'R','Y','G','C','B','M','R'};
		private static readonly char[] _darkHue =	new char[]{'r','y','g','c','b','m','r'};
		private static readonly char[] _blackHue =	new char[]{'#','+','#','#','#','#','#'};
		
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
			
			if(blue < 0.001)	return 				(red > green	? green	/ 6 : (2 - red)		/ 6);
			if(red < 0.001)	return (1.0/3) +	(green > blue	? blue	/ 6 : (2 - green)	/ 6);
			if(green < 0.001)return (2.0/3) +	(blue > red		? red	/ 6 : (2 - blue)	/ 6);
			return 0.0111144444;
			/*double invMax = 0.333333333333333 / Math.Max(Math.Max(red, green), blue);
			if(green > blue)
				return green * invMax + blue * invMax * 2;
			else
				return green * invMax + blue * invMax * 2 + red * invMax * 3;*/
		}
		private static double Lightness(double red, double green, double blue)
		{
			if(red * green + green * blue + blue * red < 0.66666)
				return (red + green + blue) * 0.63 - (red * green + green * blue + blue * red) * 0.6;
			else
				return (red + green + blue) * 0.33333;
			/*
			return Math.Sqrt
			(
  				red * red * 0.35 + 
  				green * green * 0.37 + 
  				blue * blue * 0.45
			) * 1.05 - (2 - (red * green + green * blue + blue * red)) * 0.1;
			*/
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
				else if(light > 0.7)//0.75 - 0.875
				{
					textColors = _hue;
					backColors = _lightHue;
				}
				else if(light > 0.55)//0.6 - 0.85
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
			
			if(!c3.Istransparent(c2.BackColor))
			{
				return c2;
			}
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
			
			backColor = c3.ColorMulti(c1.BackColor, c2.BackColor);
			
			if(c3.Istransparent(c2.TextColor))
			{
				textColor=c3.ColorMulti(c1.TextColor,c2.TextColor);
			}
			else
			{
				textColor=c2.TextColor;
			}
			//Finds the text Color
			
			//Finds the text
			text=c3.TextMulti(c1.Text,c2.Text);
			
			return new Stringel(text, textColor, backColor);
		}
		
		// Function that takes in 2 char values and puts them together if possiable and returns the result
		// If not it returns the firsChar
		public char TextMulti(char firstChar, char secondChar)
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
				
				if(secondChar == textMulti[i,0])
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
			
			return secondChar;
		}
		
		public char ColorMulti(char firstColor, char secondColor)
		{
			int y=0;
			int x=0;
			char[,] colorMulti=
			{{'?','0','1','2','3','4','5','6','7','8','9','R','G','B','C','M','Y','r','g','b','c','m','y','!','@','#','+','.',' ','?'},
			{'0','0','1','2','3','r','g','b','c','m','y','R','G','B','C','M','Y','r','g','b','c','m','y',' ','+','#','+','.',' ','0'},
			{'1','0','1','2','3','r','g','b','c','m','y','R','G','B','C','M','Y','r','g','b','c','m','y','.','+','#','+','.',' ','1'},
			{'2','0','1','2','3','R','G','B','C','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y','.','+','#','+','.',' ','2'},
			{'3','0','1','2','3','R','G','B','C','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y','+','#','#','+','.',' ','3'},
			{'4','r','r','R','R','R','Y','M','B','R','y','R','Y','M','B','R','y','r','y','m','b','r','y','R','r','r','r','R','R','4'},
			{'5','g','g','G','G','Y','G','C','C','G','G','Y','G','C','C','G','G','y','g','c','c','g','g','G','g','g','g','G','G','5'},
			{'6','b','b','B','B','M','C','B','B','b','G','M','C','B','B','b','G','m','c','b','b','b','g','B','b','b','b','B','B','6'},
			{'7','c','c','C','C','B','C','B','C','M','G','B','Y','B','C','M','G','b','y','b','c','m','g','C','c','c','c','C','C','7'},
			{'8','m','m','M','M','R','G','b','M','M','M','R','Y','M','B','M','M','r','y','m','b','m','m','M','m','m','m','M','M','8'},
			{'9','y','y','Y','Y','y','G','G','G','M','Y','R','G','G','G','M','Y','r','g','g','g','m','y','Y','y','y','y','Y','Y','9'},
			{'R','0','1','2','3','R','Y','M','B','R','R','R','G','B','C','M','Y','r','g','b','c','m','y','R','r','#','+','.',' ','R'},
			{'G','0','1','2','3','Y','G','C','Y','Y','G','R','G','B','C','M','Y','r','g','b','c','m','y','G','g','#','+','.',' ','G'},
			{'B','0','1','2','3','M','C','B','B','M','G','R','G','B','C','M','Y','r','g','b','c','m','y','B','b','#','+','.',' ','B'},
			{'C','0','1','2','3','B','C','B','C','B','G','R','G','B','C','M','Y','r','g','b','c','m','y','C','c','#','+','.',' ','C'},
			{'M','0','1','2','3','R','G','b','M','M','M','R','G','B','C','M','Y','r','g','b','c','m','y','M','m','#','+','.',' ','M'},
			{'Y','0','1','2','3','y','G','G','G','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y','Y','y','#','+','.',' ','Y'},
			{'r','0','1','2','3','r','y','m','b','r','r','R','G','B','C','M','Y','r','g','b','c','m','y','R','r','#','+','.',' ','r'},
			{'g','0','1','2','3','y','g','c','y','y','g','R','G','B','C','M','Y','r','g','b','c','m','y','G','g','#','+','.',' ','g'},
			{'b','0','1','2','3','m','c','b','b','m','g','R','G','B','C','M','Y','r','g','b','c','m','y','B','b','#','+','.',' ','b'},
			{'c','0','1','2','3','b','c','b','c','b','g','R','G','B','C','M','Y','r','g','b','c','m','y','C','c','#','+','.',' ','c'},
			{'m','0','1','2','3','r','g','b','m','m','m','R','G','B','C','M','Y','r','g','b','c','m','y','M','m','#','+','.',' ','m'},
			{'y','0','1','2','3','y','g','g','g','m','y','R','G','B','C','M','Y','r','g','b','c','m','y','Y','y','#','+','.',' ','y'},
			{'!',' ','.','.','+','R','G','B','C','M','Y','R','G','B','C','M','Y','R','G','B','C','M','Y',' ','#','+','.','.',' ','!'},
			{'@','+','+','+','#','r','g','b','c','m','y','r','g','b','c','m','y','r','g','b','c','m','y','+','#','#','+','+','+','@'},
			{'#','0','1','2','3','r','g','b','c','m','y','R','G','B','C','M','Y','r','g','b','c','m','y','+','#','#','+','.',' ','#'},
			{'+','0','1','2','3','r','g','b','c','m','y','R','G','B','C','M','Y','r','g','b','c','m','y','+','+','#','+','.',' ','+'},
			{'.','0','1','2','3','R','G','B','C','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y','.','+','#','+','.',' ','.'},
			{' ','0','1','2','3','R','G','B','C','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y',' ','+','#','+','.',' ',' '},
			{'?','0','1','2','3','4','5','6','7','8','9','R','G','B','C','M','Y','r','g','b','c','m','y','!','@','#','+','.',' ','?'}};
			
			for(int i=1; i<29; i++)
			{
				if(firstColor == colorMulti[0,i])
				{
					y=i;
				}
				
				if(secondColor == colorMulti[i,0])
				{
					x=i;
				}
			}
			
			if((x!=0)&&(y!=0))
			{
				return colorMulti[x,y];	
			}
			
			return secondColor;
		}
		
		public bool Istransparent(char color)
		{
			char[] transparent = {'4','5','6','7','8','9','!','@'};
			foreach(var n in transparent)
			{
				if(n==color)
				{
					return true;
				}
			}
			
			return false;
		}
	}
}

