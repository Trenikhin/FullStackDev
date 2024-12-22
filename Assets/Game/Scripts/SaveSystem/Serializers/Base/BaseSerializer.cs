namespace Game.SaveSystem
{
	using System.Collections.Generic;
	using UnityEngine;
	using Zenject;

	public interface ISerializer
	{
		void Serialize(Dictionary<string, string> data);
		void Deserialize(Dictionary<string, string> data);
	}
	
	public abstract class BaseSerializer<TComponent, TData> : ISerializer
	where TComponent : Component
	{
		[Inject] ISerializeHelper _serializeHelper;
		
		protected virtual string Key => typeof(TComponent).Name;

		public void Serialize(Dictionary<string, string> data)
		{
			_serializeHelper.Serialize<TComponent, TData>
			(
				Key,
				data,
				Get
			);
		}

		public void Deserialize(Dictionary<string, string> data)
		{
			_serializeHelper.Deserialize<TComponent, TData>
			(
				Key,
				data,
				v => Set(v.obj, v.data)
			);
		}

		protected abstract TData Get( TComponent component );
		protected abstract void Set( TComponent component, TData data );
	}
}