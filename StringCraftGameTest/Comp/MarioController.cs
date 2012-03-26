using System;
using StringCraft;

namespace StringCraftGameTest
{
	public class MarioController : Component, ISingletonComponent
	{
		public void Awake ()
		{
			Input.DefineVector("Go",
			                   System.Windows.Input.Key.Left,
			                   System.Windows.Input.Key.Right,
			                   System.Windows.Input.Key.Up,
			                   System.Windows.Input.Key.Down
			                   );
			Input.DefineButton("Jump", System.Windows.Input.Key.Space);
			
			Renderer renderer = Gameobject.AddComponent<Renderer>();
			renderer.SetSymbolByName("Mario");
			renderer.Anchor = Anchor.Bottom;
			
			Rigidbody body = Gameobject.AddComponent<Rigidbody>();
			body.LocalCollider = new Rectangle(renderer.Anchor, Vector2.ZERO, renderer.Symbol.Size, false);
		}
		private void Update ()
		{	
			Vector2 move = Input.Vector("Go");
			move *= 200;
			
			Gameobject.GetComponent<Rigidbody>().Velocity += move;
		}
		private Vector2 _velocity = Vector2.ZERO;
		private Random _rand = new Random();
	}
}

