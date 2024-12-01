namespace Game.Obj
{
	using Modules.UI;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public enum EPlanetViewState
	{
		None,
		
		Locked,
		InProgress,
		Ready,
	}

	public interface IPlanetView
	{
		Vector3 Coin { get; }
		SmartButton SmartButton { get; }
		
		void SetState(EPlanetViewState state);
		void SetProgress(float progress, string label);
		void SetIcon(Sprite icon);
		void SetPrice(string price);
	}
	
	public class PlanetView : MonoBehaviour, IPlanetView
	{
		[Header("View States")]
		[SerializeField] GameObject _lockParent;
		[SerializeField] GameObject _coinParent;
		[SerializeField] GameObject _priceParent;
		[SerializeField] GameObject _progressBarParent;
		[Header("Planet Setup")]
		[SerializeField] Image _planetIcon;
		[SerializeField] TextMeshProUGUI _unlockPriceText;
		[SerializeField] Image _progressBar;
		[SerializeField] TextMeshProUGUI _progressText;

		[field: SerializeField] public SmartButton SmartButton { get; private set; }
		
		public Vector3 Coin => _coinParent.transform.position;

		public void SetState( EPlanetViewState state )
		{
			_lockParent.SetActive( state == EPlanetViewState.Locked );
			_priceParent.SetActive( state == EPlanetViewState.Locked );
			_progressBarParent.SetActive( state == EPlanetViewState.InProgress );
			_coinParent.SetActive( state == EPlanetViewState.Ready );
		}
		
		public void SetProgress(float progress, string label)
		{
			_progressBar.fillAmount = progress;
			_progressText.text = label;
		}
		
		public void SetIcon( Sprite icon ) => _planetIcon.sprite = icon;
		public void SetPrice(string price) => _unlockPriceText.text = price;
	}
}