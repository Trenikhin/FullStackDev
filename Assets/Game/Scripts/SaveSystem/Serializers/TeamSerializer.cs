namespace Game.SaveSystem
{
	using SampleGame.Common;
	using SampleGame.Gameplay;
	
	public class TeamSerializer : BaseComponentSerializer<Team, TeamType>
	{
		protected override TeamType Get(Team component)
		{
			return component.Type;
		}
	
		protected override void Set(Team component, TeamType data)
		{
			component.Type = data;
		}
	}
}