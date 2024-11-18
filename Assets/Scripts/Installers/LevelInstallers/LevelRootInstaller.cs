namespace Installers
{
	using Zenject;

	public class LevelRootInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			CoreInstaller.Install(Container);
			FactoriesInstaller.Install(Container);
		}
	}
	
}