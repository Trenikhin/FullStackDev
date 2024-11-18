namespace Data
{
	using System.Collections.Generic;
	using Modules;
	using UnityEngine;

	[System.Serializable]
	public class KeyBinding
	{
		public SnakeDirection Direction;
		public List<KeyCode> Keys;
	} 
	
	[CreateAssetMenu(fileName = "InputMapConfig", menuName = "Configs/InputMapConfig", order = 0)]
	public class InputMapConfig : ScriptableObject
	{
		public List<KeyBinding> KeyBindings;
	}
}