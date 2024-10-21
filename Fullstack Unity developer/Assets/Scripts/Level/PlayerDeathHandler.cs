using UnityEngine;

namespace ShootEmUp
{
	public class PlayerDeathHandler : MonoBehaviour
	{
		[SerializeField] Ship     _character;
		[SerializeField] GameFlow _gameFlow;
		
		void OnEnable()		=> _character.Health.OnHealthEmpty += _gameFlow.StopGame;
		void OnDisable()	=> _character.Health.OnHealthEmpty -= _gameFlow.StopGame;
	}
}