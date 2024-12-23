namespace Game.SaveSystem
{
	using SampleGame.Gameplay;
	using UnityEngine;
		
	public class DestinationSerializer : BaseComponentSerializer<DestinationPoint, Vector3>
	{
		protected override Vector3 Get(DestinationPoint component)
		{
			return component.Value;
		}

		protected override void Set(DestinationPoint component, Vector3 data)
		{
			component.Value = data;
		}
	}
}