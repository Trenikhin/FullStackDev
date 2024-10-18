namespace ShootEmUp
{
    using UnityEngine;

    
	public class EnemyBrain : MonoBehaviour
    {
        [SerializeField] Ship _ship;
        
        [SerializeField] float         _countdown = 1;
        
        Ship    _target;
        Vector2 _destination;
        float   _currentTime;
        bool    _isPointReached;


        public void Reset() => _currentTime = _countdown;
        

        void FixedUpdate()
        {
            if (_isPointReached)
                TryAttack();
            else
                TryMove();
        }

        public void Init(Ship playerShip, Vector2 endPoint)
        {
            _target         = playerShip;
            _destination    = endPoint;
            _isPointReached = false;
        }
        

        void TryMove()
        {
            Vector2 vector = _destination - (Vector2) transform.position;
            if (vector.magnitude <= 0.25f)
            {
                _isPointReached = true;
                return;
            }

            _ship.Move( vector.normalized );
        }

        void TryAttack()
        {
            if (_target.Health.Value <= 0)
                return;

            _currentTime -= Time.fixedDeltaTime;
            
            if (_currentTime <= 0)
            {
                Vector2 startPosition = transform.position;
                Vector2 vector        = (Vector2) _target.transform.position - startPosition;
                Vector2 direction     = vector.normalized;

                _ship.Fire( direction );
                
                _currentTime += _countdown;
            }
        }
	}
}