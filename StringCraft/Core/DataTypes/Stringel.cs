using System;
using System.Linq;

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
			
			if(TextColor == BackColor) Text = ' ';
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
			
			if(!IsTransparent(c2.BackColor))//is back color of c2 opaque
				return c2;
			
			//Base case check if c2 is transparent
			if((c2.TextColor == '?' || c2.Text == ' ') && c2.BackColor == '?')
				return c1;
			//Base case check if c1 is transparent
			if((c1.TextColor == '?' || c1.Text == ' ') && c1.BackColor == '?')
				return c2;
			
			backColor = MultiplyColor(c1.BackColor, c2.BackColor);
			
			if(IsTransparent(c2.TextColor) && c1.Text != ' ')
				textColor = MultiplyColor(MultiplyColor(c1.TextColor, c2.BackColor), c2.TextColor);
			else
				textColor = MultiplyColor(c1.BackColor, c2.TextColor);
			//Finds the text Color
			
			//Finds the text
			text = MultiplyText(c1.Text,c2.Text);
			
			return new Stringel(text, textColor, backColor);
		}
		
		// Function that takes in 2 char values and puts them together if possiable and returns the result
		// If not it returns the firsChar
		public static char MultiplyText(char firstChar, char secondChar)
		{
			int y=0;
			int x=0;
			
			if(firstChar == secondChar || secondChar == ' ') return firstChar;
			if(firstChar == ' ') return secondChar;
			
			if(firstChar == '\u00B0' || firstChar == '\u00B1') return firstChar;
			if(secondChar == '\u00B0' || secondChar == '\u00B1') return secondChar;
			
			string indexMap = "/\\|_-PAEF()VDc.,'";
			
			char[][] textMap =
			{
			//			 	 \   |   _   -   P   A   E   F   (   )   V   D   c   .   ,   '
			/*/*/new char[]{'X','/','<','f','P','A','Æ','F','f','V','W','8','c','L','/','7'},
			/*\*/new char[]{   '\\','q','+','R','A','E','E','6',')','W','&','c','>','\\','('},
			/*|*/new char[]{		'L','+','A','#','E','F','K','I','W','D','d','!','!','i'},
			/*_*/new char[]{			'=','E','B','E','E','6','J','U','D','c','_','_','i'},
			/*-*/new char[]{				'P','A','E','F','{','}','H','Ð','c','c','_','L'},
			/*P*/new char[]{					'A','R','P','P','P','8','B','B','R','P','R'},
			/*A*/new char[]{						'Æ','Æ','A','B','X','&','&','A','A','A'},
			/*E*/new char[]{							'E','A','B','P','B','6','E','E','6'},
			/*F*/new char[]{								'f','B','P','B','E','E','E','P'},
			/*(*/new char[]{									'0','V','0','C','(','(','('},
			/*)*/new char[]{										'V','D','d',')',')','7'},
			/*V*/new char[]{											'D','6','V','V','W'},
			/*D*/new char[]{												'E','D','D','D'},
			/*c*/new char[]{													'c','c','c'},
			/*.*/new char[]{														',',':'},
			/*,*/new char[]{															';'}
			};
			
			x = indexMap.IndexOf(firstChar);
			y = indexMap.IndexOf(secondChar);
			
			if(x < 0 || y < 0) return secondChar;
			
			if(y > x)
			{
				//SWAP		 50		3
				x = y - x;//-47		3
				y = y - x;//-47		50
				x = y + x;//  3		50
			}
			x -= (y + 1);
			return textMap[y][x];
		}
		
		public static char MultiplyColor(char firstColor, char secondColor)
		{
			int y=0;
			int x=0;
			string indexMap1A = "0123456789RGBCMYrgbcmy@!?";
			string indexMap1B = "#+. ";
			
			string indexMap2A = "#+. RGBCMYrgbcmy";
			string indexMap2B = "0123456789";
			string indexMap2C = "@ !";
			
			char[,] colorMap = new char[,]
			{
			//     0   1   2   3   4   5   6   7   8   9   R   G   B   C   M   Y   r   g   b   c   m   y   @   !   ?
			/*#*/{'0','1','2','3','r','g','b','c','m','y','R','G','B','C','M','Y','r','g','b','c','m','y','#','+','#'},
			/*+*/{'0','1','2','3','R','G','B','C','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y','#','.','+'},
			/*.*/{'0','1','2','3','R','G','C','C','M','Y','R','G','B','C','M','Y','r','g','b','c','m','y','+',' ','.'},
			/* */{'0','1','2','3','R','C','C',' ','R',' ','R','G','B','C','M','Y','r','g','b','c','m','y','.',' ',' '},
			/*R*/{'0','1','2','3','R','y','M','.','M','y','R','G','B','C','M','Y','r','g','b','c','m','y','r','y','R'},
			/*G*/{'0','1','2','3','Y','G','C','C','.','y','R','G','B','C','M','Y','r','g','b','c','m','y','g','C','G'},
			/*B*/{'0','1','2','3','M','C','B','c','M','G','R','G','B','C','M','Y','r','g','b','c','m','y','b','C','B'},
			/*C*/{'0','1','2','3','B','C','c','C','B',' ','R','G','B','C','M','Y','r','g','b','c','m','y','c',' ','C'},
			/*M*/{'0','1','2','3','R','.','b','+','M','R','R','G','B','C','M','Y','r','g','b','c','m','y','m','R','M'},
			/*Y*/{'0','1','2','3','y','G','g','G','R','Y','R','G','B','C','M','Y','r','g','b','c','m','y','y',' ','Y'},
			/*r*/{'0','1','2','3','R','y','m','b','m','R','R','G','B','C','M','Y','r','g','b','c','m','y','r','R','r'},
			/*g*/{'0','1','2','3','y','G','c','c','y','G','R','G','B','C','M','Y','r','g','b','c','m','y','g','G','g'},
			/*b*/{'0','1','2','3','m','c','B','c','m','g','R','G','B','C','M','Y','r','g','b','c','m','y','#','B','b'},
			/*c*/{'0','1','2','3','b','g','b','C','b','+','R','G','B','C','M','Y','r','g','b','c','m','y','c','C','c'},
			/*m*/{'0','1','2','3','r','+','b','+','M','r','R','G','B','C','M','Y','r','g','b','c','m','y','m','M','m'},
			/*y*/{'0','1','2','3','y','g','g','g','r','Y','R','G','B','C','M','Y','r','g','b','c','m','y','y','Y','y'}
			};
			
			y = indexMap1A.IndexOf(secondColor);
			if(y < 0) y = indexMap1B.IndexOf(secondColor);
			if(y < 0) y = 0;
			
			x = indexMap2A.IndexOf(firstColor);
			if(x < 0) x = indexMap2B.IndexOf(firstColor);
			if(x < 0) x = indexMap2C.IndexOf(firstColor);
			if(x < 0) x = 0;
			
			return colorMap[x,y];
		}
		
		public static bool IsTransparent(char color)
		{
			string transparent = "456789!@?";
			return transparent.IndexOf(color) != -1;
		}
	}
}

