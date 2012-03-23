using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace StringCraft
{
	public enum ButtonState
	{
		Idle = 0,
		Release = 1,
		Press = 2, 
		Hold = 3
	}
	
	//TODO unit test everything here
	
	public static class Input
	{
		private static Dictionary<string, Key> _buttons = new Dictionary<string, Key>();
		private static Dictionary<Key, ButtonState> _keys = new Dictionary<Key, ButtonState>();
		private static LinkedList<Key> _keyList = new LinkedList<Key>();
		
		private static Vector2 _mouseClick = Vector2.ZERO;
		private static Vector2 _mouseRightClick = Vector2.ZERO;
		private static Vector2 _mouseMiddleClick = Vector2.ZERO;
				
		public static void DefineButton(string name, Key key)
		{
			if(_buttons.ContainsKey(name))
			{
				_keys.Remove(_buttons[name]);
				_keyList.Remove(_buttons[name]);
				_buttons[name] = key;
			}
			else
			{
				_buttons.Add(name, key);
				_keys.Add(key, ButtonState.Idle);
				_keyList.AddLast(key);
			}
		}
		public static void DefineDirection(string name, Key positiveKey, Key negativeKey)
		{
			DefineButton(name + "+", positiveKey);
			DefineButton(name + "-", negativeKey);
		}
		public static void DefineVector(string name, Key leftKey, Key rightKey, Key upKey, Key downKey)
		{
			DefineButton(name + "<", leftKey);
			DefineButton(name + ">", rightKey);
			DefineButton(name + "^", upKey);
			DefineButton(name + "v", downKey);
		}
		public static void UndefineButton(string name)
		{
			if(_buttons.ContainsKey(name))
			{
				_keys.Remove(_buttons[name]);
				_keyList.Remove(_buttons[name]);
				_buttons.Remove(name);
			}
		}
		public static void UndefineDirection(string name)
		{
			UndefineButton(name + "+");
			UndefineButton(name + "-");
		}
		public static void UndefineVector(string name)
		{
			UndefineButton(name + "<");
			UndefineButton(name + ">");
			UndefineButton(name + "^");
			UndefineButton(name + "v");
		}
		
		public static ButtonState Button(string name)
		{
			return _keys[_buttons[name]];
		}
		
		public static int Direction(string name)
		{
			int dir = 0;
			if(ButtonDown(name + "+")) dir++;
			if(ButtonDown(name + "-")) dir--;
			return dir;
		}
		public static Vector2 Vector(string name)
		{
			Vector2 v = Vector2.ZERO;
			if(ButtonDown(name + "<")) v.X--;
			if(ButtonDown(name + ">")) v.X++;
			if(ButtonDown(name + "^")) v.Y--;
			if(ButtonDown(name + "v")) v.Y++;
			return v;
		}
		
		public static ButtonState Mouse{ get; private set; }
		public static ButtonState MouseRight{ get; private set; }
		public static ButtonState MouseMiddle{ get; private set; }
		
		public static bool MouseDown{ get { return Mouse > ButtonState.Release; } }
		public static bool MouseUp{ get { return Mouse < ButtonState.Press; } }
		public static bool MousePressed{ get { return Mouse == ButtonState.Press; } }
		public static bool MouseReleased{ get { return Mouse == ButtonState.Release; } }
		public static bool MouseClicked{ get { return Mouse == ButtonState.Release && _mouseClick == MousePosition; } }
		
		public static bool MouseRightDown{ get { return MouseRight > ButtonState.Release; } }
		public static bool MouseRightUp{ get { return MouseRight < ButtonState.Press; } }
		public static bool MouseRightPressed{ get { return MouseRight == ButtonState.Press; } }
		public static bool MouseRightReleased{ get { return MouseRight == ButtonState.Release; } }
		public static bool MouseRightClicked{ get { return MouseRight == ButtonState.Release && _mouseRightClick == MousePosition; } }
		
		public static bool MouseMiddleDown{ get { return MouseMiddle > ButtonState.Release; } }
		public static bool MouseMiddleUp{ get { return MouseMiddle < ButtonState.Press; } }
		public static bool MouseMiddlePressed{ get { return MouseMiddle == ButtonState.Press; } }
		public static bool MouseMiddleReleased{ get { return MouseMiddle == ButtonState.Release; } }
		public static bool MouseMiddleClicked{ get { return MouseMiddle == ButtonState.Release && _mouseMiddleClick == MousePosition; } }
		
		public static Vector2 MousePosition
		{
			get
			{
				Vector2 pos = MouseConsolePosition;
				foreach(Camera cam in Camera.Cameras)
				{
					if(pos > cam.Screen)
					{
						return cam.FromConsoleSpaceToWorldSpace(pos);
					}
				}
				return Camera.MainCamera.FromConsoleSpaceToWorldSpace(pos);
			}
			set
			{
				Vector2 pos = MouseConsolePosition;
				foreach(Camera cam in Camera.Cameras)
				{
					if(pos > cam.Screen)
					{
						MouseConsolePosition = cam.FromWorldSpaceToConsoleSpace(value);
					}
				}
			}
		}
		public static Vector2 MouseConsolePosition
		{
			get
			{
				Vector2 pos = MouseScreenPositionInPixels;
				ScreenToClient(MyConsoleWindow, ref pos);
				return pos / FontSizeInPixels;
			}
			set
			{
				value *= FontSizeInPixels;
				ClientToScreen(MyConsoleWindow, ref value);
				SetCursorPos(value.X, value.Y);
			}
		}
		public static Vector2 MouseScreenPositionInPixels
		{
			get
			{
				Vector2 pos;
				GetCursorPos(out pos);
				return pos;
			}
			set
			{
				SetCursorPos(value.X, value.Y);
			}
		}
		
		public static Vector2 FontSizeInPixels
		{
			get
			{
				
				Rectangle rect;
				GetClientRect(MyConsoleWindow, out rect);
				return rect.sc_size / ConsoleSize;
			}
		}
		public static Vector2 ScreenResolutionInPixels
		{
			get
			{
				WindowPlacement placement = new WindowPlacement();
				GetWindowPlacement(MyScreen, ref placement);
				return placement.rect.Size;
			}
		}
		public static Vector2 WindowScreenPositionInPixels
		{
			get
			{
				WindowPlacement placement = new WindowPlacement();
				GetWindowPlacement(MyConsoleWindow, ref placement); 
				if(placement.flags == 2) return new Vector2(-1, 0);
				return placement.rect.TopLeft;
			}
			set
			{
				SetWindowPos(MyConsoleWindow, 0, value.X, value.Y, 0, 0, _NO_SIZE);
			}
		}
		public static Vector2 WindowSizeInPixels
		{
			get
			{
				WindowPlacement placement = new WindowPlacement();
				GetWindowPlacement(MyConsoleWindow, ref placement);
				if(placement.flags == 2)
				{
					double ratioX = Console.LargestWindowWidth / (double)Console.WindowWidth;
					double ratioY = Console.LargestWindowHeight / (double)Console.WindowHeight;
					Vector2 size = placement.rect.sc_size;
					size.X = (int)(size.X * ratioX);
					size.Y = (int)(size.Y * ratioY);
					return size;
				}
				return placement.rect.Size;
			}
			set
			{
				SetWindowPos(MyConsoleWindow, 0, 0, 0, value.X, value.Y, _NO_SIZE);
			}
		}
		public static Vector2 ConsoleSize
		{
			get { return new Vector2(Math.Min(Console.BufferWidth, Console.WindowWidth), Math.Min(Console.BufferHeight, Console.WindowHeight)); }
			set
			{
				if(value.X < 0 || value.X > Console.LargestWindowWidth) value.X = Console.LargestWindowWidth;
				if(value.Y < 0 || value.Y > Console.LargestWindowHeight) value.Y = Console.LargestWindowHeight;
				
				Console.SetWindowSize(value.X-1, value.Y-1);
				Console.SetBufferSize(value.X-1, value.Y-1);
				Console.SetWindowPosition(0, 0);
			}
		}
		
		
		public static bool ButtonDown(string name)
		{
			return Button(name) > ButtonState.Release;
		}
		public static bool ButtonUp(string name)
		{
			return Button(name) < ButtonState.Press;
		}
		
		public static bool ButtonPressed(string name)
		{
			return Button(name) == ButtonState.Press;
		}
		
		public static bool ButtonReleased(string name)
		{
			return Button(name) == ButtonState.Release;
		}
		
		internal static void Update()
		{
			LinkedListNode<Key> node = _keyList.First;
			while(node != null)
			{
				LinkedListNode<Key> next = node.Next;
				
				Key key = node.Value;
				
				if(Keyboard.IsKeyDown(key))
				{
					if(_keys[key] > ButtonState.Release) _keys[key] = ButtonState.Hold;
					else _keys[key] = ButtonState.Press;
				}
				else if(_keys[key] == ButtonState.Release)
					_keys[key] = ButtonState.Idle;
				else if(_keys[key] != ButtonState.Idle)
					_keys[key] = ButtonState.Release;
				
				node = next;
			}
			if(GetAsyncKeyState(_LEFT_BUTTON) != 0)
			{
				if(Mouse > ButtonState.Release)
					Mouse = ButtonState.Hold;
				else
				{
					Mouse = ButtonState.Press;
					_mouseClick = MousePosition;
				}
			}
			else if(Mouse == ButtonState.Release)
				Mouse = ButtonState.Idle;
			else if(Mouse != ButtonState.Idle)
				Mouse = ButtonState.Release;
			
			if(GetAsyncKeyState(_RIGHT_BUTTON) != 0)
			{
				if(MouseRight > ButtonState.Release)
					MouseRight = ButtonState.Hold;
				else
				{
					MouseRight = ButtonState.Press;
					_mouseRightClick = MousePosition;
				}
			}
			else if(MouseRight == ButtonState.Release)
				MouseRight = ButtonState.Idle;
			else if(MouseRight != ButtonState.Idle)
				MouseRight = ButtonState.Release;
			
			if(GetAsyncKeyState(_MIDDLE_BUTTON) != 0)
			{
				if(MouseMiddle > ButtonState.Release)
					MouseMiddle = ButtonState.Hold;
				else
				{
					MouseMiddle = ButtonState.Press;
					_mouseMiddleClick = MousePosition;
				}
			}
			else if(MouseMiddle == ButtonState.Release)
				MouseMiddle = ButtonState.Idle;
			else if(MouseMiddle != ButtonState.Idle)
				MouseMiddle = ButtonState.Release;
		}
		
		#region SOME FREAKY HACKS
		
		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(UInt16 virtualKeyCode);
		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(out Vector2 lpPoint);
		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);
		[DllImport("user32.dll")]
		private static extern bool ScreenToClient(IntPtr hWnd, ref Vector2 lpPoint);
		[DllImport("user32.dll")]
		private static extern bool ClientToScreen(IntPtr hWnd, ref Vector2 lpPoint);
		[DllImport("user32.dll")]
		private static extern bool GetClientRect(IntPtr hWnd, out Rectangle rect);	
		
		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
		
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);
		
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetConsoleWindow();
		
		[DllImport("user32.dll", ExactSpelling = true)]
		private static extern IntPtr GetDesktopWindow();
		
		
		
		private const UInt16 _LEFT_BUTTON = 0x01;//left mouse button
		private const UInt16 _RIGHT_BUTTON = 0x02;//right mouse button
		private const UInt16 _MIDDLE_BUTTON = 0x04;//middle mouse button
		
		private const int _NO_SIZE = 0x0001;
		
		private static IntPtr MyConsoleWindow = GetConsoleWindow();
		private static IntPtr MyScreen = GetDesktopWindow();
		
		[StructLayout(LayoutKind.Sequential)]
		private struct WindowPlacement
		{
			public uint length;
			public uint flags;
			public uint showCmd;
			public Vector2 min;
			public Vector2 max;
			public Rectangle rect;
		}
		
		#endregion
	}
}

