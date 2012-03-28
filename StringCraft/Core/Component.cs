using System;
using System.Reflection;
using System.Collections.Generic;

namespace StringCraft
{
	public enum DestructionResponse : byte
	{
		Allow,
		Cancel
	}
	
	internal delegate void UpdateMethod();
	
	public abstract class Component : StringCraftObject
	{
		internal static readonly Type sc_componentType = typeof(Component);
		internal static KeyedByTypeCollection<Component> sc_singletons = new KeyedByTypeCollection<Component>();
		internal static Type sc_updateMethodType = typeof(UpdateMethod);
		internal bool sc_exists;
		internal LinkedListNode<UpdateMethod> sc_updateListNodeInParent;
		public readonly GameObject Gameobject;
		public Component ()
		{
			if(GameObject.sc_isAddingAComponent == null)
			{
				throw new InvalidOperationException
				(
					"Do not instantiate components directly, instead add them to GameObjects either " +
					"with the GameObject.AddComponent() method or when instantiating GameObjects."
				);
			}
			if(this is ISingletonComponent)
			{
				Type thisType = this.GetType();
				if(sc_singletons.Contains(thisType))
				{
					throw new InvalidOperationException
					(
						"Singleton Component of type " + thisType + " that implements the" +
						"ISingletonComponent interface can only have one instance."
						
					);
				}
				sc_singletons.Add(this);
			}
			Gameobject = GameObject.sc_isAddingAComponent;
			sc_exists = true;
			sc_updateListNodeInParent = null;
		}
		public static implicit operator bool(Component component)
		{
			return component.Exists;
		}
		public bool Exists{ get{ return sc_exists; } }
		public bool Destroyed{ get{ return !sc_exists; } }
		public String Name
		{
			get
			{
				return this.GetShortTypeName();
			}
		}
		
		/*public DestructionResponse Destroy()
		{
			Type thisType = this.GetType();
			return Gameobject.RemoveComponent<thisType>();
		}*/
		
		public CompType GetComponent<CompType>() where CompType : Component
		{
			return Gameobject.GetComponent<CompType>();
		}
		public CompType AddComponent<CompType>() where CompType : Component
		{
			return Gameobject.AddComponent<CompType>();
		}
		public DestructionResponse RemoveComponent<CompType>() where CompType : Component
		{
			return Gameobject.RemoveComponent<CompType>();
		}
		
		internal void CallAwake()
		{
			this.Call("Awake");
		}
		internal void CallStart()
		{
			this.Call("Start");
		}
		internal UpdateMethod GetUpdateMethod()
		{
			if(this.HasMethod("Update"))
				return (UpdateMethod)Delegate.CreateDelegate(sc_updateMethodType, this, "Update");
			else return null;
		}
		internal DestructionResponse CallDestroy()
		{
			DestructionResponse response = this.Call<DestructionResponse>("Destroy");
			if(response == DestructionResponse.Allow)
			{
				if(this is ISingletonComponent)
				{
					sc_singletons.Remove(this);
				}
			}
			return response;
		}
		internal void ThrowAccessError()
		{
			if(Gameobject.Exists)
				throw new InvalidOperationException("The Component " + this + " has been destroyed but you're still trying to access it.");
			else
				throw new InvalidOperationException("The GameObject " + Gameobject + " has been destroyed but you're still trying to access it's Component: " + this);
		}
		internal void CheckAccess()
		{
			if(Destroyed) ThrowAccessError();
		}
		public DestructionResponse Destroy()
		{
			CheckAccess();
			return Gameobject.RemoveComponent(this);
		}
		public void Log()
		{
			System.Console.WriteLine(this + ": ");
			LogMembers(1);
		}
		public void LogMembers(int indent = 0)
		{
			LogMembers("\t".Times(indent));
		}
		public void LogMembers(string indent)
		{
			{
				PropertyInfo[] infos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
				foreach(PropertyInfo info in infos)
				{
					if(info.CanRead && info.DeclaringType != sc_componentType)
						System.Console.WriteLine(indent + info.PropertyType.GetShortName().ToMinLength(15, " ") + " " + info.Name + ": " + info.GetValue(this, null));
				}
			}
			{
				FieldInfo[] infos = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
				foreach(FieldInfo info in infos)
				{
					if(info.DeclaringType != sc_componentType)
						System.Console.WriteLine(indent + info.FieldType.GetShortName().ToMinLength(15, " ") + " " + info.Name + ": " + info.GetValue(this));
				}
			}
		}
		public override string ToString()
		{
			return (Gameobject != null ? Gameobject.ToString() : "NULL") + "::" + (Destroyed ? Name + "(destroyed)" : Name);
		}
	}
}

