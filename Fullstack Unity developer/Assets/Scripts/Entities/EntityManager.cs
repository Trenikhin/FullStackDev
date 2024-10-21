namespace ShootEmUp
{
	using UnityEngine;
	using System.Collections.Generic;
	
	public abstract class EntityManager<T, P> : MonoBehaviour where T: MonoBehaviour
	{
		protected readonly List<T> _activeObjs = new List<T>();

		IFactory<T, P> _factory;

		void Awake() => _factory = GetComponent<IFactory<T, P>>();

		protected T Spawn( P @params)
		{
			var obj = _factory.Create( @params );
			_activeObjs.Add( obj );

			return obj;
		}
        
		protected void Destroy( T obj)
		{
			_activeObjs.Remove( obj );
			_factory.Destroy( obj );
		}
	}
}