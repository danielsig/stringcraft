using System;
using StringCraft;

namespace StringCraftGameTest
{
	public class TestComponent : Component
	{
		public void Awake ()
		{
			Input.DefineVector("Go",
			                   System.Windows.Input.Key.Left,
			                   System.Windows.Input.Key.Right,
			                   System.Windows.Input.Key.Up,
			                   System.Windows.Input.Key.Down
			                   );
			Input.DefineButton("Sple", System.Windows.Input.Key.Space);
		}
		private void Update ()
		{
			/*_velocity += new Vector2((int)(_rand.NextDouble() * 3.99999 - 1.999999), (int)(_rand.NextDouble() * 3.99999 - 1.999999));
			_velocity.MinMax(-1, -1, 1, 1);*/
			
			Vector2 move = Input.Vector("Go");
			move *= 2;
			
			//Gameobject.Position += move;
			Gameobject.Position = Input.MousePosition;
			
			if(Input.ButtonDown("Sple") || Input.MouseDown)
			{
				Gameobject.GetComponent<Renderer>().SetSymbolByName("Colors");
			}
			else
			{
				Gameobject.GetComponent<Renderer>().SetSymbolByName("Mario");
			}
			Debug.Log("hello");
		}
		private Vector2 _velocity = Vector2.ZERO;
		private Random _rand = new Random();
	}
}

