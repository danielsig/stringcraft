using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace StringCraft
{
	public sealed class GameObject : StringCraftObject
	{
		static GameObject()
		{
			//Genesis
			WORLD = new GameObject("WORLD", WORLD);
		}
		
		public static readonly GameObject WORLD;
		private static Queue<Component> _componentsAddedOnLastUpdate = new Queue<Component>();
		private const String DEFAULT_NAME = "Untitled";
		
		public String Name
		{ 
			get
			{
				if(Destroyed) ThrowAccessError();
				return sc_name;
			}
			set
			{
				if(Destroyed) ThrowAccessError();
				if(this != WORLD)
				{
					sc_name = value;
				}
				else throw new InvalidOperationException("Can not assign a new name to GameObject.WORLD");
			}
		}
		public Vector2 Position
		{ 
			get
			{
				if(Destroyed) ThrowAccessError();
				return sc_pos;
			}
			set
			{
				if(Destroyed) ThrowAccessError();
				sc_pos = value;
				sc_locPos = sc_pos - sc_parent.sc_pos;
				
				UpdateChildren();
			}
		}
		public Vector2 LocalPosition
		{
			get
			{
				if(Destroyed) ThrowAccessError();
				return sc_locPos;
			}
			set
			{
				if(Destroyed) ThrowAccessError();
				sc_locPos = value;
				UpdateChildren();
			}
		}
		public GameObject Parent
		{ 
			get
			{
				if(Destroyed) ThrowAccessError();
				return sc_parent;
			}
			set
			{
				if(sc_parent == value) return;
				if(Destroyed) ThrowAccessError();
				if(value == null) throw new ArgumentNullException("Parent", "Parent must be non-null. Did you mean GameObject.WORLD ?");
				if(this != WORLD)
				{
					AddParent(value);
				}
				else throw new InvalidOperationException("Can not assign a new parent to GameObject.WORLD");
			}
		}
		private void AddParent(GameObject newParent)
		{
			if(sc_parent != null) sc_parent._children.Remove(_childNodeInParent);
			_childNodeInParent = newParent._children.AddLast(this);
			sc_parent = newParent;
		}
		public bool Exists{ get{ return sc_parent != null; } }
		public bool Destroyed{ get{ return sc_parent == null; } }
		
		internal String sc_name;
		internal Vector2 sc_pos = Vector2.ZERO;
		internal Vector2 sc_locPos = Vector2.ZERO;
		internal GameObject sc_parent = null;
		internal static GameObject sc_isAddingAComponent = null;
		
		/** 
		 * switches between true and false between updates (game loops).
		 * used to see if a GameObject has already been updated.
		 * useful in order to stop GameObjects from being updated more than once
		 * even though they have been moved between parents or created during
		 * start() method calls.
		 */
		private bool _tick;
		private static bool _globalTick = true;
		
		
		private KeyedByTypeCollection<Component> _components;
		private LinkedList<UpdateMethod> _updateList;
		private LinkedList<GameObject> _children;
		private LinkedListNode<GameObject> _childNodeInParent;
		
		public GameObject (String name, GameObject parent)
		{
			if(parent == null)
			{
				if(WORLD != null) throw new ArgumentNullException("parent", "Parent must be non-null");
				else sc_parent = this;//The world is instantiating
			}
			else AddParent(parent);
			
			sc_name = name;
			_tick = _globalTick;
			_components = new KeyedByTypeCollection<Component>();
			_updateList = new LinkedList<UpdateMethod>();//so I can iterate over it AND make changes at the same time
			_children = new LinkedList<GameObject>();
		}
		public GameObject (GameObject parent) : this(DEFAULT_NAME, parent) { }
		public GameObject (String name) : this(name, WORLD) { }
		public GameObject () : this(DEFAULT_NAME, WORLD) { }
		
		public static implicit operator bool(GameObject gameObject)
		{
			return gameObject != null && gameObject.Exists;
		}
		public GameObject Chain(GameObject child)
		{
			return child.Parent = this;
		}
		public GameObject AddChild(GameObject child)
		{
			child.Parent = this;
			return child;
		}
		public GameObject Chain<CompType>() where CompType : Component
		{
			AddComponent<CompType>();
			return this;
		}
		public CompType AddComponent<CompType>() where CompType : Component
		{
			if(Destroyed) ThrowAccessError();
			Type compType = typeof(CompType);
			//checking for conflicts
			if(_components.Contains(compType))
			{
				throw new InvalidOperationException(
					"Can not add two components of the same type on the same GameObject: "
					+ compType.ToString()
				);
			}
			
			//adding
			sc_isAddingAComponent = this;
			Component instance = compType.GetConstructor(new Type[0]).Invoke(new Object[]{}) as Component;
			sc_isAddingAComponent = null;
			_components.Add(instance);
			
			//calling awake
			instance.CallAwake();
			
			//adding to the queue
			_componentsAddedOnLastUpdate.Enqueue(instance);
			
			return instance as CompType;
		}
		public CompType GetComponent<CompType>() where CompType : Component
		{
			return _components.Find<CompType>();
		}
		public DestructionResponse RemoveComponent(Component component)
		{
			if(Destroyed) ThrowAccessError();
			Type compType = component.GetType();
			
			if(component == null)
				ThrowNotExistError<Component>(compType.ToString());
			
			if(component.CallDestroy() == DestructionResponse.Allow)
			{
				component.sc_exists = false;
				if(component.sc_updateListNodeInParent != null)
				{
					_updateList.Remove(component.sc_updateListNodeInParent);
					component.sc_updateListNodeInParent = null;
				}
				_components.Remove(component);
				
				return DestructionResponse.Allow;
			}
			else return DestructionResponse.Cancel;
		}
		public DestructionResponse RemoveComponent<CompType>() where CompType : Component
		{
			if(Destroyed) ThrowAccessError();
			CompType instance = _components.Find<CompType>();
			Type compType = typeof(CompType);
			
			if(instance == null)
				ThrowNotExistError<Component>(compType.ToString());
			
			if(instance.CallDestroy() == DestructionResponse.Allow)
			{
				instance.sc_exists = false;
				if(instance.sc_updateListNodeInParent != null)
				{
					_updateList.Remove(instance.sc_updateListNodeInParent);
					instance.sc_updateListNodeInParent = null;
				}
				_components.Remove<CompType>();
				
				return DestructionResponse.Allow;
			}
			else return DestructionResponse.Cancel;
		}
		public void Destroy()
		{
			if(Destroyed) return;
			_childNodeInParent.List.Remove(_childNodeInParent);
			DestroyRecursive();
		}
		private void DestroyRecursive()
		{
			if(Destroyed) return;
			foreach(Component comp in _components)
			{
				comp.CallDestroy();
				comp.sc_exists = false;
			}
			LinkedListNode<GameObject> current = _children.First;
			while(current != null)
			{
				LinkedListNode<GameObject> next = current.Next;
				current.Value.DestroyRecursive();
				current = next;
			}
			sc_parent = null;
		}
		
		private void ThrowAccessError()
		{
			throw new InvalidOperationException("The GameObject " + this + " has been destroyed but you're still trying to access it.");
		}
		private void ThrowNotExistError<T>(string element)
		{
			throw new InvalidOperationException("The GameObject " + this + " Does not contain the " + typeof(T) + " " + element);
		}
		private void UpdateChildren()
		{
			sc_pos = sc_parent.sc_pos + sc_locPos;
			foreach(GameObject child in _children)
			{
				child.UpdateChildren();
			}
		}
		internal static void UpdateAll()
		{
			//calling start methods
			Queue<Component> temp = _componentsAddedOnLastUpdate;
			if(_componentsAddedOnLastUpdate.Count > 0)
			{
				/*
				 * make new Queue, because the Start() methods that will be called
				 * might add more components hence adding more to the queue. Such
				 * additions are actually supposed to be added on the next update,
				 * not the curront one.
				*/
				_componentsAddedOnLastUpdate = new Queue<Component>();
				
				foreach(Component comp in temp)
					if(comp.sc_exists)//does it still exist
						comp.CallStart();
			}
			//main update
			_globalTick = !_globalTick;
			WORLD.UpdateMessage();
			
			//adding update calls to update lists
			foreach(Component comp in _componentsAddedOnLastUpdate)
			{
				if(comp.sc_exists)//does it still exist
				{
					UpdateMethod updateMethod = comp.GetUpdateMethod();
					if(updateMethod != null)
					{
						comp.sc_updateListNodeInParent =
							comp.Gameobject._updateList.AddLast(updateMethod);
					}
				}
			}
		}
		private void UpdateMessage()
		{
			//update() calls
			LinkedListNode<UpdateMethod> currentUpdate = _updateList.First;
			while(currentUpdate != null)
			{
				LinkedListNode<UpdateMethod> next = currentUpdate.Next;
				
				currentUpdate.Value();//update call
				if(Destroyed) return;
				
				currentUpdate = next;
			}
			//recursion on child GameObjects
			LinkedListNode<GameObject> current = _children.First;
			while(current != null)
			{
				LinkedListNode<GameObject> next = current.Next;
				
				GameObject child = current.Value;
				if(child.Exists && child._tick != _globalTick)
				{
					child._tick = _globalTick;
					child.UpdateMessage();//recursive call
				}
				if(Destroyed) return;
				
				current = next;
			}
		}
		public void LogHierachy()
		{
			LogHierachyRecursive("");
		}
		private void LogHierachyRecursive(string indent)
		{
			System.Console.WriteLine(indent + "" + this + "\n" + indent + "{");
			foreach(GameObject child in _children)
			{
				child.LogHierachyRecursive(indent + "  ");
			}
			System.Console.WriteLine(indent + "}");
		}
		public void LogAll()
		{
			LogAllRecursive("");
		}
		private void LogAllRecursive(string indent)
		{
			string border = "═".Times(Name.Length);
			string line = "-".Times(Name.Length);
			System.Console.WriteLine(indent + "╔"  + border + "╗");
			System.Console.WriteLine(indent + "║" + Name +    "║");
			System.Console.WriteLine(indent + "║" + line +    "╚" + border);
			
			string newIndent = indent + "║ ";
			
			if(_components.Count > 0)
			{
				//System.Console.WriteLine(indent + "║╔════════");
				foreach(Component comp in _components)
				{
					System.Console.WriteLine(newIndent + "-<" + comp.Name + ">-");
					comp.LogMembers(newIndent + "    ");
				}
				//System.Console.WriteLine(indent + "╠════════");
			}
			System.Console.WriteLine(indent + "║");
			if(_children.Count > 0)
			{
				//System.Console.WriteLine(indent + "║╔════════");
				foreach(GameObject child in _children)
				{
					child.LogAllRecursive(newIndent);
				}
				//System.Console.WriteLine(indent + "║╚════════");
			}
			System.Console.WriteLine(indent + "╚═" + border + border);
		}
		public void Log()
		{
			string[] names = new string[_children.Count];
			int i = 0;
			foreach(GameObject child in _children)
			{
				names[i++] = child.sc_name;
			}
			System.Console.WriteLine(this + ": " + string.Join(",", names));
			foreach(Component comp in _components)
			{
				System.Console.WriteLine(comp.ToString());
				comp.LogMembers(1);
			}
		}
		public GameObject this[string childName]
		{
			get
			{
				if(Destroyed) ThrowAccessError();
				foreach(GameObject child in _children)
				{
					if(childName == child.sc_name)
						return child;
				}
				ThrowNotExistError<GameObject>(childName);
				return null;
			}
			set
			{
				if(Destroyed) ThrowAccessError();
				value.sc_name = childName;
				AddChild(value);
			}
		}
		public override string ToString()
		{
			if(this == WORLD) return "WORLD";
			return sc_parent + "." + (Destroyed ? sc_name + "(destroyed)" : sc_name);
		}
	}
}

