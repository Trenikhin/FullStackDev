namespace ShootEmUp
{
	using UnityEngine;
	using System;

	
	public class Health : MonoBehaviour, IDamageable
	{
		public Action< int> OnHealthChanged;
		public Action       OnHealthEmpty;
		
		[field: SerializeField] public int Value {get; private set;}
		
		public void TakeDamage(int damage)
		{
			Debug.Log( $"{gameObject}: {Value}" );
			
			if (Value <= 0)
				return;

			Value = Mathf.Max(0, Value - damage);
			OnHealthChanged?.Invoke(Value);

			if (Value <= 0)
				OnHealthEmpty?.Invoke();
		}
	}
}