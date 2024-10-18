using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D    rigidbody2D;
        [SerializeField] SpriteRenderer spriteRenderer;
        
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
            
            transform.position   = pos;
            spriteRenderer.color = color;
            gameObject.layer     = physicsLayer;
            rigidbody2D.velocity = velocity;
        }

#endregion
    }
}