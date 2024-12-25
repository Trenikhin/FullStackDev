namespace Game.UI
{
	using Modules.Money;
	using Modules.Planets;
	using UniRx;
	using UnityEngine;
	using Zenject;

	public interface IPlanetPopupPresenter
	{
		ReactiveProperty<Sprite> Icon { get; }
		ReactiveProperty<string> Name { get; }
		ReactiveProperty<string> Population { get; }
		ReactiveProperty<string> Level { get; }
		ReactiveProperty<string> Income { get; }
		ReactiveProperty<string> Price { get; }
		ReactiveProperty<bool> CanUpgrade { get; }
		
		bool TryUpgrade();
	}
	
	public class PlanetPopupPresenter : IPlanetPopupPresenter
	{
		[Inject] IMoneyStorage _storage;
		
		IPlanet _planet;
		
		public void Setup(IPlanet planet)
		{
			_planet = planet;
			
			Icon.Value = _planet.GetIcon(true);
			Name.Value = $"{_planet.Name}";
			OnMoneyChanged(default,default);
			OnStateChanged(default);
			OnIncomeChanged(default);
			OnPopulationChanged(default);
		}

		public void OnShow()
		{
			_storage.OnMoneyChanged += OnMoneyChanged;
			_planet.OnUpgraded += OnStateChanged;
			_planet.OnIncomeChanged += OnIncomeChanged;
			_planet.OnPopulationChanged += OnPopulationChanged;
		}

		public void OnHide()
		{
			_storage.OnMoneyChanged -= OnMoneyChanged;
			_planet.OnUpgraded -= OnStateChanged;
			_planet.OnIncomeChanged -= OnIncomeChanged;
			_planet.OnPopulationChanged -= OnPopulationChanged;
		}
		
		public ReactiveProperty<Sprite> Icon { get; } = new ();
		public ReactiveProperty<string> Name { get; } = new ();
		public ReactiveProperty<string> Population { get; } = new ();
		public ReactiveProperty<string> Level { get; } = new ();
		public ReactiveProperty<string> Income { get; } = new ();
		public ReactiveProperty<string> Price { get; } = new ();
		public ReactiveProperty<bool> CanUpgrade { get; } = new ();

		public bool TryUpgrade()
		{
			if (!_planet.CanUpgrade)
				return false;

			_planet.Upgrade();
			return true;
		}

		void OnPopulationChanged(int value)
		{
			Population.Value = $"Population: {value}";
		}

		void OnIncomeChanged(int value)
		{
			Income.Value = $"Income: {_planet.MinuteIncome}";
		}

		void OnStateChanged(int value)
		{
			Level.Value = $"Level: {_planet.Level}";
			Price.Value = !_planet.IsMaxLevel ? $"Price: {_planet.Price}" : "MaxLevel";
			OnMoneyChanged(default, default);
		}
		
		void OnMoneyChanged(int v1, int v2)
		{
			CanUpgrade.Value = _planet.CanUpgrade && !_planet.IsMaxLevel;
		}
	}
}