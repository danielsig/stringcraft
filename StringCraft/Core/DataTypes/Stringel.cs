using System;

namespace StringCraft
{
	
	public struct Stringel
	{
		public static readonly Stringel DEFAULT = new Stringel(' ', ' ', '?');
		
		public readonly char Text;
		public readonly char TextColor;
		public readonly char BackColor;
		
		/**
		 * List of all possable outcomes for * operator
		 * */
		
		//Outchar 
		
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

