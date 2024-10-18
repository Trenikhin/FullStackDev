namespace ShootEmUp
{
    using UnityEngine;  
	   
    public sealed class Player : MonoBehaviour
    {
        // Components
        [SerializeField] Health _health;
        [SerializeField] Mover  _mover;
        [SerializeField] Gun    _gun;

#region IPlayer
        
		public Health Health => _health;
        
        public void Move(Vector2 moveDirection)		=> _mover.Move( moveDirection );
        public void Fire()							=> _gun.Fire( _gun.FirePointRotation * Vector3.up * 3 );
        
#endregion
    }
}