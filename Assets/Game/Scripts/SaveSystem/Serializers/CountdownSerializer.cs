﻿namespace Game.SaveSystem
{
	using SampleGame.Gameplay;
	
	public class CountdownSerializer : BaseComponentSerializer<Countdown, float>
	{
		protected override float Get(Countdown component)
		{
			return component.Current;
		}

		protected override void Set(Countdown component, float data)
		{
			component.Current = data;
		}
	}
}