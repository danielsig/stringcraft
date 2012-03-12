using System;
using NUnit.Framework;
using StringCraft;

namespace StringCraftUnitTests
{
	[TestFixture]
	public class Vector2UnitTest
	{
		
		private int[] _values = {0, 1, -1, 55, -55, int.MaxValue, int.MinValue, int.MaxValue - 1, int.MinValue + 1};
		
		[Test]
		public void TypeCastTest()
		{
			foreach(int x in _values)
			{
				foreach(int y in _values)
				{
					TestCastToAndFromULong(x, y);
				}
			}
		}
		public void TestCastToAndFromULong(int x, int y)
		{
			try
			{
				Vector2 test = new Vector2(x, y);
				Assert.AreEqual((Vector2)(ulong)test, test);
			}
			catch(System.Exception ex)
			{
				throw new System.Exception("casting Vector2( " + x + ", " + y + " ) to ulong and back", ex);
			}
		}
		[Test]
		public void MultiplicationTest()
		{
			int max = int.MaxValue;
			int min = int.MinValue;
			Assert.AreEqual(new Vector2(12, 14), new Vector2(6, 7) * 2);
			Assert.AreEqual(new Vector2(0, 0), new Vector2(6, 7) * 0);
			Assert.AreEqual(new Vector2(-6, -7), new Vector2(6, 7) * -1);
			Assert.AreEqual(new Vector2(0, 0), new Vector2(0, 0) * 999);
			Assert.AreEqual(new Vector2(max, min), new Vector2(max, min) * 1);
			Assert.AreEqual(new Vector2(0, 0), new Vector2(max, min) * 0);
			
			Assert.AreEqual(new Vector2(12, 14), 2 * new Vector2(6, 7));
			Assert.AreEqual(new Vector2(0, 0), 0 * new Vector2(6, 7));
			Assert.AreEqual(new Vector2(-6, -7), -1 * new Vector2(6, 7));
			Assert.AreEqual(new Vector2(0, 0), 999 * new Vector2(0, 0));
			Assert.AreEqual(new Vector2(max, min), 1 * new Vector2(max, min));
			Assert.AreEqual(new Vector2(0, 0), 0 * new Vector2(max, min));
			
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) * 0.5F);
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) * 0.5D);
			
			Assert.AreEqual(new Vector2(3, 3), 0.5F * new Vector2(5, 6));
			Assert.AreEqual(new Vector2(3, 3), 0.5D * new Vector2(5, 6));
		}
		[Test]
		public void DivisionTest()
		{
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) / (short)2);
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) / 2);
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) / 2L);
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) / 2.0F);
			Assert.AreEqual(new Vector2(3, 3), new Vector2(5, 6) / 2.0D);
			
			Assert.AreEqual(new Vector2(-3, -3), new Vector2(-5, -6) / (short)2);
			Assert.AreEqual(new Vector2(-3, -3), new Vector2(-5, -6) / 2);
			Assert.AreEqual(new Vector2(-3, -3), new Vector2(-5, -6) / 2L);
			Assert.AreEqual(new Vector2(-3, -3), new Vector2(-5, -6) / 2.0F);
			Assert.AreEqual(new Vector2(-3, -3), new Vector2(-5, -6) / 2.0D);
		}
		[Test]
		public void DistTest()
		{
			Assert.AreEqual(5D, (new Vector2(3, 0) - new Vector2(0, 4)).DoubleMagnitude);
			Assert.AreEqual(5, (new Vector2(3, 0) - new Vector2(0, 4)).Magnitude);
			Assert.AreEqual(5, new Vector2(3, 0) % new Vector2(0, 4));
			Assert.AreEqual(11, new Vector2(9, 16) % new Vector2(2, 7));
		}
		[Test]
		public void ArithmeticTest()
		{
			Assert.AreEqual(new Vector2(3, 4), new Vector2(3, 0) + new Vector2(0, 4));
			Assert.AreEqual(new Vector2(3, 10), new Vector2(3, 6) + new Vector2(0, 4));
			Assert.AreEqual(new Vector2(3, -4), new Vector2(3, 0) - new Vector2(0, 4));
			Assert.AreEqual(new Vector2(3, 2), new Vector2(3, 6) - new Vector2(0, 4));
		}
		[Test]
		public void SugarTest()
		{
			Assert.AreEqual(new Vector2(-3, 3), new Vector2(3, 3).FlippedX);
			Assert.AreEqual(new Vector2(3, -3), new Vector2(3, 3).FlippedY);
			
			Assert.AreEqual(Vector2.DOWN, Vector2.RIGHT.Rotated);
			Assert.AreEqual(Vector2.LEFT, Vector2.DOWN.Rotated);
			Assert.AreEqual(Vector2.UP, Vector2.LEFT.Rotated);
			Assert.AreEqual(Vector2.RIGHT, Vector2.UP.Rotated);
			
			Assert.AreEqual(Vector2.RIGHT, Vector2.RIGHT.Rotated.Rotated.Rotated.Rotated);
			
			Assert.AreEqual(Vector2.UP, Vector2.RIGHT.RotatedCCW);
			Assert.AreEqual(Vector2.LEFT, Vector2.UP.RotatedCCW);
			Assert.AreEqual(Vector2.DOWN, Vector2.LEFT.RotatedCCW);
			Assert.AreEqual(Vector2.RIGHT, Vector2.DOWN.RotatedCCW);
			
			Assert.AreEqual(Vector2.RIGHT, Vector2.RIGHT.RotatedCCW.RotatedCCW.RotatedCCW.RotatedCCW);
		}
	}
}

