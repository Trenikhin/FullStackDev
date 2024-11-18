namespace Installers
{
	using Coins;
	using Core;
	using Input;
	using Modules;
	using SnakeGame;
	using Zenject;

	public class CoreInstaller : Installer<CoreInstaller>
	{
		public override void InstallBindings()
		{
			InstallCore();
			InstallObjs();
			InstallGameFlow();
		}

		void InstallCore()
		{
			Container
				.BindInterfacesTo<WorldBounds>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Bind<Difficulty>().WithArguments( 9 );
			Bind<Score>();
		}

		void InstallGameFlow()
		{
			Bind<DifficultyChanger>();
			Bind<GameOverHandler>();
		}

		void InstallObjs()
		{
			Bind<SnakeController>();
			Bind<InputHandler>();
			
			Container
				.BindInterfacesTo<Snake>()
				.FromComponentInHierarchy()
				.AsSingle();
		}

		ConcreteIdArgConditionCopyNonLazyBinder Bind<T>()
		{
			return Container
				.BindInterfacesTo<T>()
				.AsSingle();
		}
	}
}