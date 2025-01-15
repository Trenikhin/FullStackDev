namespace Game.Components
{
	using UnityEngine;
	using Zenject;

	public class MoveComponent : ITickable, IFixedTickable
	{
		float _speed;
		Rigidbody2D _rb; 
		float _moveSpeed;
		readonly Transform _face;

		public MoveComponent( Transform face, Rigidbody2D rb, float speed )
		{
			_rb = rb;
			_speed = speed;
			_face = face;
		}
    
		public void Tick()
		{
			var direction = Input.GetAxis("Horizontal");
			_moveSpeed = direction * _speed;
		}
		
		public void FixedTick()
		{
			_rb.velocity = IsWall() ?
				new Vector2(0, _rb.velocity.y) :
				new Vector2(_moveSpeed, _rb.velocity.y);
		}

		bool IsWall()
		{
			var direction = new Vector2(_moveSpeed, 0);
			var distance = Mathf.Abs(_moveSpeed) * Time.fixedDeltaTime;
			var layer = LayerMask.GetMask("Ground");
			var hit = Physics2D.Raycast(_face.position, direction.normalized, distance, layer );
			
			return hit.collider != null;
		}
	}
}