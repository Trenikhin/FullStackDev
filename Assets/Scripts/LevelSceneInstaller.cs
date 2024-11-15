namespace DefaultNamespace
{
	using Input;
	using Modules;
	using Zenject;

	public class LevelSceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindFromScene<Snake>();
			Bind<SnakeController>();
			Bind<OldInputHandler>();
		}

		void Bind<T>()
		{
			Container
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