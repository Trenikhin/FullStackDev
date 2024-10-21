namespace ShootEmUp
{
	using UnityEngine;
	
	public class LevelInstaller : MonoBehaviour
	{
		[SerializeField] BulletManager _bulletManager;
		[SerializeField] InputHandler _inputHandler;
		
		void Awake()
		{
			// Services
			ServiceLocator.Instance.Register(_bulletManager);
			ServiceLocator.Instance.Register(_inputHandler);
		}
	}
}