namespace ShootEmUp
{
    using System;
    using UnityEngine;
    
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D    _rigidbody2D;
        [SerializeField] SpriteRenderer _spriteRenderer;
        
        int  _damage;
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable entity))
                entity.TakeDamage( _damage );
            
            OnCollisionEntered?.Invoke();
        }

 #region IBullet

        public event Action OnCollisionEntered;
        
        
        public void Init( int damage, Vector3 pos, Color color, int physicsLayer, Vector2 velocity )
        {
            _damage   = damage;

            _spriteRenderer.color = color;
            _rigidbody2D.velocity = velocity;
            
            transform.position    = pos;
            gameObject.layer      = physicsLayer;
        }

#endregion
    }
}