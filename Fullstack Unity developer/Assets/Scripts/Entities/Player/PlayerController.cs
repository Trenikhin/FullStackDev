namespace ShootEmUp
{
	using UnityEngine;
	
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] Player       _player;
		
		InputHandler _inputHandler;

		void OnEnable()
		{
			_inputHandler = ServiceLocator.Instance.Get<InputHandler>();
			
			_inputHandler.OnAttack += _player.Fire;
			_inputHandler.OnMove   += _player.Move;
		}
		
		void OnDisable()
		{
			_inputHandler.OnAttack -= _player.Fire;
			_inputHandler.OnMove   -= _player.Move;
		}
	}
}