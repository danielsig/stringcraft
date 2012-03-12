using System;
using System.Collections.Generic;
using System.Reflection;

namespace StringCraft
{
	public class GameObject : StringCraftObject
	{
		static GameObject()
		{
			WORLD = new GameObject();
			WORLD.sc_name = "WORLD";
			WORLD.sc_parent = WORLD;
		}
		
		public static readonly GameObject WORLD;
		private static readonly Type ComponentType = typeof(Component);
		
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
					sc_parent._children.Remove(sc_name);
					sc_parent._children.Add(value, this);
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
					sc_parent._children.Remove(sc_name);
					value._children.Add(sc_name, this);
					sc_parent = value;
				}
				else throw new InvalidOperationException("Can not assign a new parent to GameObject.WORLD");
			}
		}
		public bool Exists{ get{ return sc_parent != null; } }
		public bool Destroyed{ get{ return sc_parent == null; } }
		
		internal String sc_name = "Untitled";
		internal Vector2 sc_pos = Vector2.ZERO;
		internal Vector2 sc_locPos = Vector2.ZERO;
		internal GameObject sc_parent = WORLD;
		internal static GameObject sc_isAddingAComponent = null;
		private Dictionary<Type, UpdateMethod> _updates;
		private Queue<Type> _componentsAddedOnLastUpdate;
		
		
		private Dictionary<Type, Component> _components;
		private Dictionary<String, GameObject> _children;
		
		public GameObject (String name, GameObject parent) : this()
		{
			Name = name;
			Parent = parent;
		}
		public GameObject (GameObject parent) : this()
		{
			Parent = parent;
		}
		public GameObject (String name) : this()
		{
			Name = name;
		}
		public GameObject ()
		{
			//empty constructor
			_updates = new Dictionary<Type, UpdateMethod>();
			_componentsAddedOnLastUpdate = new Queue<Type>();
			_components = new Dictionary<Type, Component>();
			_children = new Dictionary<String, GameObject>();
		}
		public static implicit operator bool(GameObject gameObject)
		{
			return gameObject != null && gameObject.Exists;
		}
		public GameObject AddChild(GameObject child)
		{
			child.Parent = this;
			return child;
		}
		public CompType AddComponent<CompType>() where CompType : Component
		{
			if(Destroyed) ThrowAccessError();
			Type compType = typeof(CompType);
			//checking for conflicts
			if(_components.ContainsKey(compType))
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
			_components.Add(compType, instance);
			
			//calling awake
			instance.CallAwake();
			
			//adding to the queue
			_componentsAddedOnLastUpdate.Enqueue(compType);
			
			return instance as CompType;
		}
		public void RemoveComponent<CompType>() where CompType : Component
		{
			if(Destroyed) ThrowAccessError();
			Type compType = typeof(CompType);
			if(_components.ContainsKey(compType))
			{
				if(_components[compType].CallDestroy() == DestructionResponse.Allow)
				{
					_components[compType].sc_exists = false;
					_updates.Remove(compType);
					_components.Remove(compType);
				}
			}
		}
		public void Destroy()
		{
			foreach(KeyValuePair<Type, Component> pair in _components)
			{
				pair.Value.CallDestroy();
			}
			sc_parent._children.Remove(sc_name);
			sc_parent = null;
		}
		
		private void ThrowAccessError()
		{
			throw new InvalidOperationException("The GameObject " + this + " has been destroyed but you're still trying to access it.");
		}
		private void UpdateChildren()
		{
			sc_pos = sc_parent.sc_pos + sc_locPos;
			foreach(KeyValuePair<String, GameObject> child in _children)
			{
				child.Value.UpdateChildren();
			}
		}
		internal void UpdateMessage()
		{
			if(_componentsAddedOnLastUpdate.Count > 0)
			{
				foreach(Type compType in _componentsAddedOnLastUpdate)
				{
					if(_components.ContainsKey(compType))//does it still exist
					{
						Component instance = _components[compType];
						UpdateMethod updateMethod = instance.GetUpdateMethod();
						if(updateMethod != null) _updates.Add(compType, updateMethod);
						instance.CallStart();
					}
				}
				_componentsAddedOnLastUpdate.Clear();
			}
			foreach(KeyValuePair<Type, UpdateMethod> pair in _updates)
			{
				pair.Value();
			}
			foreach(KeyValuePair<String, GameObject> pair in _children)
			{
				pair.Value.UpdateMessage();
			}
		}
		public void Log()
		{
			string[] names = new string[_children.Count];
			int i = 0;
			foreach(KeyValuePair<string, GameObject> child in _children)
			{
				names[i++] = child.Value.sc_name;
			}
			System.Console.WriteLine(this + ": " + string.Join(",", names));
			foreach(KeyValuePair<Type, Component> comp in _components)
			{
				System.Console.WriteLine(comp.Value.ToString());
				comp.Value.LogMembers(1);
			}
		}
		public override string ToString()
		{
			if(this == WORLD) return "WORLD";
			return sc_parent + "." + (Destroyed ? sc_name + "(destroyed)" : sc_name);
		}
	}
}

