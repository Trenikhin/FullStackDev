namespace Game.SaveSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using Modules.Entities;
	using Sirenix.Utilities;
	using UnityEngine;
	using Zenject;

	public class WorldSerializer : ISerializer
	{
		[Inject] EntityWorld _entityWorld;
		[Inject] EntityCatalog _catalog;
		
		[Inject] ISerializeHelper _serializer;
		
		const string _key = "World";
		
		public void Serialize(Dictionary<string, string> data)
		{
			Dictionary<int, (string, Vector3, Quaternion)> entitiesData =  _entityWorld
				.GetAll()
				.Select( e => (id: e.Id, name: e.Name, pos: e.transform.position, rot: e.transform.rotation) )
				.ToDictionary( d => d.id , d => (d.name, d.pos, d.rot) );
			
			data[_key] = _serializer.Serialize(entitiesData);
		}

		public void Deserialize(Dictionary<string, string> data)
		{
			var deserializeValue = _serializer
				.Deserialize<Dictionary<int, (string, Vector3, Quaternion)>>(data[_key]);
			
			// Despawn
			_entityWorld
				.GetAll()
				.Where( e => !deserializeValue.ContainsKey( e.Id ) )
				.ForEach( e => _entityWorld.Destroy( e ) );
			
			// Spawn
			deserializeValue
				.Where( kv => !_entityWorld.Has(kv.Key) )
				.Select( kv => (id: kv.Key, name: kv.Value.Item1, pos: kv.Value.Item2, ratation: kv.Value.Item3 ) )
				.ForEach(kv =>
				{
					_catalog.FindConfig(kv.name, out var cfg);
					_entityWorld.Spawn( cfg, kv.pos, kv.ratation, kv.id );
				});
		}
	}
}