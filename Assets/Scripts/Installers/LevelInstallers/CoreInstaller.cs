namespace Installers
{
	using Coins;
	using Core;
	using Input;
	using Zenject;

	public class CoreInstaller : Installer<CoreInstaller>
	{
		public override void InstallBindings()
		{
			Bind<SnakeController>();
			Bind<InputHandler>();
			Bind<Coins>();
			Bind<DifficultyChanger>();
			Bind<GameOverHandler>();
		}
		
		void Bind<T>()
		{
			Container
				.BindInterfacesTo<T>()
				.AsSingle();
		}
	}
}