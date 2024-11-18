namespace Installers
{
	using Modules;
	using SnakeGame;
	using UnityEngine;
	using Zenject;

	public class ModuleInstaller : Installer<ModuleInstaller>
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesTo<Score>()
				.AsSingle();
			
			Container
				.BindInterfacesTo<Snake>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Container
				.BindInterfacesTo<WorldBounds>()
				.FromComponentInHierarchy()
				.AsSingle();
			
			Container
				.BindInterfacesTo<Difficulty>()
				.AsSingle()
				.WithArguments( 9 );
		}
	}
}