namespace Game.Obj
{
	using System;
	using Modules.Planets;
	using Modules.UI;
	using Services;
	using UI;
	using UnityEngine;
	using Zenject;

	public class PlanetPresenter : IInitializable, IDisposable
	{
		// Obj
		[Inject] IPlanet _planet;
		[Inject] IPlanetView _view;
		
		// Services
		[Inject] ITimeHelper _timeHelper;
		[Inject] IFlyIcons _flyIcons;
		[Inject] IUiNavigator _uiNavigator;
		
		SmartButton _button => _view.SmartButton;
		
		public void Initialize()
		{
			_view.SetState( EPlanetViewState.Locked );
			_view.SetIcon( _planet.GetIcon( false ) );
			_view.SetPrice( _planet.Price.ToString() );
			
			_button.OnClick += TryCollect;
			_button.OnClick += TryUnlock;
			_button.OnHold += OnHold;
			_planet.OnUnlocked += OnProgress;
			_planet.OnIncomeReady += HandleIncomeReady;
			_planet.OnIncomeTimeChanged += UpdateProgressTick;
			_planet.OnGathered += OnGathered;
		}

		public void Dispose()
		{
			_button.OnClick -= TryCollect;
			_button.OnClick -= TryUnlock;
			_button.OnHold -= OnHold;
			_planet.OnUnlocked -= OnProgress;
			_planet.OnIncomeReady -= HandleIncomeReady;
			_planet.OnIncomeTimeChanged -= UpdateProgressTick;
			_planet.OnGathered -= OnGathered;
		}

		void TryCollect()
		{
			if (!_planet.IsIncomeReady)
				return;
			
			_planet.GatherIncome();
		}

		void TryUnlock()
		{
			if (_planet.CanUnlock)
				_planet.Unlock();
		}

		void OnHold()
		{
			_uiNavigator.Show( new PlanetUi(_planet) );
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
		
		void OnGathered(int delta)
		{
			_flyIcons.Fly(_view.Coin, delta );
		}

		void UpdateProgressTick( float progress )
		{
			var time = _timeHelper.SecondsToTxt(progress);
			
			_view.SetProgress( _planet.IncomeProgress, time );
		}

		void OnReady()
		{
			_view.SetState( EPlanetViewState.Ready );
			_view.SetIcon( _planet.GetIcon( true ) );
		}
	}
}