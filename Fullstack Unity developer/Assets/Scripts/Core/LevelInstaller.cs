namespace ShootEmUp
{
	using UnityEngine;
	
	public class LevelInstaller : MonoBehaviour
	{
		[SerializeField] BulletSpawner _bulletSpawner;
		[SerializeField] InputHandler  _inputHandler;
		
		void Awake()
		{
			ServiceLocator.Instance.Register(_bulletSpawner);
			ServiceLocator.Instance.Register(_inputHandler);
		}
	}
}