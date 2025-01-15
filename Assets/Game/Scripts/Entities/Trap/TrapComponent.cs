namespace Game.Entities
{
	using Components;
	using UnityEngine;

	public class TrapComponent : ICollisionEnter2D
	{
		Entity _entity;
		float _damage;

		public TrapComponent( Entity entity, float damage )
		{
			_damage = damage;
			_entity = entity;
		}
		
		public void OnCollisionEnter2D(Entity e)
		{
			if (e.TryGet( out HealthComponent component ))
			{
				component.TakeDamage(_damage);
				_entity.Destroy();
			}
		}
	}
}