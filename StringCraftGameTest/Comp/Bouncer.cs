using System;
using StringCraft;

namespace StringCraftGameTest
{
	public class Bouncer : Component
	{
		public double Amount = 1;
		
		private void Update()
		{
			Gameobject.GetComponent<Rigidbody>().Velocity += Vector2.GetRandom(Amount * 1000.0);
		}
	}
}

