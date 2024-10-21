namespace Entities
{
	using UnityEngine;
	using System.Collections.Generic;
	
	public abstract class EntityManager<T> : MonoBehaviour where T: Component
	{
		protected readonly List<T> _activeObjs = new List<T>();
		
		protected void OnSpawn(T cache)
		{
			_activeObjs.Add( cache );
		}
        
		protected void OnObjDestroy( T obj)
		{
			_activeObjs.Remove( obj );
		}
	}
}