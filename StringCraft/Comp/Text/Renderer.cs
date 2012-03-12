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
	public class Renderer : Component
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
				
		public void Awake()
		{
			_node = sc_displayList.AddLast(this);
		}
		public void OnDestroy()
		{
			sc_displayList.Remove(_node);
			_node = null;
		}
		
		public static Renderer operator >>(Renderer ren, int shift)
		{
			ren.CheckAccess();
			
			LinkedListNode<Renderer> target = ren._node;
			for(int i = 0; i < shift; i++)
			{
				if(target.Next != null) target = target.Next;
				else break;
			}
			sc_displayList.Remove(ren._node);
			sc_displayList.AddAfter(target, ren._node);
			return ren;
		}
		public static Renderer operator <<(Renderer ren, int shift)
		{
			ren.CheckAccess();
			
			LinkedListNode<Renderer> target = ren._node;
			for(int i = 0; i < shift; i++)
			{
				if(target.Previous != null) target = target.Previous;
				else break;
			}
			sc_displayList.Remove(ren._node);
			sc_displayList.AddBefore(target, ren._node);
			return ren;
		}
		public static Renderer operator <(Renderer target, Renderer ren)
		{
			ren.CheckAccess();
			target.CheckAccess();
			
			sc_displayList.Remove(ren._node);
			sc_displayList.AddAfter(target._node, ren._node);
			return ren;
		}
		public static Renderer operator >(Renderer ren, Renderer target)
		{
			ren.CheckAccess();
			target.CheckAccess();
			
			sc_displayList.Remove(ren._node);
			sc_displayList.AddBefore(target._node, ren._node);
			return ren;
		}
		public static Renderer operator ^(Renderer left, Renderer right)
		{
			left.CheckAccess();
			right.CheckAccess();
			
			LinkedListNode<Renderer> temp = left._node;
			left._node = right._node;
			right._node = temp;
			
			left._node.Value = left;
			right._node.Value = right;
			return left;
		}
		internal static void RenderAll()
		{
			foreach(Camera cam in Camera.Cameras)
			{
				Rectangle screen = cam.Screen;
				Rectangle worldScreen = cam.WorldScreen;
				
				Symbol buffer = Symbol.CreateBuffer(screen.sc_size);
				
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
						buffer.Overwrite(renderer.Symbol, bounds.TopLeft - worldScreen.TopLeft);
					}
				}
				buffer.WriteToConsole(new Vector2(0, 0));
			}
		}
	}
}

