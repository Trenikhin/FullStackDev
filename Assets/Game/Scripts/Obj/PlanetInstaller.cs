namespace Game.Obj
{
	using Modules.Planets;
	using Modules.UI;
	using Obj;
	using UnityEngine;
	using Zenject;

	public class PlanetInstaller : MonoInstaller
	{
		[SerializeField] PlanetConfig _config;

		public override void InstallBindings()
		{
			Container.BindInstance(_config);
			
			Container
				.BindInterfacesTo<Planet>()
				.AsSingle();
			
			Container
				.BindInterfacesTo<PlanetController>()
				.AsSingle();
			
			Container
				.BindInterfacesTo<PlanetPresenter>()
				.AsSingle();
			Container
				.BindInterfacesTo<PlanetView>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Container
				.Bind<SmartButton>()
				.FromComponentInHierarchy()
				.AsSingle();
		}
	}
}