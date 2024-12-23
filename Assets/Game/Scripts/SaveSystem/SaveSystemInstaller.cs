namespace Game.SaveSystem
{
	using Zenject;

	public class SaveSystemInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<Saver>().AsSingle();
			Container.BindInterfacesTo<GameRepository>().AsSingle();
			Container.BindInterfacesTo<SerializeHelper>().AsSingle();
			
			InstallSerializers();
		}
		
		void InstallSerializers()
		{
			// (!) Have to be First
			BindSerializer<WorldSerializer>();
			
			// Component Serializers
			BindSerializer<ResourceBagSerializer>();
			BindSerializer<CountdownSerializer>();
			BindSerializer<DestinationPointSerializer>();
			BindSerializer<HealthSerializer>();
			BindSerializer<ProductionOrderSerializer>();
			BindSerializer<TargetObjectSerializer>();
			BindSerializer<TeamSerializer>();
			BindSerializer<TransformSerializer>();
		}
		
		void BindSerializer<TSerializer>()
		where TSerializer : ISerializer
		=>
			Container.BindInterfacesTo<TSerializer>().AsCached();
	}
}