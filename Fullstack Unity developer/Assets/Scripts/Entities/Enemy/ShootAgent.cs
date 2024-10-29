namespace ShootEmUp
{
	using UnityEngine;


	public class ShootAgent : MonoBehaviour
	{
		[SerializeField] float _countdown = 1;
		[SerializeField] Ship _ship;

		Ship _target;

		float _currentTime;
		bool  _isPointReached;

		void Reset() => _currentTime = _countdown;

		public void Setup(Ship tgt)
		{
			_target = tgt;
		}

		public bool TryAttack()
		{
			if ( _target.Health.Value <= 0 )
				return false;

			_currentTime -= Time.fixedDeltaTime;

			if ( _currentTime <= 0 )
			{
				Vector2 startPosition = transform.position;
				Vector2 vector        = (Vector2)_target.transform.position - startPosition;
				Vector2 direction     = vector.normalized;
				
				_ship.Fire( direction );
				_currentTime += _countdown;
				
				return true;
			}

			return false;
		}	
	}
}