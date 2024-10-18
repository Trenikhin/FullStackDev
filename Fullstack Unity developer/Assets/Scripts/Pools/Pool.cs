namespace ShootEmUp
{
	using UnityEngine;
	using System.Collections.Generic;

	
	public class Pool<T> : MonoBehaviour where T : Component
	{
		[SerializeField] T         prefab;
		[SerializeField] Transform worldTransform;
		[SerializeField] Transform container;
		
		readonly HashSet<T> m_activeObjs = new();
		readonly Queue<T>   m_Objs    = new();
        
		public IReadOnlyCollection<T> ActiveObjs => m_activeObjs;
        
        
		public T Rent()
		{
			if (this.m_Objs.TryDequeue(out var obj))
				obj.transform.SetParent(this.worldTransform);
			else
				obj = Instantiate(this.prefab, this.worldTransform);

			m_activeObjs.Add( obj );
			OnActivate( obj );
            
			return obj;
		}

        
		public void Return(T obj)
		{
			if (this.m_activeObjs.Remove(obj))
			{
				OnDeactivate(obj);
				obj.transform.SetParent(this.container);
				this.m_Objs.Enqueue(obj);
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