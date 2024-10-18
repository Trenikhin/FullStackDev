namespace ShootEmUp
{
	using System.Linq;
	using UnityEngine;

	
	public class BulletCollector : MonoBehaviour
	{
		[SerializeField] BulletPool  bulletPool;
		[SerializeField] LevelBounds levelBounds;

		void FixedUpdate() => CollectBullets();

		
		void CollectBullets()
		{
			foreach (var bullet in bulletPool.ActiveObjs)
				if (!levelBounds.InBounds(bullet.transform.position))
					bulletPool.Return(bullet);
		}
	}
}