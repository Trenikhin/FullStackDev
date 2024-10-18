namespace ShootEmUp
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public sealed class BulletPool : MonoBehaviour
    {
        [SerializeField] Bullet      prefab;
        [SerializeField] Transform   worldTransform;
        [SerializeField] Transform   container;

        readonly HashSet<Bullet> m_activeBullets = new();
        readonly Queue<Bullet>   m_bulletPool    = new();
        
        void Awake()
        {
            for (var i = 0; i < 10; i++)
            {
                Bullet bullet = Instantiate(this.prefab, this.container);
                this.m_bulletPool.Enqueue(bullet);
            }
        }
        
#region IBulletPool
        
        public IReadOnlyCollection<Bullet> ActiveBullets => m_activeBullets;
        
        
        public Bullet Rent()
        {
            if (this.m_bulletPool.TryDequeue(out var bullet))
                bullet.transform.SetParent(this.worldTransform);
            else
                bullet = Instantiate(this.prefab, this.worldTransform);

            m_activeBullets.Add( bullet );
            bullet.Activate();
            
            return bullet;
        }

        
        public void Return(Bullet bullet)
        {
            if (this.m_activeBullets.Remove(bullet))
            {
                bullet.Deactivate();
                bullet.transform.SetParent(this.container);
                this.m_bulletPool.Enqueue(bullet);
            }
        }
        
#endregion 
    }
}