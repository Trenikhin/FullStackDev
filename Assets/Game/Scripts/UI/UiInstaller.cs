namespace Game.UI
{
	using Services;
	using Zenject;

	public class UiInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesTo<UiNavigator>()
				.AsSingle();
			
			Container
				.BindInterfacesTo<UiPlanetPopupPresenter>()
				.AsSingle();
			
			Container
				.BindInterfacesTo<UiPlanetPopupView>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Container
				.BindInterfacesTo<FlyCoins>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Container
				.BindInterfacesTo<TimeHelper>()
				.AsSingle();
		}
	}
}