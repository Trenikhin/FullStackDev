namespace ShootEmUp
{
	using Entities;
	using UnityEngine;
	
	public class BulletManager : EntityManager<Bullet>
	{
		[SerializeField] LevelBounds _levelBounds;
		
		IFactory<Bullet, BulletParams> _bulletFactory;

		void Start()
		{
			_bulletFactory = ServiceLocator.Instance.Get<BulletFactory>();
		}

		void FixedUpdate() => CollectBullets();
		
		public void SpawnBullet
		(
			Vector2 position,
			Color   color,
			int     physicsLayer,
			int     damage,
			Vector2 velocity
		)
		{
			var prms = new BulletParams(damage, physicsLayer, color, velocity, position);
			Bullet bullet = _bulletFactory.Create( prms );
			
			bullet.OnDestroy += Return;
			
			OnSpawn( bullet );
		}
		
		void Return( Bullet bullet )
		{
			bullet.OnDestroy -= Return;
			
			_bulletFactory.Destroy( bullet );
			OnObjDestroy( bullet );
		}
		
		void CollectBullets()
		{
			foreach (var bullet in _activeObjs.ToArray()) // collection could be modified
				if (!_levelBounds.InBounds(bullet.transform.position))
					Return( bullet );;
		}
	}
}