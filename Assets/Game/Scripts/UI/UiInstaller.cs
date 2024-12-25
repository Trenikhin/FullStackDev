namespace Game.UI
{
	using Modules.Planets;
	using Modules.UI;
	using UniRx;
	using UnityEngine;
	using Zenject;

	public class UiInstaller : MonoInstaller
	{
		[SerializeField] Transform _planetsContainer;
		[SerializeField] PlanetView _planetTemplate;
		
		[Inject] IPlanet[] _planets;
		
		public override void InstallBindings()
		{
			Container
				.BindInterfacesTo<MessageBroker>()
				.AsSingle();
			
			InstallPlanetUi();
			
			Container
				.Bind<ParticleAnimator>()
				.FromComponentInHierarchy()
				.AsSingle();
		}

		void InstallPlanetUi()
		{
			Container
				.BindInterfacesTo<PlanetShower>()
				.AsSingle();
			
			Container
				.BindInterfacesAndSelfTo<PlanetPopupPresenter>()
				.AsSingle();
			
			Container
				.BindInterfacesAndSelfTo<PlanetPopupView>()
				.FromComponentInHierarchy()
				.AsSingle();
		}
	}
}