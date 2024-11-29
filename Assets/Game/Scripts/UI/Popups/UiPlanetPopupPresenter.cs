namespace Game.UI
{
	using System;
	using Modules.Planets;
	using UniRx;
	using Zenject;
	
	[Serializable]
	public class PlanetUi : IUi
	{
		public IPlanet Planet { get; }
		
		public PlanetUi(IPlanet planet)
		{
			Planet = planet;
		}
	}

	public class UiPlanetPopupPresenter : IInitializable, IDisposable
	{
		[Inject] IUiPlanetPopupView _view;
		[Inject] IUiNavigator _navigator;
		
		IPlanet _planet;
		
		CompositeDisposable _lifetimeDisposables = new ();
		CompositeDisposable _showDisposables = new ();
		
		public void Initialize()
		{
			// Open
			_navigator
				.OnOpen<PlanetUi>()
				.Subscribe(m => OnShow( m.Planet ))
				.AddTo(_lifetimeDisposables);
			
			// Close
			_navigator
				.OnClose<PlanetUi>()
				.Subscribe(m => OnHide())
				.AddTo(_lifetimeDisposables);
		}
		
		public void Dispose() => _lifetimeDisposables?.Dispose();
		
		void OnShow( IPlanet planet )
		{
			_planet = planet;
			_showDisposables = new CompositeDisposable();
			
			_view.ShowHide( true );
			UpdateView(_planet);
			
			// OnPlanet Upgraded
			_planet.OnUpgraded += OnUpgraded;
			
			// On Close Click
			_view.OnCloseClicked
				.Subscribe(_ => _navigator.Hide<PlanetUi>() )
				.AddTo(_showDisposables);
			
			// On Upgrade Click
			_view.OnUpgradeClicked
				.Where( _ => _planet.CanUpgrade )
				.Subscribe( _ =>  _planet.Upgrade() )
				.AddTo( _showDisposables );
		}
		
		void OnHide()
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