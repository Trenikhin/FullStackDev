namespace ShootEmUp
{
    using UnityEngine;
    
    public interface IFactory<T, in P> where T : MonoBehaviour
    {
        T Create(P parameters);
        void Destroy(T obj);
    }
}