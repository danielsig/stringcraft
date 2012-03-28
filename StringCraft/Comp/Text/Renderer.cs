using System;
using System.Collections.Generic;

namespace StringCraft
{
	/*
		It's not possible to have strings completely centered if they and
		their containers both have an even width or both have an uneven width.
		If that happens, an exception is thrown.
		To avoid the exception one can pick a preferred centering alignment:
			CenterLeft: to have it centered but a bit to the left if needed
			CenterRight: to have it centered but a bit to the right if needed
	*/
	/*public enum TextAlign
	{
		Left,
		Center,
		CenterLeft,
		CenterRight,
		Right
	}*/
	public sealed class Renderer : Component
	{
		internal static LinkedList<Renderer> sc_displayList = new LinkedList<Renderer>();
		public static Anchor DefaultAnchor = Anchor.TopLeft;
		
		public Symbol Symbol
		{
			get
			{
				return _symbol;
			}
			set
			{
				if(value == null)
				{
					throw new NullReferenceException("Symbol must be non null");
				}
				_symbol = value;
			}
		}
		
		public void SetSymbolByName(string name)
		{
			_symbol = Symbol.GetSymbol(name);
		}
		
		public Vector2 Offset = Vector2.ZERO;
		public Anchor Anchor = DefaultAnchor;
		
		private Symbol _symbol = Symbol.EMPTY;
		
		private LinkedListNode<Renderer> _node = null;

        private void Awake()
		{
			_node = sc_displayList.AddLast(this);
		}
		private void OnDestroy()
		{
			sc_displayList.Remove(_node);
			_node = null;
		}
		
		
		public void ChangeZOrder(int amount)
		{
			CheckAccess();
			
			LinkedListNode<Renderer> target = _node;
            
            if(amount > 0)
            {
			    for(int i = 0; i < amount; i++)
			    {
				    if(target.Next != null) target = target.Next;
				    else break;
			    }
			    sc_displayList.Remove(_node);
			    sc_displayList.AddAfter(target, _node);
            }
            else
            {
                for(int i = 0; i < amount; i++)
			    {
				    if(target.Previous != null) target = target.Previous;
				    else break;
			    }
			    sc_displayList.Remove(_node);
			    sc_displayList.AddBefore(target, _node);
            }
		}
		public void MoveBehind(Renderer target)
		{
            CheckAccess();
			target.CheckAccess();
			
			sc_displayList.Remove(_node);
            sc_displayList.AddAfter(target._node, _node);
		}
		public void MoveInFrontOf(Renderer target)
		{
			CheckAccess();
			target.CheckAccess();
			
			sc_displayList.Remove(_node);
			sc_displayList.AddBefore(target._node, _node);
		}
        public void SendToBack()
        {
            CheckAccess();

            sc_displayList.Remove(_node);
            sc_displayList.AddFirst(_node);
        }
        public void SendToFront()
        {
            CheckAccess();

            sc_displayList.Remove(_node);
            sc_displayList.AddLast(_node);
        }
        public void SwapWith(Renderer other)
        {
            Swap(this, other);
        }
		public static void Swap(Renderer left, Renderer right)
		{
			left.CheckAccess();
			right.CheckAccess();
			
			LinkedListNode<Renderer> temp = left._node;
			left._node = right._node;
			right._node = temp;
			
			left._node.Value = left;
			right._node.Value = right;
		}
		
		internal static void RenderAll()
		{
			foreach(Camera cam in Camera.Cameras)
			{
				Rectangle screen = cam.Screen;
				Rectangle worldScreen = cam.WorldScreen;
				
				Symbol buffer = Symbol.CreateBuffer(screen.sc_size, cam.BackgroundColor.ToChar());
				
				foreach(Renderer renderer in sc_displayList)
				{
					Rectangle bounds = new Rectangle
					(
						renderer.Anchor,
						renderer.Gameobject.sc_pos + renderer.Offset,
						renderer._symbol.Size, false
					);
					if(bounds ^ worldScreen)
					{
						buffer.Overwrite(renderer.Symbol, bounds.TopLeft - worldScreen.sc_topLeft);
					}
				}
				buffer.WriteToConsole(screen.sc_topLeft);
			}
		}
	}
}

