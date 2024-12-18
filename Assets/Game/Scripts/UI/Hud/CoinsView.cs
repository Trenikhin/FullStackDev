namespace Game.UI
{
	using TMPro;
	using UnityEngine;

	public interface ICoinsView
	{
		string Text { set; }
	}
	
	public class CoinsView : MonoBehaviour, ICoinsView
	{
		[SerializeField] TextMeshProUGUI _coinsText;
		
		public string Text { set => _coinsText.text = value; }
	}
}