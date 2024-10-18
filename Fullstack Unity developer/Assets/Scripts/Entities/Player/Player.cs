namespace ShootEmUp
{
    using System;
    using UnityEngine;  
    
    public sealed class Player : MonoBehaviour, IDamageable
    {
        [SerializeField]        BulletSpawner _bulletPool;
        [SerializeField]        Transform     firePoint;
        [SerializeField]        Rigidbody2D   _rigidbody;
        [SerializeField]        float         speed = 5.0f;
        
#region IPlayer
        
        public Action<Player, int> OnHealthChanged;
        public Action              OnHealthEmpty;

        [field: SerializeField] public int Health {get; private set;}
        
        
        public void Move( Vector2 moveDirection )
        {
            Vector2 moveStep       = moveDirection * Time.fixedDeltaTime * speed;
            Vector2 targetPosition = _rigidbody.position + moveStep;
            _rigidbody.MovePosition(targetPosition);
        }

        
        public void Fire()
        {
            _bulletPool.SpawnBullet(
                this.firePoint.position,
                Color.blue,
                (int) PhysicsLayer.PLAYER_BULLET,
                1,
                this.firePoint.rotation * Vector3.up * 3
            );
        }

        
        public void TakeDamage(int damage)
        {
            if (Health <= 0)
                return;

            Health = Mathf.Max(0, Health - damage);
            OnHealthChanged?.Invoke(this, Health);

            if (Health <= 0)
                OnHealthEmpty?.Invoke();
        }
        
#endregion
    }
}