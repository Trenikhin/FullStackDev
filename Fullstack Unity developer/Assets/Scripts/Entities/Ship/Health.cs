namespace ShootEmUp
{
	using UnityEngine;
	using System;

	public interface IDamageable
	{
		void TakeDamage(int damage);
	}
	
	public class Health : MonoBehaviour, IDamageable
	{
		public Action< int> OnHealthChanged;
		public Action       OnHealthEmpty;
		
		[SerializeField] int _health = 100;
		[field: SerializeField] public int Value {get; private set;}
		public int MaxHealth => _health;
		
		public void Start()
		{
			Value = _health;
		}

		public void ResetHealth()
		{
			Value = MaxHealth;
		}

		public void TakeDamage(int damage)
		{
			if (Value <= 0)
				return;

			Value = Mathf.Max(0, Value - damage);
			OnHealthChanged?.Invoke(Value);

			if (Value <= 0)
				OnHealthEmpty?.Invoke();
		}
	}
}