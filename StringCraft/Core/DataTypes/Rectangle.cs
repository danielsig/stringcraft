using System;

namespace StringCraft
{
	/*
		The position of the local center relatice to the symbol
	*/
	public enum Anchor
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
		Center,
		Top,
		Bottom,
		Left,
		Right
	}
	//TODO gera unit test fyrir þennan struct
	public struct Rectangle
	{
		public static readonly Rectangle NULL = new Rectangle{sc_topLeft = Vector2.ZERO, sc_size = Vector2.ZERO, sc_mutable = false };
		
		internal Vector2 sc_topLeft;
		internal Vector2 sc_size;
		internal bool sc_mutable;
		
		public Rectangle(bool mutable = true)
		{
			sc_topLeft = Vector2.ZERO;
			sc_size = Vector2.ZERO;
			sc_mutable = mutable;
		}
		public Rectangle(Anchor anchor, Vector2 position, Vector2 size, bool mutable = false)
		: this(anchor, position.X, position.Y, size.X, size.Y, mutable){}
		public Rectangle(Anchor anchor, int x, int y, int width, int height, bool mutable = false)
		{
			sc_size = new Vector2(width, height);
			switch(anchor)
			{
				case Anchor.TopLeft:
					sc_topLeft = new Vector2(x, y);
					break;
				case Anchor.Center:
					sc_topLeft = new Vector2(x - (width >> 1), y - (height >> 1));
					break;
				case Anchor.BottomRight:
					sc_topLeft = new Vector2(x - width, y - height);
					break;
				case Anchor.Top:
					sc_topLeft = new Vector2(x - (width >> 1), y);
					break;
				case Anchor.Left:
					sc_topLeft = new Vector2(x, y - (height >> 1));
					break;
				case Anchor.TopRight:
					sc_topLeft = new Vector2(x - width, y);
					break;
				case Anchor.BottomLeft:
					sc_topLeft = new Vector2(x, y - height);
					break;
				case Anchor.Bottom:
					sc_topLeft = new Vector2(x - (width >> 1), y - height);
					break;
				case Anchor.Right:
					sc_topLeft = new Vector2(x - width, y - (height >> 1));
					break;
				default:
					sc_topLeft = new Vector2(x, y);
					break;
			}
			sc_mutable = mutable;
			ValidateRectangle();
		}
		public Rectangle(Vector2 topLeft, Vector2 bottomRight, bool mutable = false)
		: this(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y, mutable){}
		public Rectangle(int left, int right, int top, int bottom, bool mutable = false)
		{
			sc_topLeft = new Vector2(left, top);
			sc_size = new Vector2(right - left, bottom - top);
			sc_mutable = mutable;
			ValidateRectangle();
		}
		public int Area
		{
			get
			{
				return sc_size.Area;
			}
		}
		public Vector2 TopLeft
		{
			get { return new Vector2(sc_topLeft.X, sc_topLeft.Y); }
			set
			{
				CheckAccess();
				sc_size.X = (sc_topLeft.X + sc_size.X) - value.X;
				sc_size.Y = (sc_topLeft.Y + sc_size.Y) - value.Y;
				sc_topLeft.X = value.X;
				sc_topLeft.Y = value.Y;
				
				ValidateRectangle();
			}
		}
		public Vector2 Size
		{
			get { return new Vector2(sc_size.X, sc_size.Y); }
			set
			{
				CheckAccess();
				
				sc_size = value;
				
				ValidateRectangle();
			}
		}
		public Vector2 SizeCenter
		{
			get { return new Vector2(sc_size.X, sc_size.Y); }
			set
			{
				CheckAccess();
				
				sc_topLeft.X = sc_topLeft.X + ((sc_size.X - value.X) >> 1);
				sc_topLeft.Y = sc_topLeft.Y + ((sc_size.Y - value.Y) >> 1);
				
				sc_size = value;
				
				ValidateRectangle();
			}
		}
		public Vector2 TopRight
		{
			get { return new Vector2(sc_topLeft.X + sc_size.X, sc_topLeft.Y); }
			set
			{
				CheckAccess();
				sc_size.X = value.X - sc_topLeft.X;
				sc_topLeft.Y = value.Y;
				ValidateRectangle();
			}
		}
		public Vector2 BottomLeft
		{
			get { return new Vector2(sc_topLeft.X, sc_topLeft.Y + sc_size.Y); }
			set
			{
				CheckAccess();
				sc_topLeft.X = value.X;
				sc_size.Y = value.Y - sc_topLeft.Y;
				ValidateRectangle();
			}
		}
		public Vector2 BottomRight
		{
			get { return new Vector2(sc_topLeft.X + sc_size.X, sc_topLeft.Y + sc_size.Y); }
			set
			{
				CheckAccess();
				sc_size.X = value.X - sc_topLeft.X;
				sc_size.Y = value.Y - sc_topLeft.Y;
				ValidateRectangle();
			}
		}
		public Vector2 Center
		{
			get { return new Vector2(sc_topLeft.X + (sc_size.X >> 1), sc_topLeft.Y + (sc_size.Y >> 1)); }
			set
			{
				CheckAccess();
				sc_topLeft.X = value.X - (sc_size.X >> 1);
				sc_topLeft.Y = value.Y - (sc_size.Y >> 1);
				ValidateRectangle();
			}
		}
		public int Left
		{
			get { return sc_topLeft.X; }
			set
			{
				CheckAccess();
				sc_topLeft.X = value;
				ValidateRectangle();
			}
		}
		public int Right
		{
			get { return sc_topLeft.X + sc_size.X; }
			set
			{
				CheckAccess();
				sc_size.X = value - sc_topLeft.X;
				ValidateRectangle();
			}
		}
		public int Top
		{
			get { return sc_topLeft.Y; }
			set
			{
				CheckAccess();
				sc_topLeft.Y = value;
				ValidateRectangle();
			}
		}
		public int Bottom
		{
			get { return sc_topLeft.Y + sc_size.Y; }
			set
			{
				CheckAccess();
				sc_size.Y = value - sc_topLeft.Y;
				ValidateRectangle();
			}
		}
		public bool IsMutable
		{
			get { return sc_mutable; }
		}
		public bool IsImmutable
		{
			get { return !sc_mutable; }
		}
		
		private void CheckAccess()
		{
			if(!sc_mutable) throw new InvalidOperationException("Rectangle is immutable!");
		}
		private void ValidateRectangle()
		{
			if(sc_size.X < 1 || sc_size.Y < 1) throw new ArgumentOutOfRangeException("Size", "The Width and Height of a Rectangle must be greater than 0");
		}
		public static Rectangle operator &(Rectangle a, Rectangle b)
		{
			int left = Math.Max(a.Left, b.Left);
			int right = Math.Min(a.Right, b.Right);
			if(left < right)
			{
				int top = Math.Max(a.Top, b.Top);
				int bottom = Math.Min(a.Bottom, b.Bottom);
				if(top < bottom)
				{
					return new Rectangle(left, right, top, bottom, a.sc_mutable || b.sc_mutable);
				}
			}
			return NULL;
		}
		public static Rectangle operator |(Rectangle a, Rectangle b)
		{
			int left = Math.Min(a.Left, b.Left);
			int right = Math.Max(a.Right, b.Right);
			int top = Math.Min(a.Top, b.Top);
			int bottom = Math.Max(a.Bottom, b.Bottom);
			return new Rectangle(left, right, top, bottom, a.sc_mutable || b.sc_mutable);
		}
		public static bool operator ^(Rectangle a, Rectangle b)
		{
			if(a.Left > b.Right || b.Left > a.Right) return false;
			if(a.Top > b.Bottom || b.Top > a.Bottom) return false;
			return true;
		}
		public static bool operator >(Rectangle a, Rectangle b)
		{
			return a.sc_topLeft > b && a.BottomRight > b;
		}
		public static bool operator <(Rectangle b, Rectangle a)
		{
			return a.sc_topLeft > b && a.BottomRight > b;
		}
		public static bool operator <(Rectangle a, Vector2 b)
		{
			Vector2 c = b - a.sc_topLeft;
			return c.X >= 0 && c.X < a.sc_size.X
				&& c.Y >= 0 && c.Y < a.sc_size.Y;
		}
		public static bool operator >(Rectangle a, Vector2 b)
		{
			return false;
		}
		public static bool operator <(Vector2 b, Rectangle a)
		{
			return false;
		}
		public static bool operator >(Vector2 b, Rectangle a)
		{
			Vector2 c = b - a.sc_topLeft;
			return c.X >= 0 && c.X < a.sc_size.X
				&& c.Y >= 0 && c.Y < a.sc_size.Y;
		}
		public static Rectangle operator +(Rectangle a, Vector2 b)
		{
			return new Rectangle{ sc_topLeft = a.sc_topLeft + b, sc_size = a.sc_size, sc_mutable = a.sc_mutable };
		}
		public static Rectangle operator -(Rectangle a, Vector2 b)
		{
			return new Rectangle{ sc_topLeft = a.sc_topLeft - b, sc_size = a.sc_size, sc_mutable = a.sc_mutable };
		}
		public static implicit operator bool(Rectangle rect)
		{
			return rect.sc_size.X != 0;
		}
		public override string ToString()
		{
			return "[ left: " + Left + ", Right: " + Right + ", Top: " + Top + ", Bottom: " + Bottom + " ]";
		}
	}
}

