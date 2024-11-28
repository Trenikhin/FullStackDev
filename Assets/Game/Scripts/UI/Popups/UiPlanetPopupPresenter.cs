namespace Game.UI
{
	using System;
	using Modules.Planets;
	using Obj;
	using UniRx;
	using UnityEngine;
	using Zenject;
	
	public class UiPlanetPopupPresenter : IInitializable, IDisposable
	{
		[Inject] IUiPlanetPopupView _view;
		[Inject] PlanetPopupModel _model;
		[Inject] IPlanetPopup _planetPopup; 
		
		IPlanet _planet;
		
		CompositeDisposable _disposables = new ();
		
		public void Initialize()
		{
			// Show / Hide
			_model.IsShowing
				.Skip(1)
				.Subscribe(s =>
				{
					if (s)
						OnShow(_model.Planet);
					else
						OnHide();
				} )
				.AddTo(_disposables);
			
			// On Close Click
			_view.OnCloseClicked
				.Subscribe(_ => _planetPopup.Hide() )
				.AddTo(_disposables);
			
			// On Upgrade Click
			_view.OnUpgradeClicked
				.Where( _ => _planet.CanUpgrade )
				.Subscribe( _ =>  _planet.Upgrade() )
				.AddTo( _disposables );
			
			// Update view
			_model.IsShowing
				.Where( s => s )
				.Subscribe( _ => UpdateView( _planet ) )
				.AddTo(_disposables);
		}

		public void Dispose() => _disposables?.Dispose();

		void UpdateView(IPlanet planet)
		{
			_view.SetIcon( planet.GetIcon( true) );
			_view.SetName( $"{planet.Name}");
			_view.SetPopulation( $"Population: {planet.Population}" );
			_view.SetLevel( $"Level: {planet.Level}/{planet.MaxLevel}" );
			_view.SetIncome( $"Income: {planet.MinuteIncome}" );
			_view.SetUpgradePrice( $"{planet.Price}" );
			_view.SetInteractable( _planet.CanUpgrade );
		}

		void OnUpgraded( int _ ) => UpdateView(_planet);
		
		void OnShow( IPlanet planet )
		{
			_planet = planet;
			_planet.OnUpgraded += OnUpgraded;
			_view.ShowHide( true );
		}

		void OnHide()
		{
			_planet.OnUpgraded -= OnUpgraded;
			_planet = null;
			_view.ShowHide( false );
		}
	}
}