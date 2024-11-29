namespace Game.Obj
{
	using System;
	using Modules.Planets;
	using UI;
	using UnityEngine;
	using Zenject;

	public class PlanetPresenter : IInitializable, IDisposable
	{
		[Inject] IPlanet _planet;
		[Inject] IPlanetView _view;
		
		public void Initialize()
		{
			_view.SetState( EPlanetViewState.Locked );
			_view.SetIcon( _planet.GetIcon( false ) );
			_view.SetPrice( _planet.Price.ToString() );

			_planet.OnUnlocked += OnProgress;
			_planet.OnIncomeReady += HandleIncomeReady;
			_planet.OnIncomeTimeChanged += UpdateProgressTick;
		}

		public void Dispose()
		{
			_planet.OnUnlocked -= OnProgress;
			_planet.OnIncomeReady -= HandleIncomeReady;
			_planet.OnIncomeTimeChanged -= UpdateProgressTick;
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
			_view.SetState( EPlanetViewState.Ready );
			_view.SetIcon( _planet.GetIcon( true ) );
		}
		
		string FormatTime(float seconds)
		{
			int minutes = Mathf.FloorToInt(seconds / 60);
			int remainingSeconds = Mathf.FloorToInt(seconds % 60);
        
			return $"{minutes:D1}m:{remainingSeconds:D2}s";
		}
	}
}