namespace Game.SaveSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using Modules.Entities;
	using Sirenix.Utilities;
	using UnityEngine;
	using Zenject;

	public abstract class BaseComponentSerializer<TComponent, TData> : ISerializer
	where TComponent : Component
	{
		[Inject] EntityWorld _entityWorld;
		[Inject] ISerializeHelper _serializer;
		
		protected virtual string _key => typeof(TComponent).Name;

		public void Serialize(Dictionary<string, string> data)
		{
			var components = _entityWorld
				.GetAll()
				.Select(e => (e.Id, e.GetComponent<TComponent>()))
				.Where(d => d.Item2 != null)
				.ToDictionary(d => d.Item1, d => Get(d.Item2));
			
			data[_key] = _serializer.Serialize(components);;
		}

		public void Deserialize(Dictionary<string, string> data)
		{
			if (!data.TryGetValue(_key, out string d))
				return;
			
			_serializer
				.Deserialize<Dictionary<int, TData>>(d)
				.Select(c => (_entityWorld.Get(c.Key).GetComponent<TComponent>(), c.Value))
				.ForEach( v => Set(v.Item1, v.Item2) );
		}

		protected abstract TData Get( TComponent component );
		protected abstract void Set( TComponent component, TData data );
	}
}