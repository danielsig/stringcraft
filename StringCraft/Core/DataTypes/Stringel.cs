using System;

namespace StringCraft
{
	
	public struct Stringel
	{
		public static readonly Stringel DEFAULT = new Stringel(' ', ' ', '?');
		
		public readonly char Text;
		public readonly char TextColor;
		public readonly char BackColor;
		
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
			return c2;
		}
	}
}

