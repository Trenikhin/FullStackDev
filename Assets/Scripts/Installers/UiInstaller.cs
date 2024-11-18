namespace Installers
{
	using Ui;
	using SnakeGame;
	using Zenject;

	public class UiInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<GameUI>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Container
				.BindInterfacesAndSelfTo<GameUiPresenter>()
				.AsSingle();
		}
	}
}