namespace Game.Scripts
{
	using System;
	using Codice.Client.Commands.WkTree;
	using UnityEngine;
	using R3;
	using UnityEngine.UI;
	using Toggle = UnityEngine.UIElements.Toggle;
	
	public interface IObjConverter
	{
		public bool IsOn {get;}
		public int RawCapacity { get; }
		public int RawMaterialsAmount { get; }
		public int ConvertedCapacity { get; }
		public int ConvertedMaterialsAmount { get; }
		public TimeSpan ConvertTime { get; }
		public int CycleInput { get; }
		public int CycleOutput { get; }

		void Toggle(bool isOn);
		void PushRaw(int amount, out int outOfCapacity);
		int PullConverted(int amount);
	}
	
	public interface IInitializable
	{
		void Initialize();
	}
	
	public class ObjConverter : IInitializable, IDisposable, IObjConverter
	{
		// Services
		ITimeHelper _timeHelper;
		
		readonly ObjStack _raw;
		readonly ObjStack _inProgress;
		readonly ObjStack _converted;

		TimeSpan _cycleEndTime;
		
		public int RawMaterialsAmount => _raw.Amount;
		public int RawCapacity { get; private set; }
		public int ConvertedMaterialsAmount => _converted.Amount;
		public int ConvertedCapacity { get; private set; }
		public int CycleInput { get; private set; }
		public int CycleOutput { get; private set; }
		
		
		public TimeSpan ConvertTime { get; private set; }
		
		CompositeDisposable _disposables = new ();
		
		ReactiveProperty<bool> _isConverting = new (false);
		ReactiveProperty<bool> _isOn = new (false);
		
		public ObjConverter // in a real game it might be a 'ConvertedConfig' instead of arguments (convertedAmount ..convertTime)
		(
			int convertedAmount,
			int convertedCapacity,
			int rawAmount,
			int rawCapacity,
			int cycleInput,
			int cycleOutput,
			float convertTime,
			ITimeHelper timeHelper
		)
		{
			ConvertedCapacity = convertedCapacity;
			RawCapacity = rawCapacity;
			CycleInput = cycleInput;
			CycleOutput = cycleOutput;
			
			_timeHelper	= timeHelper;
			ConvertTime	= TimeSpan.FromSeconds( convertTime );
			
			_raw = new ObjStack( rawAmount, RawCapacity);
			_inProgress = new ObjStack( 0, cycleInput);
			_converted = new ObjStack( convertedAmount, ConvertedCapacity);
			
			//Initialize(); // Just for tests
		}
		
		public void Initialize()
		{
			HandleCycle();
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		public bool IsOn => _isOn.Value;
		
		public void Toggle(bool isOn)
		{
			_isOn.Value = isOn;
		}

		public void PushRaw( int rawMaterials, out int outOfCapacity)
		{
			if (rawMaterials <= 0)
				throw new ArgumentException("Raw materials must be greater than 0");
			
			outOfCapacity = RawCapacity - (rawMaterials + RawMaterialsAmount);
			
			_raw.SetAmount( rawMaterials );
		}
		
		public int PullConverted(int amount)
		{
			if (amount <= 0)
				throw new ArgumentException("Amount must be greater than 0");
			if (amount > ConvertedMaterialsAmount)
				throw new ArgumentException("Amount is greater than the converted materials amount");
			
			return _converted.Amount;
		}
		
		void HandleCycle()
		{
			// Start new Cycle
			Observable
				.Merge
				(
					_isOn.AsUnitObservable(),
					_isConverting.AsUnitObservable(),
					_raw.AmountRx.AsUnitObservable()
				)
				.Select( _ => (
					isOn: _isOn.Value,
					isConverting: _isConverting.Value,
					rawCount: _raw.AmountRx.CurrentValue,
					needed: CycleInput ) ) 
				.Where(v => CanStartCycle( v.isOn, v.isConverting, v.rawCount, v.needed ))
				.Subscribe( _ => StartCycle() )
				.AddTo(_disposables);

			// Stop Cycle
			_isOn
				.Where(_ => !_isOn.Value)
				.Subscribe( _ => StopCycle() )
				.AddTo(_disposables);
		}
		
		void StartCycle()
		{
			EnterCycle();
			
			_cycleEndTime = _timeHelper.GetTimeEnd( ConvertTime );
			
			// Start cycle ticks
			Observable
				.EveryUpdate()
				.TakeWhile(_ => _isConverting.Value)
				.Subscribe( _ => TickCycle() )
				.AddTo( _disposables );
		}

		void StopCycle()
		{
			// If it stopped while converting
			if (!IsTimeEnd())
			{
				int amount		= Math.Min( RawCapacity, RawMaterialsAmount + _inProgress.Amount ); // 
				
				_raw.SetAmount( amount );
				_inProgress.SetAmount( 0);
			}
			
			_isConverting.Value = false;
		}

		void EnterCycle()
		{
			_isConverting.Value = true;
			
			int inProgressAmount	= CycleInput;
			int remainingRaw		= RawMaterialsAmount - inProgressAmount;
			
			_raw.SetAmount( inProgressAmount );
			_inProgress.SetAmount( remainingRaw );
		}
		
		void TickCycle()
		{
			if (IsTimeEnd())
				ExitCycle();
		}

		void ExitCycle()
		{
			_converted.SetAmount( CycleOutput );
			
			_isConverting.Value = false;
		}

		bool IsTimeEnd() => _timeHelper.IsTimeEnd( _cycleEndTime );
		
		bool CanStartCycle( bool isOn, bool isAlreadyConverting, int rawMaterialsAmount, int amountTgt )
		{
			return isOn &&
			       !isAlreadyConverting &&
			       rawMaterialsAmount >= amountTgt;
		}
	}
}