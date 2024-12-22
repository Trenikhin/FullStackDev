namespace Game.SaveSystem
{
	using UnityEngine;
	using Zenject;

	public class SaveSystemInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<Saver>().AsSingle();
			Container.BindInterfacesTo<SerializeHelper>().AsSingle();
			Container.BindInterfacesTo<GameRepository>().AsSingle();
			
			InstallSerializers();
		}
		
		void InstallSerializers()
		{
			BindSerializer<CountdownSerializer>();
			BindSerializer<DestinationSerializer>();
			BindSerializer<BagSerializer>();
			BindSerializer<TransformSerializer>();
		}
		
		void BindSerializer<TSerializer>()
		where TSerializer : ISerializer
		=>
			Container.BindInterfacesTo<TSerializer>().AsCached();
		
	}
}