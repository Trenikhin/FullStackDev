namespace ShootEmUp
{
	using UnityEngine;
	
	public class BulletSpawner : MonoBehaviour
	{
		[SerializeField] BulletPool _bulletPool;
	
		
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
	}
}