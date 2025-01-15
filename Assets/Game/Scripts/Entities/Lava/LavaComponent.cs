namespace Game.Components
{
	using Entities;
	using UnityEngine;
		
	public class LavaComponent : ITriggerEnter2D
	{
		public void OnTriggerEnter2D(Entity e)
		{
			if (e.TryGet( out HealthComponent component ))
			{
				component.TakeDamage( component.MaxHealth );	
			}
		}
	}
}