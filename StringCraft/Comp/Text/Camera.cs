using System;
using System.Collections.Generic;

namespace StringCraft
{
	public sealed class Camera : Component
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
		public Anchor CameraLocalCenter = Anchor.Center;
		
		public Rectangle WorldScreen
		{
			get
			{
				CheckAccess();
				return new Rectangle(CameraLocalCenter, Gameobject.sc_pos, Screen.sc_size, false) ;
			}
		}
		
		public Vector2 ToWorldSpace(Vector2 screenVector)
		{
			return WorldScreen.sc_topLeft + screenVector;
		}
		public Vector2 ToScreenSpace(Vector2 worldVector)
		{
			return worldVector - WorldScreen.sc_topLeft;
		}
		
		public Vector2 FromConsoleSpaceToWorldSpace(Vector2 consoleVector)
		{
			return WorldScreen.sc_topLeft + (consoleVector - Screen.sc_topLeft);
		}
		public Vector2 FromWorldSpaceToConsoleSpace(Vector2 worldVector)
		{
			return Screen.sc_topLeft + (worldVector - WorldScreen.sc_topLeft);
		}
		
		private void Awake()
		{
			Cameras.AddLast(this);
			Screen = new Rectangle(Anchor.TopLeft, Vector2.ZERO, Input.ConsoleSize);
		}
		private void OnDestroy()
		{
			Cameras.Remove(this);
		}
	}
}

