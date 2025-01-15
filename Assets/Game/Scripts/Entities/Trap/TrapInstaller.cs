namespace Game.Entities
{
	using UnityEngine;
	using UnityEngine.Serialization;

	public class TrapInstaller : EntityInstaller
	{
		[SerializeField] PhysicsProvider2D physics;
		[SerializeField] float _damage;
		
		public override void InstallBindings()
		{
			base.InstallBindings();
			
			Container
				.BindInterfacesTo<TrapComponent>()
				.AsSingle()
				.WithArguments(_damage);
			
			Container
				.BindInstance(physics)
				.AsSingle();
		}
	}
}