namespace Game.SaveSystem
{
	using UnityEngine;
	using Zenject;

	public class SaveSystemInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<Saver>().AsSingle();
			Container.BindInterfacesTo<GameRepository>().AsSingle();
			
			InstallSerializers();
		}
		
		void InstallSerializers()
		{
			BindSerializer<CountdownComponentSerializer>();
			BindSerializer<DestinationComponentSerializer>();
			BindSerializer<BagComponentSerializer>();
			BindSerializer<TransformComponentSerializer>();
		}
		
		void BindSerializer<TSerializer>()
		where TSerializer : ISerializer
		=>
			Container.BindInterfacesTo<TSerializer>().AsCached();
		
	}
}