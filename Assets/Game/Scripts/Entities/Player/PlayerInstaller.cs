namespace Game.Entities
{
	using Components;
	using UnityEngine;
	using UnityEngine.Serialization;

	public class PlayerInstaller : EntityInstaller
	{
		[Header("Common")]
		[SerializeField] Rigidbody2D _rigidbody;
		[FormerlySerializedAs("_pushTrigger")] [SerializeField] PhysicsProvider2D pushPhysics;
		[SerializeField] Transform _face;
		
		[Header("Movement")]
		[SerializeField] float _movementSpeed = 15;
		[SerializeField] Transform _parentTransform;
		
		[Header("Jumping")]
		[SerializeField] float _jumpForce = 6;
		[SerializeField] Transform _groundChecker;
		[SerializeField] LayerMask _groundLayer;
		
		[Header("Health")]
		[SerializeField] float _health = 100;
		[SerializeField] float _maxHealth;
		
		public override void InstallBindings()
		{
			base.InstallBindings();

			Container
				.BindInterfacesTo<PlayerController>()
				.AsSingle();
			
			Container
				.BindInstance(pushPhysics)
				.AsCached()
				.WhenInjectedInto<PushComponent>();
			
			Container
				.BindInstance(_rigidbody)
				.AsSingle();
			
			BindComponents();
		}

		void BindComponents()
		{
			Container
				.Bind<HealthComponent>()
				.AsSingle()
				.WithArguments( _health, _maxHealth );
			
			Container
				.BindInterfacesAndSelfTo<MoveComponent>()
				.AsSingle()
				.WithArguments( _face, _movementSpeed );
			
			Container
				.BindInterfacesAndSelfTo<RotatingPlayerComponent>()
				.AsSingle()
				.WithArguments( _parentTransform );
			
			Container
				.BindInterfacesAndSelfTo<JumpComponent>()
				.AsSingle()
				.WithArguments( _jumpForce  );
			
			Container
				.BindInterfacesAndSelfTo<PushComponent>()
				.AsCached()
				.WithArguments( Vector3.up, 100f );
			
			Container
				.BindInterfacesAndSelfTo<PushComponent>()
				.AsCached()
				.WithArguments( Vector3.right, 100f );
			
			Container
				.BindInterfacesAndSelfTo<GroundComponent>()
				.AsCached()
				.WithArguments( transform, _groundChecker, _groundLayer );
		}
	}
}