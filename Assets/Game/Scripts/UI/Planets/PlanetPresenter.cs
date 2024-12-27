namespace Game.UI
{
	using System;
	using Modules.Money;
	using Modules.Planets;
	using UniRx;
	using UnityEngine;
	using Zenject;

	public class PlanetPresenter : IInitializable, IDisposable
	{
		[Inject] IPlanet _planet;
		[Inject] IPlanetView _view;
		
		[Inject] IMoneyStorage _storage;
		[Inject] IPlanetShower _popShower;
		[Inject] IMessagePublisher _publisher;
		
		CompositeDisposable _disposables = new ();
		
		public void Initialize()
		{
			_view.ActivateCoin( false );
			_view.SetState( EPlanetViewState.Locked );
			_view.SetIcon( _planet.GetIcon( false ) );
			_view.SetPrice( _planet.Price.ToString() );

			_view.OnClick
				.Subscribe(_ =>
				{
					TryCollect();
					TryUnlock();
				} )
				.AddTo( _disposables );
		
			_view.OnHold
				.Subscribe(_ => OpenPopup())
				.AddTo( _disposables );
			
			_planet.OnUnlocked += OnProgress;
			_planet.OnIncomeReady += HandleIncomeReady;
			_planet.OnIncomeTimeChanged += UpdateProgressTick;
			_planet.OnGathered += OnCoinCollected;
		}

		public void Dispose()
		{
			_disposables?.Dispose();
			
			_planet.OnUnlocked -= OnProgress;
			_planet.OnIncomeReady -= HandleIncomeReady;
			_planet.OnIncomeTimeChanged -= UpdateProgressTick;
			_planet.OnGathered -= OnCoinCollected;
		}

		void TryCollect()
		{
			if (!_planet.IsIncomeReady)
				return;
			
			_planet.GatherIncome();
		}

		void TryUnlock()
		{
			if (_planet.MaxLevel > _planet.Level && _planet.CanUnlock)
				_planet.Unlock();
		}

		void OpenPopup()
		{
			_popShower.Show( _planet );
		}
		
		void HandleIncomeReady(bool isReady)
		{
			if (isReady)
				OnReady();
			else
				OnProgress();
		}
		
		void OnProgress()
		{
			_view.SetState( EPlanetViewState.InProgress );
			_view.SetIcon( _planet.GetIcon( true ) );
		}

		void UpdateProgressTick( float progress )
		{
			_view.SetProgress( _planet.IncomeProgress, FormatTime(progress) );
		}

		void OnReady()
		{
			_view.ActivateCoin( true );
			_view.SetState( EPlanetViewState.Ready );
			_view.SetIcon( _planet.GetIcon( true ) );
		}
		
		void OnCoinCollected(int delta)
		{
			_view.ActivateCoin( false );
			
			_publisher.Publish( new EarnData()
			{
				From = _view.CoinsPos,
				Delta = delta
			} );
		}
		
		string FormatTime(float seconds)
		{
			int minutes = Mathf.FloorToInt(seconds / 60);
			int remainingSeconds = Mathf.FloorToInt(seconds % 60);
        
			return $"{minutes:D1}m:{remainingSeconds:D2}s";
		}
	}
}