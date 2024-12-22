namespace Game.SaveSystem
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Modules.Entities;
	using Sirenix.Serialization;
	using Sirenix.Utilities;
	using UnityEngine;
	using Zenject;

	public interface ISerializer
	{
		void Serialize(Dictionary<string, string> data);
		void Deserialize(Dictionary<string, string> data);
	}
	
	public abstract class BaseComponentSerializer<TComponent, TData> : ISerializer
	where TComponent : Component
	{
		[Inject] EntityWorld _entityWorld;

		bool _dontCache = true; // Enable/ Disable caching
		Dictionary<int, Dictionary< Type, Component >> _cachedComponents = new ();
		
		protected virtual string Key => typeof(TComponent).Name;

		public void Serialize(Dictionary<string, string> data)
		{
			var components = GetWorldComponents<TComponent>()
				.ToDictionary(d => d.Item1, d => Get(d.Item2));

			Serialize(data, components, Key);
		}

		public void Deserialize(Dictionary<string, string> data)
		{
			if (!data.TryGetValue(Key, out string d))
				return;
					
			Deserialize<TData>(d)
				.Select(c => (_entityWorld.Get(c.Key).GetComponent<TComponent>(), c.Value))
				.ForEach( v => Set(v.Item1, v.Item2) );
		}

		protected abstract TData Get( TComponent component );
		protected abstract void Set( TComponent component, TData data );

		IEnumerable<(int, T)> GetWorldComponents<T>() where T : Component
		{
			return _entityWorld
				.GetAll()
				.Select(e => (e.Id, GetComponent<T>( e.Id, e )))
				.Where(d => d.Item2 != null);
		}
		
		void Serialize<T>( Dictionary<string, string> data, T components, string id )
		{
			byte[] bytes = SerializationUtility.SerializeValue(components, DataFormat.Binary);
			
			data[id] = Convert.ToBase64String(bytes);
		}

		Dictionary<int, T> Deserialize<T>( string data )
		{
			byte[] bytes = Convert.FromBase64String(data);
			
			return SerializationUtility.DeserializeValue<Dictionary<int, T>>(bytes, DataFormat.Binary);
		}
		
		T GetComponent<T>(int objectId, Entity entity) where T : Component
		{
			if (_dontCache)
				return entity.GetComponent<T>();
			
			var type = typeof(T);
			
			if (!_cachedComponents.TryGetValue(objectId, out var componentDict))
			{
				componentDict = new Dictionary<Type, Component>();
				_cachedComponents[objectId] = componentDict;
			}

			if (componentDict.TryGetValue(type, out var component))
				return component as T;
			
			component = entity.GetComponent<T>();
				
			if (component != null)
				componentDict[type] = component;
			
			return (T)component;
		}
	}
}