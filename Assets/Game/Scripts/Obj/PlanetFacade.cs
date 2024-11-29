namespace Game.Obj
{
	using Modules.Planets;using UnityEngine;
	using Zenject;
	
	public interface IPlanetFacade
	{
		IPlanetView View { get; }
		IPlanet Planet { get; }
	}
	
	public class PlanetFacade: MonoBehaviour, IPlanetFacade
	{
		[Inject] IPlanetView	_view;
		[Inject] IPlanet		_logic;

		public IPlanetView	View		=>	_view;
		public IPlanet		Planet		=> _logic;
	}
}