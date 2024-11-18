namespace Installers
{
	using Zenject;

	public class LevelRootInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			ModuleInstaller.Install(Container);
			CoreInstaller.Install(Container);
			FactoriesInstaller.Install(Container);
		}
	}
	
}