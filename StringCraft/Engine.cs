using System;

namespace StringCraft
{
	public static class Engine
	{
		public static void Init<MainComponent>(int gameLoopsPerSecond = 12, int width = -1, int height = -1, ConsoleColor mainCameraBackgroundColor = ConsoleColor.Black) where MainComponent : Component
		{
			GameLoopsPerSecond = gameLoopsPerSecond;
			
			Input.ConsoleSize = new Vector2(width, height);
			Input.WindowScreenPositionInPixels = new Vector2(0, 0);
			
			Console.WriteLine ("Starting StringCraft");
			Console.Title = typeof(MainComponent).ToString();
			
			Console.CursorVisible = false;
			
			Camera.CreateMain(mainCameraBackgroundColor);//instantiates the main camera.
			
			GameObject.WORLD.AddComponent<MainComponent>();
			
			Input.DefineButton("Quit", System.Windows.Input.Key.Escape);
			Input.DefineButton("Continue", System.Windows.Input.Key.Return);
			Input.DefineButton("Step", System.Windows.Input.Key.Tab);
			//Debug.Log("");
			bool stepping = false;
			while(true)
			{
				DateTime prev = DateTime.Now;
				
				GameObject.UpdateAll();
				Renderer.RenderAll();
				
				//Console.SetCursorPosition(0,0);
				//GameObject.WORLD.LogAll();
				
				TimeSpan interval = _span.Subtract(DateTime.Now.Subtract(prev));
				if(interval.Ticks > 0)
					System.Threading.Thread.Sleep(interval);
				
				Input.Update();
				if(Input.ButtonPressed("Quit")) return;
				
				if(stepping)
				{
					stepping = false;
					while(true)
					{
						Input.Update();
						if(Input.ButtonPressed("Quit")) return;
						if(Input.ButtonPressed("Step") || Input.ButtonPressed("Continue")) break;
					}
				}
				if(Input.ButtonPressed("Step"))
				{
					stepping = true;
				}
				
				while(Console.KeyAvailable) Console.ReadKey(false);
				
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