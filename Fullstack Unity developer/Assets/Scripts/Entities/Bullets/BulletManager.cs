namespace ShootEmUp
{
	using UnityEngine;
	
	public class BulletManager : EntityManager<Bullet, BulletParams>
	{
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
			var prms = new BulletParams(damage, physicsLayer, color, velocity, position);
			var obj = Spawn( prms );
			
			obj.OnDestroy += Return;
		}
		
		void Return( Bullet bullet )
		{
			bullet.OnDestroy -= Return;
			
			Destroy( bullet );
		}
		
		void CollectBullets()
		{
			foreach (var bullet in _activeObjs.ToArray()) // collection could be modified
				if (!_levelBounds.InBounds(bullet.Pos))
					Return( bullet );;
		}
	}
}