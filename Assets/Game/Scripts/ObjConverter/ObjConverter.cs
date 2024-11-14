namespace Game.Obj
{
	using System;
	using Configs;
	using R3;
	
	public class ObjConverter : IDisposable
	{
		readonly ConvertConfig _config;
		readonly ObjStack _raw;
		readonly ObjStack _converted;
		readonly ReactiveProperty<bool> _isOn;
		readonly RecyclingTimer _timer;
		
		// Lifetime
		readonly CompositeDisposable _disposables = new ();
		
		public ObjConverter(ConvertConfig config, bool isOn)
		{
			if ( config == null )
				throw new ArgumentNullException(nameof(config));
			
			ConvertedCapacity = config.ConvertedMaterialsCapacity;
			RawCapacity = config.RawMaterialsCapacity;
			CycleInput = config.InputAmount;
			CycleOutput = config.OutputAmount;
			ConvertTime	= TimeSpan.FromSeconds( config.ConvertTime );
			
			_raw = new ObjStack( 0, RawCapacity);
			_converted = new ObjStack( 0, ConvertedCapacity);
			_timer = new RecyclingTimer();
			
			_isOn = new ReactiveProperty<bool>(isOn);
			
			Initialize(); // Just for tests
		}
		
		public void Initialize()
		{
			HandleCycle();
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}
		
#region IObjConverter

		public int RawMaterialsAmount => _raw.Amount;
		public int ConvertedMaterialsAmount => _converted.Amount;
		public int RawCapacity { get; }
		public int ConvertedCapacity { get; }
		public TimeSpan ConvertTime { get; }
		public int CycleInput { get; }
		public int CycleOutput { get; }
		public bool IsOn => _isOn.Value;
		
		public void Toggle(bool isOn) => _isOn.Value = isOn;

		public void PushRaw( int rawMaterials, out int outOfCapacity)
		{
			if (rawMaterials <= 0)
				throw new ArgumentException("Raw materials must be greater than 0");
			
			int newAmount = RawMaterialsAmount + rawMaterials;
			outOfCapacity = newAmount > RawCapacity ? 
				newAmount - RawCapacity :
				0;
			
			_raw.SetAmount( newAmount - outOfCapacity );
		}
		
#endregion
		
		void HandleCycle()
		{
			// Start new Cycle
			Observable
				.Merge
				(
					_isOn.AsUnitObservable(), 
					_timer.State.AsUnitObservable(), 
					_raw.AmountRx.AsUnitObservable(),
					_converted.AmountRx.AsUnitObservable()
				)
				.Where
				(_ => // Start cycle condition:
					_isOn.Value && // IsOn													
					_timer.State.CurrentValue != ETimerState.Running && // Not converting
					RawMaterialsAmount >= CycleInput && // Enough materials
					ConvertedMaterialsAmount + CycleOutput <= ConvertedCapacity // Enough capacity
				)
				.Subscribe( _ => _timer.Start( ConvertTime ) )
				.AddTo(_disposables);

			// Stop Cycle
			Observable
				.Merge( _isOn.AsUnitObservable(), _timer.State.AsUnitObservable() )
				.Where(_ => !_isOn.Value && _timer.State.CurrentValue == ETimerState.Running)
				.Subscribe( _ => _timer.Stop() )
				.AddTo(_disposables);
			
			// Handle Cycle
			_timer.State
				.Subscribe( s =>
				{
					switch (s)
					{
						case ETimerState.Running:	StartRecycling(); break;
						case ETimerState.Ready:		FinishRecycling(); break;
						case ETimerState.Stopped:	StopRecycling(); break;
					}
				})
				.AddTo(_disposables);
		}

		void StartRecycling()
		{
			int inProgressAmount = CycleInput;
			int remainingRaw = RawMaterialsAmount - inProgressAmount;
			
			_raw.SetAmount( remainingRaw );
		}

		void StopRecycling()
		{
			// When overflowed, out of capacity resources are "burned".
			int amount = Math.Min( RawCapacity, RawMaterialsAmount + CycleInput );
			
			// returns resources back to the loading zone.
			_raw.SetAmount( amount );
		}

		void FinishRecycling()
		{
			var convertedMaterials = ConvertedMaterialsAmount + CycleOutput;
			
			_converted.SetAmount( convertedMaterials );
		}
	}
}