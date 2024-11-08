namespace Game.Scripts
{
	using System;

	public interface ITimeHelper
	{
		TimeSpan SetTimeEnd(TimeSpan timeEnd);
		
		bool IsTimeEnd(TimeSpan endTime);
	}
	
	public class TimeHelper : ITimeHelper
	{
		public TimeSpan SetTimeEnd( TimeSpan timeEnd )
		{
			return TimeSpan.FromSeconds( DateTime.UtcNow.Ticks + timeEnd.Ticks );
		}

		public bool IsTimeEnd(TimeSpan endTime)
		{
			return DateTime.UtcNow.Ticks >= endTime.Ticks;
		}
	}
}