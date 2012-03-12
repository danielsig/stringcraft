using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;


namespace StringCraft
{
	[StructLayout(LayoutKind.Explicit)]
	public struct CharUnion
	{
		[FieldOffset(0)] public char UnicodeChar;
		[FieldOffset(0)] public byte AsciiChar;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct CharInfo
	{
		[FieldOffset(0)] public CharUnion Char;
		[FieldOffset(2)] public short Attributes;
	}
	
	public static class ConsoleWriter
	{
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern SafeFileHandle CreateFile(
				string fileName,
				[MarshalAs(UnmanagedType.U4)] uint fileAccess,
				[MarshalAs(UnmanagedType.U4)] uint fileShare,
				IntPtr securityAttributes,
				[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
				[MarshalAs(UnmanagedType.U4)] int flags,
				IntPtr template);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool WriteConsoleOutput(
			SafeFileHandle hConsoleOutput, 
			CharInfo[] lpBuffer, 
			Coord dwBufferSize, 
			Coord dwBufferCoord, 
			ref SmallRect lpWriteRegion);

		[StructLayout(LayoutKind.Sequential)]
		public struct Coord
		{
			public short X;
			public short Y;

			public Coord(short X, short Y)
			{
				this.X = X;
				this.Y = Y;
			}
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct SmallRect
		{
			public short Left;
			public short Top;
			public short Right;
			public short Bottom;
		}


		[STAThread]
		public static void Write(Rectangle rectangle, CharInfo[] buf)
		{
			SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

			if (!h.IsInvalid && buf.Length == rectangle.Area)
			{
				SmallRect rect = new SmallRect()
				{
					Left = (short)rectangle.Left, Top = (short)rectangle.Top,
					Right = (short)rectangle.Right, Bottom = (short)rectangle.Bottom
				};

				bool b = WriteConsoleOutput(h, buf,
							new Coord() { X = (short)rectangle.sc_size.X, Y = (short)rectangle.sc_size.Y },
							new Coord() { X = 0, Y = 0 },
							ref rect);
			}
		}
	}
}

