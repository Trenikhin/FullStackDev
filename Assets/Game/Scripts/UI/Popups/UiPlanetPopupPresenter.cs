namespace Game.UI
{
	using System;
	using Modules.Planets;
	using UniRx;
	using Zenject;
	
	public interface IPlanetPopupHandler : IPopupHandler
	{
		void OnShow(IPlanet planet);
	}

	public class UiPlanetPopupPresenter : IPlanetPopupHandler
	{
		[Inject] IUiPlanetPopupView _view;
		[Inject] IUiNavigator _navigator;
 		
		IPlanet _planet;
		
		CompositeDisposable _showDisposables = new ();
		
		public void OnShow( IPlanet planet )
		{
			_planet = planet;
			_showDisposables = new CompositeDisposable();
			
			_view.ShowHide( true );
			UpdateView(_planet);
			
			// OnPlanet Upgraded
			_planet.OnUpgraded += OnUpgraded;
			
			// On Close Click
			_view.OnCloseClicked
				.Subscribe(_ => _navigator.Hide() )
				.AddTo(_showDisposables);
			
			// On Upgrade Click
			_view.OnUpgradeClicked
				.Where( _ => _planet.CanUpgrade )
				.Subscribe( _ =>  _planet.Upgrade() )
				.AddTo( _showDisposables );
		}

		public void OnHide()
		{
			_showDisposables.Dispose();
			_planet.OnUpgraded -= OnUpgraded;
			_planet = null;
			_view.ShowHide( false );
		}
		
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
	}
}