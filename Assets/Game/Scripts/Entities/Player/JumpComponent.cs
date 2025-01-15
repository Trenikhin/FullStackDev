namespace Game.Components
{
	using System;
	using Unity.Mathematics;
	using UnityEngine;
	using Zenject;

	public class JumpComponent : ITickable
	{
		float _force;
		Rigidbody2D _rigidbody2D;

		Func<bool> _canJump;
		float _cooldown = 0.1f;
		float _jumpTime = 0.1f;
		float _verticalSpeed;
		bool _isJumping;
		
		public JumpComponent(Rigidbody2D rb, float force)
		{
			_rigidbody2D = rb;
			_force = force;
		}

		public void AddJumpCondition( Func<bool> condition )
		{
			_canJump = condition;
		}
		
		public void Tick()
		{
			if (_canJump == null)
				return;
			if (!_canJump())
				return;
			
			if (Input.GetButtonDown("Jump") && _jumpTime + _cooldown < Time.time)
			{
				Jump();
			}
		}

		void Jump()
		{
			_rigidbody2D.AddForce(new Vector2(0, _force), ForceMode2D.Impulse);
			_jumpTime = Time.time;
		}
	}
}

