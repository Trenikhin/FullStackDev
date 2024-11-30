namespace Game.Obj
{
	using System;
	using Modules.Planets;
	using Modules.UI;
	using UI;
	using UniRx;
	using Zenject;

	public class PlanetController : IInitializable, IDisposable
	{
		[Inject] SmartButton _button;
		[Inject] IUiNavigator _uiNavigator;
		[Inject] IPlanet _planet;
		
		CompositeDisposable _disposables = new ();
		
		public void Initialize()
		{
			_button.OnClick += TryCollect;
			_button.OnClick += TryUnlock;
			_button.OnHold += OnHold;
		}

		public void Dispose()
		{
			_button.OnClick -= TryCollect;
			_button.OnClick -= TryUnlock;
			_button.OnHold -= OnHold;
			
			_disposables?.Dispose();
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
	}
}