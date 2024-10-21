namespace ShootEmUp
{
	using System;
	using System.Linq;
	using UnityEngine;
	using System.Collections.Generic;
	
	public sealed class ActivePool<T> where T: MonoBehaviour
	{
		readonly List<T> _activeObjs = new List<T>();
		readonly Pool<T> _pool;

		public ActivePool( Pool<T> pool )
		{
			_pool = pool;
		}
		
		public T Rent()
		{
			var obj = _pool.Rent();
			_activeObjs.Add( obj );

			return obj;
		}
        
		public void Return( T obj)
		{
			_activeObjs.Remove( obj );
			_pool.Return( obj );
		}

		public List<T> GetCopy( Func<T, bool> condition )
		{
			return _activeObjs
				.Where(condition)
				.ToList();
		}
	}
}