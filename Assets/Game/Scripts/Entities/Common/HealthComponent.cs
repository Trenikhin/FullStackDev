namespace Game.Components
{
	using System;
	using Entities;
	using UnityEngine;
	using Object = UnityEngine.Object;

	public class HealthComponent
	{
		float _health;
		float _maxHealth;
		Entity _entity;
		
		public HealthComponent( Entity entity, float health, float maxHealth )
		{
			_maxHealth = maxHealth;
			_health = health;
			_entity = entity;
		}
		
		public float MaxHealth => _maxHealth;
		
		public void TakeDamage(float damage)
		{
			_health = Math.Clamp( _health - damage, 0, _maxHealth );

			if (_health <= 0)
				_entity.Destroy();
		}
	}
}