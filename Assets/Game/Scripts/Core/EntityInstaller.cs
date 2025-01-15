namespace Game.Entities
{
	using System.Linq;
	using Sirenix.OdinInspector;
	using UnityEngine;
	using Zenject;

	[RequireComponent(typeof(EntityMonoProvider))]
	public abstract class EntityInstaller : MonoInstaller
	{
		[Title("Base")]
		[InfoBox("Add GameObjectContext and MonoProvider")]
		[SerializeField]
		bool _autoComplete = true;
		
#if UNITY_EDITOR
		void OnValidate()
		{
			if (!_autoComplete)
				return;
				
			if (gameObject.GetComponent<EntityMonoProvider>() == null)
				gameObject.AddComponent<EntityMonoProvider>();

			var context = gameObject.GetComponent<GameObjectContext>();
			
			if (context == null)
				context = gameObject.AddComponent<GameObjectContext>();
			
			if (!context.Installers.Any())
				context.Installers = gameObject.GetComponents<EntityInstaller>();
		}
#endif
		
		public override void InstallBindings()
		{
			Container
				.BindInstance(gameObject)
				.AsSingle();
			
			Container
				.BindInstance(transform)
				.AsSingle();
			
			Container
				.Bind<Entity>()
				.AsSingle();

			Container
				.Bind<EntityMonoProvider>()
				.FromComponentInHierarchy()
				.AsSingle();
		}
	}
}