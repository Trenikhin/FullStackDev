namespace Game.SaveSystem
{
	using UnityEditor;
	using UnityEngine;

	public class SaveMenu
	{
		[MenuItem("Game/Clear PlayerPrefs")]
		public static void ClearPlayerPrefs()
		{
			if (EditorUtility.DisplayDialog("Clear PlayerPrefs", "Are you sure you want to clear all PlayerPrefs?", "Yes", "No"))
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();
				Debug.Log("PlayerPrefs cleared!");
			}
		}
	}
}