namespace Game.Services
{
	using UnityEngine;

	public interface ITimeHelper
	{
		string SecondsToTxt(float seconds);
	}
	
	public class TimeHelper : ITimeHelper
	{
		public string SecondsToTxt(float seconds)
		{
			int minutes = Mathf.FloorToInt(seconds / 60);
			int remainingSeconds = Mathf.FloorToInt(seconds % 60);
        
			return $"{minutes:D1}m:{remainingSeconds:D2}s";
		}
	}
}