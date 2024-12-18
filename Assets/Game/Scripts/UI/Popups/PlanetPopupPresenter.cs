namespace Game.UI
{
	using Modules.Planets;
	using UnityEngine;

	public interface IPlanetPopupPresenter
	{
		Sprite Icon { get; }
		string Name { get; }
		string Population { get; }
		string Level { get; }
		string Income { get; }
		string Price { get; }
		bool CanUpgrade { get; }
		bool IsMaxLevel { get; }

		bool TryUpgrade();
	}
	
	public class PlanetPopupPresenter : IPlanetPopupPresenter
	{
		IPlanet _planet;

		public void Setup(IPlanet planet) => _planet = planet;
		
		public Sprite Icon => _planet.GetIcon(true);
		public string Name => _planet.Name;
		public string Population => $"Population: {_planet.Population}";
		public string Level => $"Level: {_planet.Level}/ {_planet.MaxLevel}";
		public string Income => $"Income: {_planet.MinuteIncome}";
		public string Price => $"{_planet.Price}";
		public bool CanUpgrade => _planet.CanUpgrade;
		public bool IsMaxLevel => _planet.IsMaxLevel;
		
		public bool TryUpgrade()
		{
			if (!_planet.CanUpgrade)
				return false;

			_planet.Upgrade();
			return true;
		}
	}
}