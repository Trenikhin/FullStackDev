namespace Data
{
	using Modules;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Prefabs", menuName = "Configs/Prefabs", order = 0)]
	public class PrefabsConfig : ScriptableObject
	{
		public Coin CoinTemplate;
	}
}