namespace ShootEmUp
{
	using System.Linq;
	using UnityEngine;
	
	public class BulletSpawner : MonoBehaviour
	{
		[SerializeField] BulletPool  _bulletPool;
		[SerializeField] LevelBounds _levelBounds;

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
			Bullet bullet = _bulletPool.Rent();
				
			bullet.Init( damage, position, color, physicsLayer, velocity );
			
			bullet.OnCollisionEntered += Return;
			
			return;
			void Return()
			{
				bullet.OnCollisionEntered -= Return;
				_bulletPool.Return( bullet );
			}
		}
		
		void CollectBullets()
		{
			foreach (var bullet in _bulletPool.ActiveObjs.ToArray()) // collection could be modified
				if (!_levelBounds.InBounds(bullet.transform.position))
					_bulletPool.Return(bullet);
		}
	}
}