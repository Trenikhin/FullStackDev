namespace Game.Obj
{
	using System;
	using Configs;

	public class Converter
	{
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
			
			ConvertedCapacity = config.ConvertedMaterialsCapacity;
			RawCapacity = config.RawMaterialsCapacity;
			CycleInput = config.InputAmount;
			CycleOutput = config.OutputAmount;
			RawMaterialsAmount = rawAmount;
			RawCapacity = config.RawMaterialsCapacity;
			ConvertedMaterialsAmount = convertedAmount;
			ConvertedCapacity = config.ConvertedMaterialsCapacity;
			ConvertTime = config.ConvertTime;
			IsOn = isOn;

			// for enter from save: not implemented yet
			_isRecycling = false;
			_remainingTime = 0;
		}
		
		public bool IsOn {get; private set;}
		public int RawMaterialsAmount {get; private set;}
		public int ConvertedMaterialsAmount	{get; private set;}
		public int RawCapacity {get;}
		public int ConvertedCapacity {get;}
		public int CycleInput {get;}
		public int CycleOutput {get;}
		public float ConvertTime {get;}
		
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
			RawMaterialsAmount = RawMaterialsAmount - CycleInput;
			
			SetTimeLeft(ConvertTime);
			
			return true;
		}
				
		public void TickRecycling( float deltaTime ) // deltaTime: time(in sec) since last Tick
		{
			ReduceTime( deltaTime );
		}
		
		public void StopRecycling()
		{
			if (!_isRecycling)
				throw new Exception("Can't stop recycling if not running");
			
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