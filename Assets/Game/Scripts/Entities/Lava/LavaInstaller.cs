namespace Game.Entities
{
	using Components;
	using UnityEngine;
	using UnityEngine.Serialization;

	public class LavaInstaller : EntityInstaller
	{
		[FormerlySerializedAs("_trigger")] [SerializeField] PhysicsProvider2D physics;
		
		public override void InstallBindings()
		{
			base.InstallBindings();
			
			Container
				.BindInterfacesTo<LavaComponent>()
				.AsSingle();
			
			Container
				.BindInstance(physics)
				.AsSingle();
		}
	}
}