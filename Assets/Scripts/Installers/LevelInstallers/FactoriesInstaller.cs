namespace Installers
{
	using Data;
	using Modules;
	using Zenject;
	using Coins;

	public class FactoriesInstaller : Installer<FactoriesInstaller>
	{
		[Inject] PrefabsConfig _prefabsConfig;
		
		public override void InstallBindings()
		{
			// Coins Pool
			Container
				.BindMemoryPool<Coin, Coins.CoinPool>()
				.WithInitialSize(5) 
				.FromComponentInNewPrefab(_prefabsConfig.CoinTemplate)
				.UnderTransformGroup("Coins")
				.AsSingle(); 
		}
	}
}