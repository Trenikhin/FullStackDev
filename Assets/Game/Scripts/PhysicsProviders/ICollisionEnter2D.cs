namespace Game.Entities
{
	using UnityEngine;

	public interface ICollisionEnter2D
	{
		void OnCollisionEnter2D(Entity entity);
	}
}