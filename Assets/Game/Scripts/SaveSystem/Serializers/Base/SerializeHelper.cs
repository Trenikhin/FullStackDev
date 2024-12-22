namespace Game.SaveSystem
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Sirenix.Serialization;
	using Sirenix.Utilities;
	using Modules.Entities;
	using UnityEngine;
	using Zenject;

	public interface ISerializeHelper
	{
		void Serialize<TComponent, TParams>
		(
			string id,
			Dictionary<string, string> data,
			Func<TComponent, TParams> valueSelector
		) where TComponent : Component;
		
		void Deserialize<TComponent, TData>
		(
			string id,
			Dictionary<string, string> data,
			Action<(TComponent obj, TData data)> callback
		);
	}
	
	public class SerializeHelper : ISerializeHelper
	{
		[Inject] EntityWorld _entityWorld;

		bool _dontCache; // Enable/ Disable caching
		Dictionary<int, Dictionary< Type, Component >> _cachedComponents = new ();
		
		public void Serialize<T, TParams>(string id, Dictionary<string, string> data, Func<T, TParams> valueSelector)
		where T : Component
		{
			var components = GetWorldComponents<T>()
				.ToDictionary(d => d.Item1, d => valueSelector(d.Item2));
			
			Serialize(data, components, id);
		}

		public void Deserialize<TObj, TData>(string id, Dictionary<string, string> data, Action<(TObj, TData)> callback)
		{
			if (!data.TryGetValue(id, out string d))
				return;
					
			Deserialize<TData>(d)
				.Select(c => (_entityWorld.Get(c.Key).GetComponent<TObj>(), c.Value))
				.ForEach( v => callback(v) );
		}

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