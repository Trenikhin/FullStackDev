namespace Game.Configs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "ConvertConfig", menuName = "Configs/ConvertConfig", order = 0)]
	public class ConvertConfig : ScriptableObject
	{
		public int RawMaterialsCapacity = 12;
		public int ConvertedMaterialsCapacity = 12;
		public int InputAmount = 3;
		public int OutputAmount = 1;
		public float ConvertTime = 1;
	}
}