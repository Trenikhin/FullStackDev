namespace Game.Entities
{
	using UnityEngine;
	using Zenject;
	
	[RequireComponent(typeof(Collider2D))]
	public class PhysicsProvider2D : MonoBehaviour
	{
		[InjectOptional] ITriggerEnter2D[] _triggerEnter;
		[InjectOptional] ITriggerExit2D[] _triggerExit;
		[InjectOptional] ITriggerStay2D[] _triggerStay;
		[InjectOptional] ICollisionEnter2D[] _collisionsEnter;
		[InjectOptional] ICollisionExit2D[] _collisionsExit;
		
		public void OnTriggerStay2D(Collider2D other)
		{
			foreach (var t in _triggerStay)
			{
				if (other.gameObject.TryGetEntity(out var e))
					t.OnTriggerStay2D( e );
			}
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			foreach (var t in _triggerEnter)
			{
				if (other.gameObject.TryGetEntity(out var e))
					t.OnTriggerEnter2D( e );
			}
		}

		public void OnTriggerExit2D(Collider2D other)
		{
			foreach (var t in _triggerExit)
			{
				if (other.gameObject.TryGetEntity(out var e))
					t.OnTriggerExit2D( e );
			}
		}

		public void OnCollisionEnter2D(Collision2D other)
		{
			foreach (var t in _collisionsEnter)
			{
				if (other.gameObject.TryGetEntity(out Entity e))
					t.OnCollisionEnter2D( e );
			}
		}
		
		public void OnCollisionExit2D(Collision2D other)
		{
			foreach (var t in _collisionsExit)
			{
				if (other.gameObject.TryGetEntity(out var e))
					t.OnCollisionExit2D( e );
			}
		}
	}
}