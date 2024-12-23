namespace Game.SaveSystem
{
	using Modules.Entities;
	using SampleGame.Gameplay;
	using Zenject;

	public class TargetObjectSerializer : BaseComponentSerializer<TargetObject, int>
	{
		[Inject] EntityWorld _world;
		
		protected override int Get(TargetObject component)
		{
			return component.Value != null ? component.Value.Id : default;
		}

		protected override void Set(TargetObject component, int data)
		{
			component.Value = data == default ? null : _world.Get(data);
		}
	}
}