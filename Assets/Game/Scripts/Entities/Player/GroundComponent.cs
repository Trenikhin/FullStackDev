namespace Game.Components
{
	using Entities;
	using UnityEngine;
	using Zenject;

	public class GroundComponent : ITickable
	{
		Transform _root;
		Transform _groundChecker;
		LayerMask _groundLayer;
		Rigidbody2D _rigidbody;
		
		Vector3 _localScale;
		
		public GroundComponent( Rigidbody2D rigidbody2D, Transform root, Transform groundChecker, LayerMask groundLayer )
		{
			_rigidbody = rigidbody2D;
			_root = root;
			_groundChecker = groundChecker;
			_groundLayer = groundLayer;
		}
		
		public bool IsGrounded()
		{
			bool isGrounded = Physics2D.OverlapCircle( _groundChecker.position, 0.2f, _groundLayer );

			return isGrounded;
		}
		
		public void Tick()
		{
			if (_rigidbody.velocity.y >= 0.1f || !IsGrounded() )
			{
				_root.transform.parent = null;
				return;
			}
			
			if (TryGetPlatform( out var platform ))
			{
				_root.transform.parent = platform.transform;	
			}
		}
		
		bool TryGetPlatform( out Transform platform )
		{
			var ray = Physics2D.Raycast( _root.position, Vector2.down, 0.1f, _groundLayer );
			var collider = ray.collider;

			if ( collider != null && collider.gameObject.TryGet<PlatformTag>( out _))
			{
				platform = collider.transform;
				return true;
			}

			platform = null;
			return false;
		}
	}
}