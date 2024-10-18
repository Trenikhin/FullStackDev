namespace ShootEmUp
{
	using UnityEngine;
	
	public class GameFlow : MonoBehaviour
	{
		[SerializeField] Ship _character;

		void OnEnable()		=> _character.Health.OnHealthEmpty += StopGame;
		void OnDisable()	=> _character.Health.OnHealthEmpty -= StopGame;
		
		void StopGame()		=> Time.timeScale = 0;
	}
}