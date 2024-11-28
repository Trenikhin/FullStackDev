namespace Game.UI
{
	using System;
	using TMPro;
	using UniRx;
	using UnityEngine;
	using UnityEngine.UI;

	public interface IUiPlanetPopupView
	{
		IObservable<Unit> OnUpgradeClicked { get; }
		IObservable<Unit> OnCloseClicked { get; }
		
		void ShowHide(bool show);

		void SetIcon(Sprite icon);
		void SetName(string name);
		void SetPopulation(string population);
		void SetLevel(string lvl);
		void SetIncome(string income);
		void SetUpgradePrice(string upgradePrice);
		void SetInteractable(bool isInteractable);
	}
	
	public class UiPlanetPopupView : MonoBehaviour, IUiPlanetPopupView
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

		public IObservable<Unit> OnUpgradeClicked => _upgradeButton.OnClickAsObservable();
		public IObservable<Unit> OnCloseClicked => Observable.Merge
		(
			_closeButton.OnClickAsObservable(), 
			_backgroundButton.OnClickAsObservable()
		);

		
		public void ShowHide( bool show ) => gameObject.SetActive(show);
		
		public void SetIcon(Sprite icon ) => _image.sprite = icon;
		public void SetName( string planetName ) => _nameTxt.text = planetName;
		public void SetPopulation( string population ) => _populationTxt.text = population;
		public void SetLevel( string lvl ) => _lvlTxt.text = lvl;
		public void SetIncome( string income ) => _incomeTxt.text = income;
		public void SetUpgradePrice(string upgradePrice) => _upgradePriceTxt.text = upgradePrice;
		public void SetInteractable(bool isInteractable) => _upgradeButton.interactable = isInteractable;
	}
}