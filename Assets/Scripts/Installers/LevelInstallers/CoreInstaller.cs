namespace Installers
{
	using Core;
	using Snake;
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
			Bind<ScoreCounter>();
		}

		void InstallObjs()
		{
			Bind<SnakeController>();
			Bind<InputHandler>();
			BindFromScene<Snake>();
			Bind<CoinCollector>();
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