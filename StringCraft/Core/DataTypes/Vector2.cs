using System;
using System.Runtime.InteropServices;

namespace StringCraft
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2
	{
		#region properties
		public int X;
		public int Y;
		public int Area
		{
			get
			{
				return X * Y;
			}
		}
		public int Magnitude
		{
			get
			{
				return RoundToInt(Math.Sqrt((double)(X * X + Y * Y)));
			}
		}
		public double DoubleMagnitude
		{
			get
			{
				return Math.Sqrt((double)(X * X + Y * Y));
			}
		}
		public int SqrMagnitude
		{
			get
			{
				return X * X + Y * Y;
			}
		}
		public double DoubleSqrMagnitude
		{
			get
			{
				return X * X + Y * Y;
			}
		}
		public Vector2 Rotated
		{
			get
			{
				return new Vector2(-Y, X);
			}
		}
		public Vector2 RotatedCCW
		{
			get
			{
				return new Vector2(Y, -X);
			}
		}
		public Vector2 FlippedX
		{
			get
			{
				return new Vector2(-X, Y);
			}
		}
		public Vector2 FlippedY
		{
			get
			{
				return new Vector2(X, -Y);
			}
		}
		#endregion
		#region constructs
		public Vector2(int x, int y)
		{
			X = x;
			Y = y;
		}
		#endregion
		#region constants
		public readonly static Vector2 ZERO = new Vector2(0, 0);
		public readonly static Vector2 ONE = new Vector2(1, 1);
		public readonly static Vector2 LEFT = new Vector2(-1, 0);
		public readonly static Vector2 RIGHT = new Vector2(1, 0);
		public readonly static Vector2 UP = new Vector2(0, -1);
		public readonly static Vector2 DOWN = new Vector2(0, 1);
		#endregion
		
		#region operators
		
		#region unary
		public static Vector2 operator -(Vector2 operand)
		{
			operand.X = -operand.X;
			operand.Y = -operand.X;
			return operand;
		}
		public static bool operator true(Vector2 operand)
		{
			return operand.X != 0 || operand.Y != 0;
		}
		public static bool operator false(Vector2 operand)
		{
			return operand.X == 0 && operand.Y == 0;
		}
		#endregion
		
		#region binary
		public static Vector2 operator +(Vector2 left, Vector2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}
		public static Vector2 operator -(Vector2 left, Vector2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}
		public static int operator &(Vector2 left, Vector2 right)//dot product
		{
			return left.X * right.X + left.Y * right.Y;
		}
		public static int operator %(Vector2 left, Vector2 right)//distance
		{
			left -= right;
			return left.Magnitude;
		}
		#region multiplication
		#region vector * number
		public static Vector2 operator *(Vector2 left, Vector2 right)
		{
			left.X = left.X * right.X;
			left.Y = left.Y * right.Y;
			return left;
		}
		public static Vector2 operator *(Vector2 left, short right)
		{
			left.X *= right;
			left.Y *= right;
			return left;
		}
		public static Vector2 operator *(Vector2 left, int right)
		{
			left.X *= right;
			left.Y *= right;
			return left;
		}
		public static Vector2 operator *(Vector2 left, long right)
		{
			left.X *= (int)right;
			left.Y *= (int)right;
			return left;
		}
		public static Vector2 operator *(Vector2 left, float right)
		{
			left.X = RoundToInt(left.X * right);
			left.Y = RoundToInt(left.Y * right);
			return left;
		}
		public static Vector2 operator *(Vector2 left, double right)
		{
			left.X = RoundToInt(left.X * right);
			left.Y = RoundToInt(left.Y * right);
			return left;
		}
		#endregion
		#region number * vector
		public static Vector2 operator *(short left, Vector2 right)
		{
			right.X *= left;
			right.Y *= left;
			return right;
		}
		public static Vector2 operator *(int left, Vector2 right)
		{
			right.X *= left;
			right.Y *= left;
			return right;
		}
		public static Vector2 operator *(long left, Vector2 right)
		{
			right.X *= (int)left;
			right.Y *= (int)left;
			return right;
		}
		public static Vector2 operator *(float left, Vector2 right)
		{
			right.X = RoundToInt(right.X * left);
			right.Y = RoundToInt(right.Y * left);
			return right;
		}
		public static Vector2 operator *(double left, Vector2 right)
		{
			right.X = RoundToInt(right.X * left);
			right.Y = RoundToInt(right.Y * left);
			return right;
		}
		#endregion
		#endregion
		
		#region division
		public static Vector2 operator /(Vector2 left, Vector2 right)
		{
			left.X = left.X / right.X;
			left.Y = left.Y / right.Y;
			return left;
		}
		public static Vector2 operator /(Vector2 left, short right)
		{
			left.X = RoundToInt(left.X / (double)right);
			left.Y = RoundToInt(left.Y / (double)right);
			return left;
		}
		public static Vector2 operator /(Vector2 left, int right)
		{
			left.X = RoundToInt(left.X / (double)right);
			left.Y = RoundToInt(left.Y / (double)right);
			return left;
		}
		public static Vector2 operator /(Vector2 left, long right)
		{
			left.X = RoundToInt(left.X / (double)right);
			left.Y = RoundToInt(left.Y / (double)right);
			return left;
		}
		public static Vector2 operator /(Vector2 left, float right)
		{
			left.X = RoundToInt(left.X / right);
			left.Y = RoundToInt(left.Y / right);
			return left;
		}
		public static Vector2 operator /(Vector2 left, double right)
		{
			left.X = RoundToInt(left.X / right);
			left.Y = RoundToInt(left.Y / right);
			return left;
		}
		#endregion
		#endregion
		
		#region comarisons
		public static bool operator >(Vector2 left, Vector2 right)
		{
			left.X *= left.X;
			left.Y *= left.Y;
			right.X *= right.X;
			right.Y *= right.Y;
			return left.X + left.Y > right.X + right.Y;
		}
		public static bool operator <(Vector2 left, Vector2 right)
		{
			left.X *= left.X;
			left.Y *= left.Y;
			right.X *= right.X;
			right.Y *= right.Y;
			return left.X + left.Y < right.X + right.Y;
		}
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.X == right.X && left.Y == right.Y;
		}
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return left.X != right.X || left.Y != right.Y;
		}
		#endregion
		
		#endregion
		
		#region type casts
		public static explicit operator ulong(Vector2 vector)
		{
			
			byte[] bytesX = BitConverter.GetBytes(vector.X);
			byte[] bytesY = BitConverter.GetBytes(vector.Y);
			byte[] bytes = new byte[8];
			bytesX.CopyTo(bytes, 4);
			bytesY.CopyTo(bytes, 0);
			return BitConverter.ToUInt64(bytes, 0);
		}
		public static explicit operator Vector2(ulong data)
		{
			byte[] bytes = BitConverter.GetBytes(data);
			int x = BitConverter.ToInt32(bytes, 4);
			int y = BitConverter.ToInt32(bytes, 0);
			return new Vector2(x, y);
		}
		#endregion
		
		#region methods
		public override string ToString()
		{
			return "( " + X + ", " + Y + " )";
		}
		public void Rotate()
		{
			int temp = Y;
			Y = X;
			X = -temp;
		}
		public void RotateCCW()
		{
			int temp = Y;
			Y = -X;
			X = temp;
		}
		public void FlipX()
		{
			X = -X;
		}
		public void FlipY()
		{
			Y = -Y;
		}
		public void Min(int xMin, int yMin)
		{
			if(X < xMin) X = xMin;
			if(Y < yMin) Y = yMin;
		}
		public void Max(int xMax, int yMax)
		{
			if(X > xMax) X = xMax;
			if(Y > yMax) Y = yMax;
		}
		public void MinMax(int xMin, int yMin, int xMax, int yMax)
		{
			if(X < xMin) X = xMin;
			else if(X > xMax) X = xMax;
			
			if(Y < yMin) Y = yMin;
			else if(Y > yMax) Y = yMax;
		}
		public Vector2 Copy()
		{
			return new Vector2(X, Y);
		}
		#endregion
		
		private static int RoundToInt(double number)
		{
			return (int)(number + (number > 0.0 ? 0.5 : -0.5));
		}
	}
}

