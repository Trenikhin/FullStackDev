namespace ShootEmUp
{
	using UnityEngine;
	
	public class LevelInstaller : MonoBehaviour
	{
		[SerializeField] BulletManager _bulletManager;
		[SerializeField] InputHandler _inputHandler;
		
		[SerializeField] EnemyFactory _enemyFactory;
		[SerializeField] BulletFactory _bulletFactory;
		
		void Awake()
		{
			// Services
			ServiceLocator.Instance.Register(_bulletManager);
			ServiceLocator.Instance.Register(_inputHandler);
			
			// Factories
			ServiceLocator.Instance.Register(_enemyFactory);
			ServiceLocator.Instance.Register(_bulletFactory);
		}
	}
}