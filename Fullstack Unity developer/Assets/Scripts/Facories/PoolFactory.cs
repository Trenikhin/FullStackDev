namespace ShootEmUp
{
    using UnityEngine;
    
    public abstract class PoolFactory<T, P> : MonoBehaviour, IFactory<T, P> where T : MonoBehaviour
    {
        [SerializeField] Pool<T> _pool;

        public abstract T Create(P parameters);
        public virtual void Destroy( T enemy ) =>  _pool.Return( enemy );
        
        protected T Get() => _pool.Rent();
    }
}