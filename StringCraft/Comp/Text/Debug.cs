using System;

namespace StringCraft
{
	public sealed class Debug : Component, ISingletonComponent
	{
		private static Debug _instance;
		static Debug()
		{
			new GameObject("Debug").AddComponent<Debug>();
		}
		public static int Height
		{
			get{ return _instance._buffer.Height; }
			set
			{
				if(value > 0) _instance._buffer.Height = value;
				else throw new ArgumentOutOfRangeException("Height", "Height can not be less than 1");
			}
		}
		
		public static void Log(params object[] args)
		{
			foreach(object arg in args)
				_instance.WriteLine(arg);
		}
		
		private int _lineLength = 1;
		private Symbol _buffer = Symbol.CreateBuffer(Input.ConsoleSize.X, 20,'#');
		
		public void WriteLine(object obj)
		{
			string toString = obj.ToString();
			for(int i = 0; i < toString.Length; i+= _lineLength)
			{
				int len = Math.Min (_lineLength, toString.Length - i);
				_buffer.Remove(0);
				_buffer.Append( new Line(toString.Substring(i, len), '.'.ToLength(len), "#".ToLength(len)));
			}
		}
		private void Awake()
		{
			_instance = this;
			Gameobject.Parent = Camera.MainCamera.Gameobject;
			
			_lineLength = Input.ConsoleSize.X;
			int top = Height * 3;
			
			Renderer bufferRenderer = Gameobject.AddComponent<Renderer>();
			bufferRenderer.Symbol = _buffer;
		}
		private void Update()
		{
			Gameobject.LocalPosition = (Camera.MainCamera.WorldScreen.BottomLeft - Camera.MainCamera.Gameobject.sc_pos) + Vector2.UP * Height;
		}
	}
}

