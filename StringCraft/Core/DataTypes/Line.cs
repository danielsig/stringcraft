using System;
using System.Text;
using System.Text.RegularExpressions;

namespace StringCraft
{
	//TODO gera unit test fyrir þennan struct
	public struct Line
	{
		private static readonly Regex textInvalid = new Regex("[\n\r\v\t]", RegexOptions.Compiled);
		private static readonly Regex colorsInvalid = new Regex("[^0-9 .+#rgbcmyRGBCMY?!@]", RegexOptions.Compiled);
		internal void Check(string text, string textColor, string backColor)
		{
			if(text == null) throw new NullReferenceException("text must be non-null");
			if(textColor == null) throw new NullReferenceException("textColor must be non-null");
			if(backColor == null) throw new NullReferenceException("backColor must be non-null");
			int len = text.Length;
			if(TextColor.Length != len)
				throw new ArgumentOutOfRangeException( "textColor", ErrorLength(text, textColor, backColor));
			else if(backColor.Length != len)
				throw new ArgumentOutOfRangeException( "backColor", ErrorLength(text, textColor, backColor));
			
			if(textInvalid.IsMatch(text))
				throw new ArgumentException("text", "text must not contain any whitespece characters besides ' '");
			if(colorsInvalid.IsMatch(textColor))
				throw new ArgumentException("textColor", "textColor must only contain any of these characters:\n0123456789 .+#rgbcmyRGBCMY\n");
			if(colorsInvalid.IsMatch(backColor))
				throw new ArgumentException("backColor", "backColor must only contain any of these characters:\n012345689 .+#rgbcmyRGBCMY\n");
		}
		public readonly string Text;
		/*
			0123 = grayscale 0 is black, 3 is white
				optional..
				# = black
				+ = dark gray
				. = light gray
				  = white (space)
			RGB = red, green and blue
			CMY = cyan, magenta and yellow
			
			rgb = dark- reg, green and blue
			cmy = dark- cyan, magenta and yellow
			
			?   = transparent
			!   = transparent white
			
			
			@   = transparent black
			
			456 = transparent red, green and blue
			789 = transparent cyan, magenta and yellow
		*/
		public readonly string TextColor;
		public readonly string BackColor;
		public int Length
		{
			get { return Text.Length; }
		}
		
		private string ErrorLength(string text, string textColor, string backColor)
		{
			return "text, textColors and backgroundColors must be of equal length, got:\n\t" + 
					"\n\t\ttext:\n\t\tValue: " + text + "\n\t\tLength: " + text.Length + 
					"\n\t\ttextColors:\n\t\tValue: " + textColor + "\n\t\tLength: " + textColor.Length + 
					"\n\t\tbackgroundColors:\n\t\tValue: " + backColor + "\n\t\tLength: " + backColor.Length;
		}
		public Line(string text, string textColor, string backColor)
		{
			Text = text;
			TextColor = textColor;
			BackColor = backColor;
			Check(text, textColor, backColor);
		}
		public Line(string text, string textColor, string backColor, int length)
		{
			Text = text.ToLength(length);
			TextColor = textColor.ToLength(length);
			BackColor = backColor.ToLength(length);
			Check(Text, TextColor, BackColor);
		}
		public Line(Stringel stringel, int length)
		{
			Text = stringel.Text.ToLength(length);
			TextColor = stringel.TextColor.ToLength(length);
			BackColor = stringel.BackColor.ToLength(length);
			Check(Text, TextColor, BackColor);
		}
		public Line(int[] colorCodes12Bit)
		: this(colorCodes12Bit, 0, colorCodes12Bit.Length)
		{
			/*int max = stringels.Length;
			StringBuilder text = new StringBuilder(max);
			StringBuilder textColor = new StringBuilder(max);
			StringBuilder backColor = new StringBuilder(max);
			
			int i = 0;
			foreach(int code in colorCodes12Bit)
			{
				double red = code >> 8;
				double green = code & 0x0F0 >> 4;
				double blue = code & 0x00F;
				Stringel str = new Stringel(red / 16, green / 16, blue / 16);
				text[i] = str.Text;
				textColor[i] = str.TextColor;
				backColor[i++] = str.BackColor;
			}
			Text = text.ToString();
			TextColor = textColor.ToString();
			BackColor = backColor.ToString();*/
		}
		public Line(int[] colorCodes12Bit, int index, int amount)
		{
			StringBuilder text = new StringBuilder(amount);
			StringBuilder textColor = new StringBuilder(amount);
			StringBuilder backColor = new StringBuilder(amount);
			
			backColor.Length = textColor.Length = text.Length = amount;
			
			for(int i = 0; i < amount; i++)
			{
				int code = colorCodes12Bit[i + index];
				double red = (code & 0xF00) >> 8;
				double green = (code & 0x0F0) >> 4;
				double blue = code & 0x00F;
				//Console.WriteLine("--------------");
				//Console.WriteLine(red + ", " + green + ", " + blue);
				Stringel str = new Stringel(red / 16, green / 16, blue / 16);
				text[i] = str.Text;
				textColor[i] = str.TextColor;
				backColor[i] = str.BackColor;
			}
			Text = text.ToString();
			TextColor = textColor.ToString();
			BackColor = backColor.ToString();
		}
		public Line(Stringel[] stringels)
		{
			int max = stringels.Length;
			StringBuilder text = new StringBuilder(max);
			StringBuilder textColor = new StringBuilder(max);
			StringBuilder backColor = new StringBuilder(max);
			
			backColor.Length = textColor.Length = text.Length = max;
			
			int i = 0;
			foreach(Stringel str in stringels)
			{
				text[i] = str.Text;
				textColor[i] = str.TextColor;
				backColor[i++] = str.BackColor;
			}
			Text = text.ToString();
			TextColor = textColor.ToString();
			BackColor = backColor.ToString();
		}
		public Line(int length) : this(Stringel.DEFAULT, length){}
		
		public static Symbol operator+(Line left, Line right)
		{
			return Symbol.DefineSymbol(new Line[2]{left, right}, true);
		}
		public static Line operator*(Line left, Line right)
		{
			int min;
			int max;
			min = max = left.Length;
			if(right.Length < min)
			{
				min = right.Length;
			}
			else max = right.Length;
			
			StringBuilder text = new StringBuilder(max);
			StringBuilder textColor = new StringBuilder(max);
			StringBuilder backColor = new StringBuilder(max);
			
			backColor.Length = textColor.Length = text.Length = max;
			
			for(int i = 0; i < min; i++)
			{
				Stringel str = left[i] * right[i];
				text[i] = str.Text;
				textColor[i] = str.TextColor;
				backColor[i] = str.BackColor;
				
			}
			return new Line
			(
				text.ToString(),
				textColor.ToString(),
				backColor.ToString()
			);
		}
		public static Line operator &(Line left, Line right)
		{
			return new Line
			(
				left.Text + right.Text,
				left.TextColor + right.TextColor,
				left.BackColor + right.BackColor
			);
		}
		public Symbol Prepend(Symbol symbol)
		{
			if(symbol.IsMutable)
			{
				Line[] lines = symbol.sc_lines;
				int height = lines.Length;
				
				if(symbol.Width >= Length)
					Array.Resize<Line>(ref lines, height + 1);
				else
					symbol.Size = new Vector2(Length, height + 1);
					
				for(int i = height; i > 0; i--)
				{
					lines[i] = lines[i - 1];
				}
				lines[0] = this;
				return symbol;
			}
			else throw new InvalidOperationException("Could not add Line:\n" + this + "\nto an immutable Symbol:\n" + symbol);
		}
		public Line Overwrite(Line other, int beginIndex)
		{
			int endIndex = Math.Min(Length, beginIndex + other.Length);
			int c = 0;
			
			if(beginIndex < 0)
			{
				c -= beginIndex;
				beginIndex = 0;
			}
			
			StringBuilder text = new StringBuilder(Text);
			StringBuilder textColor = new StringBuilder(TextColor);
			StringBuilder backColor = new StringBuilder(BackColor);
			
			for(int i = beginIndex; i < endIndex; i++)
			{
				Stringel str = this[i] * other[c++];
				text[i] = str.Text;
				textColor[i] = str.TextColor;
				backColor[i] = str.BackColor;
			}
			
			return new Line
			(
				text.ToString(),
				textColor.ToString(),
				backColor.ToString()
			);
		}
		public void WriteToConsole(Vector2 position)
		{
			ConsoleWriter.Write(new Rectangle(position, new Vector2(Length, 1), false), GetBuffer());
		}
		public CharInfo[] GetBuffer()
		{
			CharInfo[] buf = new CharInfo[Length];
			uint index = 0;
			WriteToBuffer(buf, ref index);
			return buf;
		}
		public void WriteToBuffer(CharInfo[] buf, ref uint index)
		{
			int len = Length;
			for(int i = 0; i < len; i++)
			{
				buf[index].Attributes = (short)((int)TextColor[i].ToColor() + ((int)BackColor[i].ToColor() << 4));
				buf[index++].Char.UnicodeChar = Text[i];
			}
		}
		public Stringel this[int stringelIndex]
		{
			get
			{
				if(stringelIndex < 0 || stringelIndex >= Length)
				{
					throw new IndexOutOfRangeException("stringelIndex " + stringelIndex + " is out of range " + Length);
				}
				return new Stringel(Text[stringelIndex], TextColor[stringelIndex], BackColor[stringelIndex]);
			}
		}
	}
}

