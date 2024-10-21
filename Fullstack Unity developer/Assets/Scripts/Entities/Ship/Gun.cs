namespace ShootEmUp
{
	using UnityEngine;
	
	public class Gun : MonoBehaviour
	{
		[SerializeField] int          _damage = 1;
		[SerializeField] EPhysicsLayer _physicsLayer;
		[SerializeField] Transform    _firePoint;
		[SerializeField] float        _velocity;
		[SerializeField] Color _color = Color.blue;

		BulletManager _bulletManager;
		
		void Start()
		{
			_bulletManager = ServiceLocator.Instance.Get<BulletManager>();
		}

		public void Fire( Vector2 direction )
		{
			_bulletManager.SpawnBullet(
				_firePoint.position,
				_color,
				(int) _physicsLayer,
				_damage,
				direction * _velocity
			);
		}
	}
}