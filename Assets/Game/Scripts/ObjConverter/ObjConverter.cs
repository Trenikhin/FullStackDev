namespace Game.Obj
{
	using Cysharp.Threading.Tasks;
	using System;
	using System.Threading;
	using Configs;
	using R3;
	
	public class ObjConverter : IDisposable
	{
		readonly ReactiveProperty<bool> _isRecycling;
		readonly ReactiveProperty<bool> _isOn;
		readonly ObjStack _raw;
		readonly ObjStack _converted;
		
		CancellationTokenSource _cts;
		
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

			_isOn = new ReactiveProperty<bool>(isOn);
			_isRecycling = new ReactiveProperty<bool>();
			
			Initialize(); // Just for tests
		}
		
		public void Initialize()
		{
			HandleCycle();
		}

		public void Dispose()
		{
			_disposables?.Dispose();
			_cts?.Cancel();
			_cts?.Dispose();
		}
		
#region IObjConverter

		public int RawMaterialsAmount => _raw.Amount;
		public int RawCapacity { get; private set; }
		public int ConvertedMaterialsAmount => _converted.Amount;
		public int ConvertedCapacity { get; private set; }
		public TimeSpan ConvertTime { get; private set; }
		public int CycleInput { get; private set; }
		public int CycleOutput { get; private set; }
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
					_isRecycling.AsUnitObservable(),
					_raw.AmountRx.AsUnitObservable(),
					_converted.AmountRx.AsUnitObservable()
				)
				.Where
				(v => 
						_isOn.Value && // IsOn													
						!_isRecycling.Value && // Not converting
						RawMaterialsAmount >= CycleInput && // Enough materials
						ConvertedMaterialsAmount + CycleOutput <= ConvertedCapacity // Enough capacity
				)
				.Subscribe( _ =>
				{
					_cts = new CancellationTokenSource();
					StartRecyclingProcess(_cts.Token).Forget();
				})
				.AddTo(_disposables);

			// Stop Cycle
			_isOn
				.Where(_ => !_isOn.Value && _isRecycling.Value)
				.Subscribe( _ => _cts?.Cancel() )
				.AddTo(_disposables);
		}
				
		async UniTaskVoid StartRecyclingProcess( CancellationToken ct )
		{
			_isRecycling.Value = true;
			StartRecycling();

			try
			{
				await UniTask.Delay(ConvertTime, cancellationToken: ct);
				FinishRecycling();
			}
			catch (OperationCanceledException)
			{
				StopRecycling();
			}
			finally
			{
				_isRecycling.Value = false;
			}
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