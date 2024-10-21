namespace ShootEmUp
{
    using UnityEngine;

    
	public class EnemyBrain : MonoBehaviour
    {
        [SerializeField] MoveAgent _moveAgent;
        [SerializeField] ShootAgent _shootAgent;

        void FixedUpdate()
        {
            if (!_moveAgent.TryMove())
                _shootAgent.TryAttack();
        }

        public void Init(Ship playerShip, Vector2 endPoint)
        {
            _moveAgent.Setup( endPoint );
            _shootAgent.Setup( playerShip );
        }
	}
}