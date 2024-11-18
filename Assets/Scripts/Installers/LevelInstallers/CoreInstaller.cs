namespace Installers
{
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
			BindFromScene<WorldBounds>();
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
			BindFromScene<Snake>();
		}

		ConcreteIdArgConditionCopyNonLazyBinder Bind<T>()
		{
			return Container
				.BindInterfacesTo<T>()
				.AsSingle();
		}

		void BindFromScene<T>()
		{
			Container
				.BindInterfacesTo<T>()
				.FromComponentInHierarchy()
				.AsSingle();
		}
	}
}