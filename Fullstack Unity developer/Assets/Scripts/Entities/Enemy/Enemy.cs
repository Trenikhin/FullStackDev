namespace ShootEmUp
{
    using UnityEngine;
    
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] Health _health;
        [SerializeField] Mover  _mover;
        [SerializeField] Gun    _gun;
        
        [SerializeField] float         _countdown = 1;
        
        Player  _target;
        Vector2 _destination;
        float   _currentTime;
        bool    _isPointReached;
        
#region Editor

        public void Reset() => _currentTime = _countdown;

#endregion
#region UnityEvents

        void FixedUpdate()
        {
            if (_isPointReached)
                TryAttack();
            else
                TryMove();
        }

#endregion
#region IEnemy

        public Health Health => _health;

        public void Init(Player player, Vector2 endPoint)
        {
            _target         = player;
            _destination    = endPoint;
            _isPointReached = false;
        }

#endregion

        void TryMove()
        {
            Vector2 vector = _destination - (Vector2) transform.position;
            if (vector.magnitude <= 0.25f)
            {
                _isPointReached = true;
                return;
            }
            
            _mover.Move( vector.normalized );
        }

        void TryAttack()
        {
            if (_target.Health.Value <= 0)
                return;

            _currentTime -= Time.fixedDeltaTime;
            
            if (_currentTime <= 0)
            {
                Vector2 startPosition = _gun.Position;
                Vector2 vector        = (Vector2) _target.transform.position - startPosition;
                Vector2 direction     = vector.normalized;
                
                _gun.Fire( direction * 2 );
                _currentTime += _countdown;
            }
        }
    }
}