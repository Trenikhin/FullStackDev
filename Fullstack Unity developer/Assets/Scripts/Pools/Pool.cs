namespace ShootEmUp
{
	using UnityEngine;
	using System.Collections.Generic;

	
	public class Pool<T> : MonoBehaviour where T : Component
	{
		[SerializeField] T _prefab;
		[SerializeField] Transform _worldTransform;
		[SerializeField] Transform _container;
		
		readonly HashSet<T> _activeObjs = new();
		readonly Queue<T>   _objs    = new();
        
        
		public T Rent()
		{
			if (_objs.TryDequeue(out var obj))
				obj.transform.SetParent(_worldTransform);
			else
				obj = Instantiate(_prefab, _worldTransform);

			_activeObjs.Add( obj );
			OnActivate( obj );
            
			return obj;
		}

        
		public void Return(T obj)
		{
			if (_activeObjs.Remove(obj))
			{
				OnDeactivate(obj);
				obj.transform.SetParent(_container);
				_objs.Enqueue(obj);
			}
		}

		protected virtual void OnActivate( T obj )
		{
			obj.gameObject.SetActive( true );
		}

		protected virtual void OnDeactivate( T obj )
		{
			obj.gameObject.SetActive( false );
		}
	}
}