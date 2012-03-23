using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace StringCraft
{
	//TODO gera unit test fyrir þennan clasa
	public sealed class Symbol
	{
		public readonly static Symbol EMPTY = DefineSymbol("EMPTY", new Line[]{new Line(" ", " ", "#")});
		internal Line[] sc_lines = {new Line(1)};
		
		public string Name{ get; private set; }
		public bool IsMutable{ get; private set; }
		public Vector2 Size
		{
			get { return new Vector2(sc_lines[0].Length, sc_lines.Length); }
			set
			{
				if(IsMutable)
				{
					value.Min(1, 1);
					if(value.Y != sc_lines.Length) Array.Resize<Line>(ref sc_lines, value.Y);
					
					if(sc_lines[0].Length > value.X)
					{
						int numLines = sc_lines.Length;
						for(int i = 0; i < numLines; i++)
						{
							Line line = sc_lines[i];
							sc_lines[i] = new Line
							(
								line.Text.Substring(0, value.X),
								line.TextColor.Substring(0, value.X),
								line.BackColor.Substring(0, value.X)
							);
						}
					}
					else
					{
						int addLen = value.X - sc_lines[0].Length;
						String space = " ".Times(addLen);
						String transparent = "?".Times(addLen);
						
						int numLines = sc_lines.Length;
						for(int i = 0; i < numLines; i++)
						{
							Line line = sc_lines[i];
							sc_lines[i] = new Line
							(
								line.Text + space,
								line.TextColor + space,
								line.BackColor + transparent
							);
						}
					}
				}
				else throw new InvalidOperationException("Could not change the Size of an immutable Symbol");
			}
		}
		public int Width
		{
			get { return sc_lines[0].Length; }
			set
			{
				Size = new Vector2(value, sc_lines.Length);
			}
		}
		public int Height
		{
			get { return sc_lines.Length; }
			set
			{
				Size = new Vector2(sc_lines[0].Length, value);
			}
		}
		
		public Line this[int lineIndex]
		{
			get
			{
				return sc_lines[lineIndex];
			}
			set
			{
				if(IsMutable)
				{
					if(lineIndex < 0 || lineIndex > Height)
						throw new IndexOutOfRangeException("lineIndex " + lineIndex + " is out of range " + Height);
					
					if(value.Length > Size.X)
						Width = value.Length;
					else if(value.Length < Size.X)
						value &= (Stringel.DEFAULT * (Size.X - value.Length));
					sc_lines[lineIndex] = value;
				}
				else throw new InvalidOperationException("Could not change a line of an immutable Symbol");
			}
		}
		public Line Remove(int lineIndex)
		{
			if(IsMutable)
			{
				int height = sc_lines.Length;
				if(lineIndex < 0 || lineIndex >= height)
					throw new ArgumentOutOfRangeException("lineIndex", "lineIndex is out of range");
				
				
				Line lineToRemove = sc_lines[lineIndex];
				while(++lineIndex < height)
				{
					sc_lines[lineIndex - 1] = sc_lines[lineIndex];
				}
				Height--;
				return lineToRemove;
			}
			else throw new InvalidOperationException("Could not remove a line from an immutable Symbol");
		}
		public Symbol Append(Line line, bool trimToFit = true)
		{
			if(IsMutable)
			{	
				int width = Width;
				int lineLength = line.Length;
				if(lineLength < width)
					line &= new Line(width - lineLength);
				if(lineLength > width)
				{
					if(trimToFit)
						line.Slice(0, width);
					else
						width = lineLength;
				}
					
				Size = new Vector2(width, Height + 1);
				return this;
			}
			else throw new InvalidOperationException("Could not add Line:\n" + line + "\nto an immutable Symbol:\n" + this);
		}
		public Symbol Overwrite(Symbol other, Vector2 relativePosition)
		{
			if(IsMutable)
			{
				int c = 0;
				int start = relativePosition.Y;
				if(start < 0)
				{
					c -= relativePosition.Y;
					start = 0;
				}
				int end = Math.Min(sc_lines.Length, relativePosition.Y + other.sc_lines.Length);
				for(int i = start; i < end; i++)
				{
					sc_lines[i] = sc_lines[i].Overwrite(other[c++], relativePosition.X);
				}
				return this;
			}
			else throw new InvalidOperationException("Could not write Symbol:\n" + other + "\non to an immutable Symbol:\n" + this);
		}
		public void WriteToConsole(Vector2 position)
		{
			ConsoleWriter.Write(new Rectangle(Anchor.TopLeft, position, Size, false), GetBuffer());
		}
		public CharInfo[] GetBuffer()
		{
			CharInfo[] buf = new CharInfo[Size.Area];
			uint index = 0;
			foreach (Line line in sc_lines)
			{
				line.WriteToBuffer(buf, ref index);
			}
			return buf;
		}
		private Symbol(string name, Line[] lines, bool mutable)
		{
			if(sc_lines.Length < 1)
			{
				throw new ArgumentOutOfRangeException("lines.Length", "Number of lines must be greater than 0");
			}
			if(sc_lines[0].Text != null && sc_lines[0].Length < 1)
			{
				throw new ArgumentOutOfRangeException("lines[0].Length", "Number of characters per line must be greater than 0");
			}
			
			Name = name;
			IsMutable = mutable;
			sc_lines = lines;
		}
		private Symbol()
		{
			
		}
		
		public static Symbol CreateBuffer(Vector2 bufferSize, char backgroundColor = '#')
		{
			return CreateBuffer(bufferSize.X, bufferSize.Y, backgroundColor);
		}
		public static Symbol CreateBuffer(int width, int height, char backgroundColor = '#')
		{
			Symbol s = new Symbol();
			/*
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(s.Size);
			Console.WriteLine(s.ToString());
			Console.WriteLine(width + ", " + height);
			*/
			if(height < 1)
			{
				throw new ArgumentOutOfRangeException("height", "Number of lines must be greater than 0");
			}
			if(width < 1)
			{
				throw new ArgumentOutOfRangeException("width", "Number of characters per line must be greater than 0");
			}
			
			s.Name = "";
			s.IsMutable = true;
			s.sc_lines = new Line[height];
			
			Line emptyLine = new Line(" ", " ", backgroundColor + "", width);
			
			for(int i = 0; i < height; i++)
			{
				s.sc_lines[i] = emptyLine;
			}
			return s;
		}
		public static Symbol DefineSymbol(string name, char textColor, char backColor, bool mutable = false, params string[] linesText)
		{
			if(linesText.Length < 1)
			{
				throw new ArgumentOutOfRangeException("linesText", "linesText must have a length of at least 1");
			}
			string lineTextColor = textColor.Times(linesText[0].Length);
			string lineBackColor = backColor.Times(linesText[0].Length);
			Line[] lines = new Line[linesText.Length];
			int c = 0;
			foreach(string line in linesText)
			{
				lines[c++] = new Line(line, lineTextColor, lineBackColor);
			}
			return DefineSymbol(name, lines, mutable);
		}
		public static Symbol DefineSymbol(string name, bool mutable, int width, params int[] colorCodes12Bit)
		{
			int height = colorCodes12Bit.Length / width;
			Line[] linesArr = new Line[height];
			for(int i = 0; i < height; i++)
			{
				linesArr[i] = new Line(colorCodes12Bit, i * width, width);
			}
			return DefineSymbol(name, linesArr, mutable);
		}
		/**
		 * every 3 strings in the optional params "lines" represents a line.
		 * where the first string is the text
		 * the second is the textColor
		 * and the third is the backColor
		 * 
		 * like this:
		 * text, textColor, backColor,
		 * text, textColor, backColor,
		 * text, textColor, backColor
		 * etc.
		 * 
		 * here's an example:
		 
		  	Symbol.DefineSymbol
		 	( "R3D2", false,
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
			
		 */
		public static Symbol DefineSymbol(string name, bool mutable, params string[] lines)
		{
			if(lines.Length < 3)
			{
				throw new ArgumentOutOfRangeException("lines", "lines must have a length of at least 3");
			}
			if(lines.Length % 3 != 0)
			{
				throw new ArgumentOutOfRangeException("lines", "lines must have a length that is a multiple of 3");
			}
			Line[] linesArr = new Line[lines.Length / 3];
			for(int i = 0; i < linesArr.Length; i++)
			{
				int index = i * 3;
				linesArr[i] = new Line(lines[index], lines[index + 1], lines[index + 2]);
			}
			return DefineSymbol(name, linesArr, mutable);
		}
		public static Symbol DefineSymbol(Line[] lines, bool mutable = false)
		{
			String name = "autoGenerated" + _symbolCounter++;
			return _symbols[name] = new Symbol(name, lines, mutable);
		}
		public static Symbol DefineSymbol(string name, Line[] lines, bool mutable = false)
		{
			_symbols = _symbols ?? new Dictionary<string, Symbol>();
			if(_symbols.ContainsKey(name))
			{
				throw new ArgumentOutOfRangeException("name", "symbol with name " + name + " already exists.");
			}
			return _symbols[name] = new Symbol(name, lines, mutable);
		}
		public static Symbol GetSymbol(string name)
		{
			if(!_symbols.ContainsKey(name))
			{
				throw new ArgumentOutOfRangeException("name", "symbol with name " + name + " doesn't exist.");
			}
			return _symbols[name];
		}
		public static void DuplicateSymbol(string originalName, string duplicateName)
		{
			Symbol original = GetSymbol(originalName);
			DefineSymbol(duplicateName, original.sc_lines, original.IsMutable);
		}
		public static void DuplicateSymbol(string originalName, string duplicateName, bool duplicateMutable)
		{
			Symbol original = GetSymbol(originalName);
			DefineSymbol(duplicateName, original.sc_lines, duplicateMutable);
		}
		
		private static Dictionary<string, Symbol> _symbols = new Dictionary<string, Symbol>();
		private static long _symbolCounter = 0;
		
		public override String ToString()
		{
			StringBuilder builder = new StringBuilder(Size.Area);
			
			foreach(Line line in sc_lines)
			{
				builder.AppendLine(line.Text);
			}
			
			return Name + ":\n" + builder.ToString();
		}
	}
}

