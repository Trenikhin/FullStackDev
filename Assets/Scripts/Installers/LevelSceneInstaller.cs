namespace DefaultNamespace
{
	using CoinManager;
	using Core;
	using Input;
	using Modules;
	using SnakeGame;
	using Zenject;

	public class LevelSceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			// Install Modules
			Bind<Score>();
			BindFromScene<Snake>();
			BindFromScene<WorldBounds>();
			Bind<Difficulty>().WithArguments( 9 );

			// Core
			Bind<SnakeController>();
			Bind<InputHandler>();
			Bind<Coins>();
			Bind<DifficultyChanger>();
			Bind<GameOverHandler>();
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