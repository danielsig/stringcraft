using System;
using StringCraft;

namespace StringCraftGameTest
{
	public class TestComponent : Component
	{
		public void Update ()
		{
			_velocity += new Vector2((int)(_rand.NextDouble() * 3.99999 - 1.999999), (int)(_rand.NextDouble() * 3.99999 - 1.999999));
			_velocity.MinMax(-1, -1, 1, 1);
			Gameobject.Position += _velocity;
		}
		private Vector2 _velocity = Vector2.ZERO;
		private Random _rand = new Random();
	}
}

