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
			
			_onUpgradeClicked
				.StartWith(Unit.Default)
				.Subscribe( _ =>
				{
					_presenter.TryUpgrade();
					UpdateView();
				})
				.AddTo(_disposables);
			
			_onCloseClicked
				.Subscribe( _ => Hide() )
				.AddTo(_disposables);
		}

		void Hide()
		{
			_disposables.Clear();
			gameObject.SetActive(false);
		}

		void UpdateView()
		{
			 _image.sprite = _presenter.Icon;
			_nameTxt.text = _presenter.Name;
			_populationTxt.text = _presenter.Population;
			_lvlTxt.text = _presenter.Level;
			_incomeTxt.text = _presenter.Income;
			_upgradeButton.interactable = _presenter.CanUpgrade;
			_upgradePriceTxt.text = _presenter.IsMaxLevel ? "MaxLevel" : _presenter.Price;
		}
	}
}