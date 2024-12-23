namespace Game.SaveSystem
{
	using SampleGame.Common;
	using SampleGame.Gameplay;
	
	public class ResourceBagSerializer : BaseComponentSerializer<ResourceBag, (ResourceType type, int cur) >
	{
		protected override (ResourceType, int) Get(ResourceBag component)
		{
			return (component.Type, component.Current);
		}
		
		protected override void Set(ResourceBag component, (ResourceType type, int cur) data)
		{
			component.Type = data.type;
			component.Current = data.cur;
		}
	}
}