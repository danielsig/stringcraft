using System;
using System.Reflection;

namespace StringCraft
{
	public enum DestructionResponse : byte
	{
		Allow,
		Cancel
	}
	
	internal delegate void UpdateMethod();
	
	public class Component : StringCraftObject
	{
		internal static Type sc_updateMethodType = typeof(UpdateMethod);
		internal bool sc_exists;
		public readonly GameObject Gameobject;
		public Component ()
		{
			if(GameObject.sc_isAddingAComponent == null)
			{
				throw new InvalidOperationException
				(
					"Do not instantiate components directly, instead add them to GameObjects either" +
					"with the GameObject.AddComponent() method or when instantiating GameObjects."
				);
			}
			Gameobject = GameObject.sc_isAddingAComponent;
			sc_exists = true;
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
			return this.Call<DestructionResponse>("Destroy");
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
		public void Log()
		{
			System.Console.WriteLine(this + ": ");
			LogMembers(1);
		}
		public void LogMembers(int indent = 0)
		{
			string indentString = "\t".Times(indent);
			
			PropertyInfo[] infos = this.GetType().GetProperties();
			foreach(PropertyInfo info in infos)
			{
				if(info.CanRead)
					System.Console.WriteLine(indentString + info.PropertyType.GetShortName() + " " + info.Name + ": " + info.GetValue(this, null));
			}
		}
		public override string ToString()
		{
			return (Gameobject != null ? Gameobject.ToString() : "NULL") + "::" + (Destroyed ? Name + "(destroyed)" : Name);
		}
	}
}

