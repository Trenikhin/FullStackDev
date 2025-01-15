namespace Game.Entities
{
	using Components;
	using UnityEngine;
	using Zenject;
	using Zenject.SpaceFighter;

	public class PlayerController : IInitializable
	{
		PushComponent _pushForwardComponent;
		PushComponent _pushUpperComponent;
		JumpComponent _jumpComponent;
		GroundComponent _groundComponent;
		
		public PlayerController
		(
			PushComponent[] pushComponents,
			JumpComponent jumpComponent,
			GroundComponent groundComponent
		)
		{
			_pushForwardComponent = pushComponents[0];
			_pushUpperComponent = pushComponents[1];
			_jumpComponent = jumpComponent;
			_groundComponent = groundComponent;
		}
		
		public void Initialize()
		{
			_pushForwardComponent.AddPushCondition( () => Input.GetKeyDown(KeyCode.Mouse0) );
			_pushUpperComponent.AddPushCondition( () => Input.GetKeyDown(KeyCode.Mouse1) );
			_jumpComponent.AddJumpCondition( () => _groundComponent.IsGrounded() );
		}
	}
}