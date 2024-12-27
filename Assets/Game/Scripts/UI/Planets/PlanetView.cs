namespace Game.UI
{
	using System;
	using DG.Tweening;
	using Modules.UI;
	using TMPro;
	using UI;
	using UniRx;
	using UnityEngine;
	using UnityEngine.UI;
	using Zenject;

	public enum EPlanetViewState
	{
		None,
		
		Locked,
		InProgress,
		Ready,
	}

	public interface IPlanetView
	{
		IObservable<Unit> OnClick { get; }
		IObservable<Unit> OnHold { get; }

		Vector3 CoinsPos { get; }
		
		void SetState(EPlanetViewState state);
		void ActivateCoin(bool isActive);
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
		
		[SerializeField] SmartButton _smartButton;
		
		[Inject] ParticleAnimator _particleAnimator;
		[Inject (Id = "CoinsTransform")] RectTransform _coinsParent;
		[Inject] ICoinsView _coinsView;
		
		public IObservable<Unit> OnClick => Observable.FromEvent
		(
			x => _smartButton.OnClick += x,
			x => _smartButton.OnClick -= x
		);
		
		public IObservable<Unit> OnHold => Observable.FromEvent
		(
			x => _smartButton.OnHold += x,
			x => _smartButton.OnHold -= x
		);

		public Vector3 CoinsPos => _coinParent.transform.position;

		public void SetState( EPlanetViewState state )
		{
			_lockParent.SetActive( state == EPlanetViewState.Locked );
			_priceParent.SetActive( state == EPlanetViewState.Locked );
			_progressBarParent.SetActive( state == EPlanetViewState.InProgress );
		}
		
		public void ActivateCoin( bool isActive )
		{
			_coinParent.SetActive( isActive );
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