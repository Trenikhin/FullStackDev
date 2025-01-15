namespace Game.Entities
{
	using UnityEngine;
	using Zenject;

	public class EntityMonoProvider : MonoBehaviour
	{
		public readonly Entity Value;
		
		public EntityMonoProvider( Entity entity )
		{
			Value = entity;
		}
	}
	
	public static class EntityMonoProviderExtensions
	{
		public static bool TryGet<T>(this GameObject gameObject, out T component) where T : class
		{
			if (!gameObject.TryGetComponent(out EntityMonoProvider provider))
			{
				component = null;
				return false;	
			}
			
			return provider.Value.TryGet(out component);
		}
		
		public static bool TryGetEntity(this GameObject gameObject, out Entity entity)
		{
			if (!gameObject.TryGetComponent(out EntityMonoProvider provider))
			{
				entity = null;
				return false;	
			}
			
			entity = provider.Value;
			return true;
		}
	}
}