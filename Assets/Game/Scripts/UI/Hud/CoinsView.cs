namespace Game.UI
{
	using TMPro;
	using UnityEngine;

	public interface ICoinsView
	{
		string Text { set; }
		
		void ShowHide(bool show);
	}
	
	public class CoinsView : MonoBehaviour, ICoinsView
	{
		[SerializeField] TextMeshProUGUI _coinsText;
		
		public string Text { set => _coinsText.text = value; }
		
		public void ShowHide(bool show) => gameObject.SetActive(show);
	}
}