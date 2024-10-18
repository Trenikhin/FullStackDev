namespace ShootEmUp
{
    using UnityEngine;
    
    public sealed class Ship : MonoBehaviour
    {
        // Components
        [SerializeField] Health _health;
        [SerializeField] Mover  _mover;
        [SerializeField] Gun    _gun;

#region IShip
        
        public Health Health => _health;
        
        public void Move(Vector2  moveDirection)	=> _mover.Move( moveDirection );
        public void Fire( Vector2 velocity )		=> _gun.Fire( velocity );
        
#endregion
    }
}