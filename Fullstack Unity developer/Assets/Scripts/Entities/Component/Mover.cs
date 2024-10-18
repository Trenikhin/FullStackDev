namespace ShootEmUp
{
	using UnityEngine;

	
	public class Mover : MonoBehaviour
	{
		[SerializeField] Rigidbody2D _rigidbody;
		
		[SerializeField] float _speed;
		
		
		public void Move( Vector2 moveDirection )
		{
			Vector2 moveStep       = moveDirection * Time.fixedDeltaTime * _speed;
			Vector2 targetPosition = _rigidbody.position + moveStep;
			_rigidbody.MovePosition(targetPosition);
		}
	}
}