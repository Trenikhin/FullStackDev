namespace Game.UI
{
	using Modules.Planets;
	using UnityEngine;
	using Zenject;

	public class PlanetInstaller : MonoInstaller
	{
		[SerializeField] int _planetIndex;
		
		[Inject] IPlanet[] _planets;

#if UNITY_EDITOR
		void OnValidate()
		{
			_planetIndex = transform.GetSiblingIndex();
		}
#endif
		public override void InstallBindings()
		{
			// Presenter
			Container
				.BindInterfacesAndSelfTo<PlanetPresenter>()
				.AsSingle();
			
			// View
			Container
				.BindInterfacesTo<PlanetView>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			// Model
			Container.BindInstance(_planets[_planetIndex]);
		}
	}
}