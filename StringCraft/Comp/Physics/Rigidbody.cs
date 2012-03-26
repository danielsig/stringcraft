using System;

namespace StringCraft
{
	public sealed class Rigidbody : Component
	{
		public static Vector2 Gravity = Vector2.DOWN * 100;
		public static Rectangle WorldArea = new Rectangle(0, 50, 0, 25);
		
		public Rectangle LocalCollider = new Rectangle(-1, 1, -1, 1, true);
		public Rectangle Collider
		{
			get
			{
				return new Rectangle(Anchor.Center, Position + LocalCollider.Center, LocalCollider.sc_size);
			}
			set
			{
				LocalCollider.sc_size = value.sc_size;
				LocalCollider.Center = value.Center - Position;
			}
		}
		public Vector2 Velocity
		{
			get
			{
				return new Vector2((int)(_vx * 1000.0), (int)(_vy * 1000.0));
			}
			set
			{
				_vx = value.X / 1000.0;
				_vy = value.Y / 1000.0;
			}
		}
		public double Bounciness
		{
			get
			{
				return -_bounciness;
			}
			set
			{
				_bounciness = -((value < 0) ? (0) : (value > 1 ? 1 : value));
			}
		}
		public double Friction
		{
			get
			{
				return 1.0 - _slippery;
			}
			set
			{
				_slippery = 1.0 - ((value < 0) ? (0) : (value > 1 ? 1 : value));
			}
		}
		private Vector2 Position
		{
			get
			{
				return new Vector2((int)(_px - 0.5), (int)(_py - 0.5));
			}
			set
			{
				_px = value.X + 0.5;
				_py = value.Y + 0.5;
			}
		}
		
		private double _bounciness = -0.5;
		private double _slippery = 0.8;
		private double _vx = 0.0;
		private double _vy = 0.0;
		
		private double _px = 0.0;
		private double _py = 0.0;
				
		private void Update()
		{
			//check if the position has been assigned by something else
			if(Gameobject.sc_pos != Position)
				Position = Gameobject.sc_pos;
			
			//increment velocity by gravity
			_vx += Gravity.X / 1000.0;
			_vy += Gravity.Y / 1000.0;
			
			//increment position by velocity
			_px += _vx;
			_py += _vy;
			
			//check if it's outside the world area bounding box
			Rectangle bounds = Collider;
			int safetyCounter = 3;
			while(!(bounds > WorldArea) && safetyCounter-- > 0)
			{
				if(bounds.Bottom >= WorldArea.Bottom && _vy >= 0)
				{
					_py -= (bounds.Bottom - WorldArea.Bottom) + 0.5;
					_vy *= _bounciness;
					_vx *= _slippery;
				}
				else if(bounds.Top <= WorldArea.Top && _vy <= 0)
				{
					_py -= (bounds.Top - WorldArea.Top) - 0.5;
					_vy *= _bounciness;
					_vx *= _slippery;
				}
				
				if(bounds.Right >= WorldArea.Right && _vx >= 0)
				{
					_px -= (bounds.Right - WorldArea.Right) + 0.5;
					_vx *= _bounciness;
					_vy *= _slippery;
				}
				else if(bounds.Left <= WorldArea.Left && _vx <= 0)
				{
					_px -= (bounds.Left - WorldArea.Left) - 0.5;
					_vx *= _bounciness;
					_vy *= _slippery;
				}
				bounds = Collider;
			} 
			//update position
			Gameobject.sc_pos = Position;
		}
	}
}

