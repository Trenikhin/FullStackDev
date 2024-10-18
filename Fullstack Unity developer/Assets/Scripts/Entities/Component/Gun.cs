namespace ShootEmUp
{
	using UnityEngine;
	
	public class Gun : MonoBehaviour
	{
		[Header( "Настройки пули" )]
		// Logic
		[SerializeField] int          _damage = 1;
		[SerializeField] PhysicsLayer _physicsLayer;
		[SerializeField] Transform    _firePoint;
		
		// View
		[SerializeField] Color _color = Color.blue;

		BulletSpawner _bulletSpawner;
		
		void Start()
		{
			_bulletSpawner = ServiceLocator.Instance.Get<BulletSpawner>();
		}


		#region IGun

		public Quaternion FirePointRotation => _firePoint.rotation;
		public Vector3    Position          => _firePoint.position;
		
		public void Fire( Vector2 velocity )
		{
			_bulletSpawner.SpawnBullet(
				_firePoint.position,
				_color,
				(int) _physicsLayer,
				_damage,
				velocity
			);
		}

#endregion		
	}
}