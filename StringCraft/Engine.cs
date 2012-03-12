using System;

namespace StringCraft
{
	public static class Engine
	{
		public static void Init<MainComponent>(int gameLoopsPerSecond = 12, int width = -1, int height = -1, ConsoleColor textDefaultColor = ConsoleColor.Gray, ConsoleColor backgroundDefaultColor = ConsoleColor.Black) where MainComponent : Component
		{
			GameLoopsPerSecond = gameLoopsPerSecond;
			DefaultTextColor = textDefaultColor;
			DefaultBackgroundColor = backgroundDefaultColor;
			
			Console.ForegroundColor = DefaultTextColor;
			Console.BackgroundColor = DefaultBackgroundColor;
			
			WindowSize = new Vector2(width, height);
			
			Console.WriteLine ("Starting StringCraft");
			Console.Title = typeof(MainComponent).ToString();
			
			Console.CursorVisible = false;
			
			Camera.CreateMain();//instantiates the main camera.
			
			GameObject.WORLD.AddComponent<MainComponent>();
						
			while(true)
			{
				DateTime prev = DateTime.Now;
				
				GameObject.WORLD.UpdateMessage();
				Console.ForegroundColor = DefaultTextColor;
				Console.BackgroundColor = DefaultBackgroundColor;
				Renderer.RenderAll();
				
				TimeSpan interval = _span.Subtract(DateTime.Now.Subtract(prev));
				if(interval.Ticks > 0)
					System.Threading.Thread.Sleep(interval);
				if(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)
				{
					return;
				}
			}
		}
		
		public static ConsoleColor DefaultTextColor;
		public static ConsoleColor DefaultBackgroundColor;
		public static Vector2 WindowSize
		{
			get { return new Vector2(Console.WindowWidth, Console.WindowHeight); }
			set
			{
				if(value.X < 0) value.X = Console.LargestWindowWidth;
				if(value.Y < 0) value.Y = Console.LargestWindowHeight;
				
				Console.SetWindowSize(value.X, value.Y);
				Console.SetBufferSize(value.X, value.Y);
			}
		}
		public static String Title
		{
			get { return Console.Title; }
			set { Console.Title = value; }
		}
		public static int GameLoopsPerSecond
		{
			get
			{
				return sc_fps;
			}
			set
			{
				sc_fps = value;
				_span = new TimeSpan(10000000/value);
			}
		}
		internal static int sc_fps;
		private static TimeSpan _span;
	}
}