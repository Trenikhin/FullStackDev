namespace Game.UI
{
	using System;
	using TMPro;
	using UniRx;
	using UnityEngine;
	using UnityEngine.UI;
	using Zenject;
	
	public class PlanetPopupView : MonoBehaviour
	{
		[Header("Planet Icon")]
		[SerializeField] Image _image;
		[Header("Texts")]
		[SerializeField] TextMeshProUGUI _nameTxt;
		[SerializeField] TextMeshProUGUI _populationTxt;
		[SerializeField] TextMeshProUGUI _lvlTxt;
		[SerializeField] TextMeshProUGUI _incomeTxt;
		[SerializeField] TextMeshProUGUI _upgradePriceTxt;
		[Header("Buttons")]
		[SerializeField] Button _closeButton;
		[SerializeField] Button _upgradeButton;
		[SerializeField] Button _backgroundButton;

		[Inject] IPlanetPopupPresenter _presenter;
		[Inject] IPlanetHider _shower;
		
		CompositeDisposable _disposables = new ();
		
		IObservable<Unit> _onUpgradeClicked => _upgradeButton.OnClickAsObservable();
		IObservable<Unit> _onCloseClicked => Observable.Merge
		(
			_closeButton.OnClickAsObservable(), 
			_backgroundButton.OnClickAsObservable()
		);
		
		void OnDestroy() => _disposables.Dispose();
		
		public void Show()
		{
			gameObject.SetActive(true);
			
			// Input
			_onUpgradeClicked
				.Subscribe( _ => _presenter.TryUpgrade())
				.AddTo(_disposables);
			
			_onCloseClicked
				.Subscribe( _ => _shower.Hide() )
				.AddTo(_disposables);
			
			// Update View
			_presenter.Icon
				.Subscribe( s => _image.sprite = s )
				.AddTo(_disposables);
			
			_presenter.Name
				.Subscribe( s => _nameTxt.text = s )
				.AddTo(_disposables);
			
			_presenter.Population
				.Subscribe( s => _populationTxt.text = s )
				.AddTo(_disposables);
			
			_presenter.Level
				.Subscribe( s => _lvlTxt.text = s )
				.AddTo(_disposables);
			
			_presenter.Income
				.Subscribe( s => _incomeTxt.text = s )
				.AddTo(_disposables);
			
			_presenter.Price
				.Subscribe( s => _upgradePriceTxt.text = s )
				.AddTo(_disposables);
			
			_presenter.CanUpgrade
				.Subscribe( s => _upgradeButton.interactable = s )
				.AddTo(_disposables);
		}

		public void Hide()
		{
			_disposables.Clear();
			gameObject.SetActive(false);
		}
	}
}