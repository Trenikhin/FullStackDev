namespace Game.Obj
{
	using System;
	using Configs;

	public class Converter
	{
		readonly ConvertConfig _config;

		bool	_isRecycling;
		float	_remainingTime;
		
		public Converter(ConvertConfig config, int rawAmount = 0, int convertedAmount = 0, bool isOn = false)
		{
			if ( config == null )
				throw new ArgumentNullException(nameof(config));
			if ( rawAmount > config.RawMaterialsCapacity )
				throw new ArgumentException( "The input amount is out of range.", nameof(rawAmount) );
			if ( convertedAmount > config.ConvertedMaterialsCapacity )
				throw new ArgumentException( "The input amount is out of range.", nameof(convertedAmount) );
			
			ConvertedCapacity	= config.ConvertedMaterialsCapacity;
			RawCapacity			= config.RawMaterialsCapacity;
			CycleInput			= config.InputAmount;
			CycleOutput			= config.OutputAmount;
			ConvertTime			= TimeSpan.FromSeconds( config.ConvertTime );
			
			RawMaterialsAmount			= rawAmount;
			RawCapacity					= config.RawMaterialsCapacity;
			ConvertedMaterialsAmount	= convertedAmount;
			ConvertedCapacity			= config.ConvertedMaterialsCapacity;
			
			IsOn				= isOn;
		}
		
		public int RawMaterialsAmount { get; private set; }
		public int RawCapacity { get; private set; }
		public int ConvertedMaterialsAmount { get; private set; }
		public int ConvertedCapacity { get; private set; }
		public TimeSpan ConvertTime { get; private set; }
		public int CycleInput { get; private set; }
		public int CycleOutput { get; private set; }
		public bool IsOn { get; private set; }
		
		public void Toggle(bool isOn)
		{
			if (IsOn == isOn)
				return;
			
			IsOn = isOn;
		}

		public void AddResources( int rawMaterials, out int outOfCapacity)
		{
			if (rawMaterials <= 0)
				throw new ArgumentException("Raw materials must be greater than 0");
			
			int newAmount = RawMaterialsAmount + rawMaterials;
			outOfCapacity = newAmount > RawCapacity ? 
				newAmount - RawCapacity :
				0;
			
			RawMaterialsAmount = newAmount - outOfCapacity;
		}
		
		public bool TryStartRecycle()
		{
			if (!_isRecycling					||
			    !IsOn							||
			    RawMaterialsAmount < CycleInput		||
			    ConvertedMaterialsAmount >= ConvertedCapacity )
				return false;
			
			_isRecycling				= true;
			
			int inProgressAmount		= CycleInput;
			int remainingRaw			= RawMaterialsAmount - inProgressAmount;

			RawMaterialsAmount			= remainingRaw;
			_remainingTime				= (float)ConvertTime.TotalSeconds;
			
			return true;
		}
				
		public void TickRecycling( float deltaTime ) // deltaTime: time(in sec) since last update
		{
			if (!_isRecycling)
				throw new Exception("Can't tick if recycling not started");
			
			_remainingTime -= deltaTime;
		}
		
		public void StopRecycling()
		{
			if (!_isRecycling)
				throw new Exception("Can't stop recycling if not running");
			
			if (_remainingTime >= 0)
			{
				ConvertedMaterialsAmount += CycleOutput;
			}
			else
			{
				// When overflowed, out of capacity resources are "burned".
				RawMaterialsAmount = Math.Min( RawCapacity, RawMaterialsAmount + CycleInput );
			}

			_remainingTime	= 0;
			_isRecycling	= false;
		}
	}
}