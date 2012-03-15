using System;
using System.Collections.Generic;

namespace StringCraft
{
	public class Camera : Component
	{
		public static LinkedList<Camera> Cameras = new LinkedList<Camera>();
		public static Camera MainCamera
		{
			get
			{
				if(Cameras.Count == 0) CreateMain(ConsoleColor.Black);
				return Cameras.First.Value;
			}
		}
		internal static Camera CreateMain(ConsoleColor backgroundColor)
		{
			GameObject mainCam = new GameObject("MainCamera");
			Camera cam = mainCam.AddComponent<Camera>();
			cam.BackgroundColor = backgroundColor;
			return cam;
		}
		
		public Rectangle Screen;
		public ConsoleColor BackgroundColor;
		
		public Rectangle WorldScreen
		{
			get
			{
				CheckAccess();
				return Screen + Gameobject.sc_pos;
			}
		}
		
		public void Awake()
		{
			Cameras.AddLast(this);
			Screen = new Rectangle(Anchor.TopLeft, Vector2.ZERO, Input.ConsoleSize);
		}
		public void OnDestroy()
		{
			Cameras.Remove(this);
		}
	}
}

