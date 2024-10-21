namespace ShootEmUp
{
	using UnityEngine;
	
	public class GameFlow : MonoBehaviour
	{
		public void StopGame() => Time.timeScale = 0;
	}
}