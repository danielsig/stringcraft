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
				if(Cameras.Count == 0) CreateMain();
				return Cameras.First.Value;
			}
		}
		internal static Camera CreateMain()
		{
			GameObject mainCam = new GameObject("MainCamera");
			return mainCam.AddComponent<Camera>();
		}
		
		public Rectangle Screen;
		
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
			Screen = new Rectangle(Anchor.TopLeft, Vector2.ZERO, Engine.WindowSize);
		}
		public void OnDestroy()
		{
			Cameras.Remove(this);
		}
	}
}

