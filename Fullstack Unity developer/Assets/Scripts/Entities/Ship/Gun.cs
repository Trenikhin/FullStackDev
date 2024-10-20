namespace ShootEmUp
{
	using UnityEngine;
	
	public class Gun : MonoBehaviour
	{
		[SerializeField] int          _damage = 1;
		[SerializeField] PhysicsLayer _physicsLayer;
		[SerializeField] Transform    _firePoint;
		[SerializeField] float        _velocity;
		[SerializeField] Color _color = Color.blue;

		BulletSpawner _bulletSpawner;
		
		void Start()
		{
			_bulletSpawner = ServiceLocator.Instance.Get<BulletSpawner>();
		}


#region IGun

		public void Fire( Vector2 direction )
		{
			_bulletSpawner.SpawnBullet(
				_firePoint.position,
				_color,
				(int) _physicsLayer,
				_damage,
				direction * _velocity
			);
		}

#endregion		
	}
}