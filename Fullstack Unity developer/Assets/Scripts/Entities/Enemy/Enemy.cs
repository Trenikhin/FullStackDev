using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class Enemy : MonoBehaviour, IDamageable
    {
        public delegate void FireHandler(Vector2 position, Vector2 direction);
        
        public event FireHandler OnFire;
        
        [SerializeField] public Transform     firePoint;
        [SerializeField]        Rigidbody2D   _rigidbody;
        [SerializeField]        float         speed = 5.0f;
        [SerializeField]        float         countdown;
        [SerializeField]        BulletSpawner _bulletSystem;
        
        Player target;

        Vector2 destination;
        float currentTime;
        bool isPointReached;

        void FixedUpdate()
        {
            if (this.isPointReached)
                Attack();
            else
                Move();
        }
        
        [field: SerializeField] public int Health {get; private set;}

        public void Init(Player player, Vector2 endPoint)
        {
            target              = player;
            
            this.destination    = endPoint;
            this.isPointReached = false;
        }
        
        public void Reset()
        {
            this.currentTime = this.countdown;
        }

        public void TakeDamage(int damage)
        {
            if (Health > 0)
                Health = Mathf.Max(0, Health - damage);
        }

        void Move()
        {
            Vector2 vector = this.destination - (Vector2) this.transform.position;
            if (vector.magnitude <= 0.25f)
            {
                this.isPointReached = true;
                return;
            }

            Vector2 direction    = vector.normalized * Time.fixedDeltaTime;
            Vector2 nextPosition = _rigidbody.position + direction * speed;
            _rigidbody.MovePosition(nextPosition);
        }

        void Attack()
        {
            if (this.target.Health <= 0)
                return;

            this.currentTime -= Time.fixedDeltaTime;
            
            if (this.currentTime <= 0)
            {
                Vector2 startPosition = this.firePoint.position;
                Vector2 vector        = (Vector2) this.target.transform.position - startPosition;
                Vector2 direction     = vector.normalized;
                
                _bulletSystem.SpawnBullet(
                    startPosition,
                    Color.red,
                    (int) PhysicsLayer.ENEMY_BULLET,
                    1,
                    direction * 2
                );
                
                    
                this.currentTime += this.countdown;
            }
        }
    }
}