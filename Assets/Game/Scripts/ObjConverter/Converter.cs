namespace Game.Obj
{
	using System;
	using Configs;

	public class Converter
	{
		ConvertConfig _config;
		bool _isRecycling;
		float _remainingTime;
		
		public Converter(ConvertConfig config, int rawAmount = 0, int convertedAmount = 0, bool isOn = false)
		{
			if ( config == null )
				throw new ArgumentNullException(nameof(config));
			if ( rawAmount > config.RawMaterialsCapacity )
				throw new ArgumentException( "The input amount is out of range.", nameof(rawAmount) );
			if ( convertedAmount > config.ConvertedMaterialsCapacity )
				throw new ArgumentException( "The input amount is out of range.", nameof(convertedAmount) );
			if ( rawAmount < 0 )
				throw new ArgumentException( "The input amount is negative.", nameof(rawAmount) );
			if ( convertedAmount < 0 )
				throw new ArgumentException( "The input amount is negative.", nameof(convertedAmount) );
			
			_config = config;
			RawMaterialsAmount = rawAmount;
			ConvertedMaterialsAmount = convertedAmount;
			IsOn = isOn;
		}
		
		public bool IsOn {get; private set;}
		public int RawMaterialsAmount {get; private set;}
		public int ConvertedMaterialsAmount	{get; private set;}
		public int RawCapacity => _config.RawMaterialsCapacity;
		public int ConvertedCapacity => _config.ConvertedMaterialsCapacity;
		public int CycleInput => _config.InputAmount;
		public int CycleOutput => _config.OutputAmount;
		public float ConvertTime => _config.ConvertTime;
		
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
			if (_isRecycling ||
			    !IsOn ||
			    RawMaterialsAmount < CycleInput	||
			    ConvertedMaterialsAmount + CycleOutput >= ConvertedCapacity )
				return false;
			
			_isRecycling = true;
			RawMaterialsAmount -= CycleInput;
			SetTimeLeft(ConvertTime);
			
			return true;
		}
				
		public void TickRecycling( float deltaTime ) // deltaTime: time(in sec) since last Tick
		{
			ReduceTime( deltaTime );
			TryStop();
		}

		void TryStop()
		{
			if (IsOn && !IsRecyclingEnd() && !_isRecycling)
				return;
			
			if (IsRecyclingEnd())
			{
				// Finish Recycling
				ConvertedMaterialsAmount += CycleOutput;
			}
			else
			{
				// Not Finished yet
				// When overflowed, out of capacity resources are "burned".
				RawMaterialsAmount = Math.Min( RawCapacity, RawMaterialsAmount + CycleInput );
			}

			SetTimeLeft(0);
			_isRecycling = false;
		}

#region TimeLogic

		void ReduceTime(float delta) => SetTimeLeft( _remainingTime - delta );
		bool IsRecyclingEnd() => _remainingTime <= 0;
		void SetTimeLeft(float sec) => _remainingTime = sec;
		
#endregion
	}
}