using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class Enemy : MonoBehaviour, IDamageable
    {
        public delegate void FireHandler(Vector2 position, Vector2 direction);
        
        public event FireHandler OnFire;

        [SerializeField]
        public bool isPlayer;
        
        [SerializeField]
        public Transform firePoint;
        
        [SerializeField] public int Health {get; private set;}

        [SerializeField]
        public Rigidbody2D _rigidbody;

        [SerializeField]
        public float speed = 5.0f;

        [SerializeField]
        private float countdown;

        [NonSerialized]
        public Player target;

        private Vector2 destination;
        private float currentTime;
        private bool isPointReached;

        public void Reset()
        {
            this.currentTime = this.countdown;
        }
        
        public void SetDestination(Vector2 endPoint)
        {
            this.destination = endPoint;
            this.isPointReached = false;
        }

        public void TakeDamage(int damage)
        {
            if (Health > 0)
                Health = Mathf.Max(0, Health - damage);
        }

        private void FixedUpdate()
        {
            if (this.isPointReached)
            {
                //Attack:
                if (this.target.Health <= 0)
                    return;

                this.currentTime -= Time.fixedDeltaTime;
                if (this.currentTime <= 0)
                {
                    Vector2 startPosition = this.firePoint.position;
                    Vector2 vector = (Vector2) this.target.transform.position - startPosition;
                    Vector2 direction = vector.normalized;
                    this.OnFire?.Invoke(startPosition, direction);
                    
                    this.currentTime += this.countdown;
                }
            }
            else
            {
                //Move:
                Vector2 vector = this.destination - (Vector2) this.transform.position;
                if (vector.magnitude <= 0.25f)
                {
                    this.isPointReached = true;
                    return;
                }

                Vector2 direction = vector.normalized * Time.fixedDeltaTime;
                Vector2 nextPosition = _rigidbody.position + direction * speed;
                _rigidbody.MovePosition(nextPosition);
            }
        }
    }
}