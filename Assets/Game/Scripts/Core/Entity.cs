namespace Game.Entities
{
	using UnityEngine;
	using Zenject;
	
	public class Entity
	{
		DiContainer _container;
		GameObject _gameObject;

		public Entity( GameObject gameObject, DiContainer container )
		{
			_container = container;
			_gameObject = gameObject;
		}
		
		public bool TryGet<T>(out T component) where T : class
		{
			component = _container.TryResolve<T>();
			
			return component != null;
		}
		
		public void Destroy()
		{
			Object.Destroy(_gameObject);
		}
	}
}