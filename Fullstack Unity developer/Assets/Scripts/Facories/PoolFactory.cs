namespace ShootEmUp
{
    using UnityEngine;
    
    public abstract class PoolFactory<T> : MonoBehaviour
        where T: Component
    {
        [SerializeField] Pool<T> _pool;

        public virtual void Destroy( T enemy ) =>  _pool.Return( enemy );
        
        protected T Get() => _pool.Rent();
    }
}