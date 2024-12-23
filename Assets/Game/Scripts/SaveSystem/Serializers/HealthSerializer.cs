namespace Game.SaveSystem
{
	using SampleGame.Gameplay;

	public class HealthSerializer : BaseComponentSerializer<Health, int>
	{
		protected override int Get(Health component)
		{
			return component.Current;
		}
		
		protected override void Set(Health component, int data)
		{
			component.Current = data;
		}
	}
}