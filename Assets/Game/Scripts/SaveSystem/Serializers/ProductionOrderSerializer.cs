namespace Game.SaveSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using Modules.Entities;
	using SampleGame.Gameplay;
	using Zenject;

	public class ProductionOrderSerializer : BaseComponentSerializer<ProductionOrder, List<string>>
	{
		[Inject] EntityCatalog _catalog;
		
		protected override List<string> Get(ProductionOrder component)
		{
			return component.Queue.Select( c => c.Name ).ToList();
		}

		protected override void Set(ProductionOrder component, List<string> data)
		{
			var lst = new List<EntityConfig>();

			foreach (var n in data)
			{
				_catalog.FindConfig(n, out var cfg);
				lst.Add( cfg );
			}
			
			component.Queue = lst;
		}
	}
}