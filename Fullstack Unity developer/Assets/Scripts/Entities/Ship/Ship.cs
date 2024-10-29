namespace ShootEmUp
{
    using UnityEngine;
    
    public sealed class Ship : MonoBehaviour
    {
        // Components
        [SerializeField] Health _health;
        [SerializeField] Mover _mover;
        [SerializeField] Gun _gun;

        public void Init()
        {
            Health.ResetHealth();
        }
        
        public Health Health => _health;
        
        public void Move(Vector2  moveDirection) => _mover.Move( moveDirection );
        public void Fire( Vector2 direction ) => _gun.Fire( direction );
    }
}