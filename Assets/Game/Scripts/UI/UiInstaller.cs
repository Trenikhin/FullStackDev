﻿namespace Game.UI
{
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
				.BindInterfacesTo<FlyIcons>()
				.FromComponentInHierarchy()
				.AsSingle();
		}
	}
}