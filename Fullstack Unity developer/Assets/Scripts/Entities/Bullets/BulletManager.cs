namespace ShootEmUp
{
	using System.Linq;
	using UnityEngine;
	
	public class BulletManager : MonoBehaviour
	{
		[SerializeField] LevelBounds _levelBounds;
		[SerializeField] BulletPool _pool;
		
		ActivePool<Bullet> _activePool;
		
		void Awake() => _activePool = new ActivePool<Bullet>(_pool);

		void FixedUpdate()
		{
			_activePool
				.GetCopy( IsOutOffBounds )
				.ForEach( Return );	
		}


		public void SpawnBullet
		(
			Vector2 position,
			Color   color,
			int     physicsLayer,
			int     damage,
			Vector2 velocity
		)
		{
			var bullet = _activePool.Rent();
			bullet.Init( damage, position, color, physicsLayer, velocity );
			bullet.OnDestroy += Return;
		}
		
		void Return( Bullet bullet )
		{
			bullet.OnDestroy -= Return;
			_activePool.Return(bullet);
		}

		bool IsOutOffBounds(Bullet bullet) => !_levelBounds.InBounds(bullet.Pos);
	}
}