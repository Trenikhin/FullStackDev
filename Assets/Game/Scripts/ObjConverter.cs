namespace Game.Scripts
{
	using System;
	using UnityEngine;
	using R3;

	/// <summary>
	/// UTF-8
	/// Базовый конвертер - перерабатывает что угодно во что угодоно.
	/// В Тз делался акцент именно на конвертации бревен в доски, но по сути для реализации логики конвертера тип материала не имеет значения
	/// Исходя из этого было принято решение пренебречь типом материала, тем самым упростив решение. 
	/// Также это позволит воспользоваться паттерном 'Decorator' чтобы расширить функционал и сделать конвертацию под любой тип по необходимости.
	/// </summary>
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
		// Configs
		ConvertConfig _config;
		
		// Services
		ITimeHelper _timeHelper;
		
		readonly ObjStack _raw;
		readonly ObjStack _inProgress;
		readonly ObjStack _converted;
		
		public int RawCapacity => _raw.Capacity;
		public int RawMaterialsAmount => _raw.Amount;
		public int ConvertedCapacity => _converted.Capacity;
		public int ConvertedMaterialsAmount => _converted.Amount;
		
		public TimeSpan ConvertTime { get; private set; }
		public int CycleInput => _config.InputAmount;
		public int CycleOutput => _config.OutputAmount;
		
		CompositeDisposable _disposables = new ();
		
		ReactiveProperty<bool> _isConverting = new (false);
		ReactiveProperty<bool> _isOn = new (false);
		
		public ObjConverter( ConvertConfig config, ITimeHelper timeHelper )
		{
			_config			= config;
			_timeHelper		= timeHelper;
			
			ConvertTime		= _timeHelper.GetTimeEnd( TimeSpan.FromSeconds( _config.ConvertTime ) );
			
			_raw			= new ObjStack( 0, config.RawMaterialsCapacity);
			_inProgress		= new ObjStack( 0, config.InputAmount);
			_converted		= new ObjStack( 0, config.ConvertedMaterialsCapacity);

			//Initialize(); // Just for tests
		}
		
		public void Initialize()
		{
			HandleCycle();
		}

		public void Dispose()
		{
			Debug.Log("Dispose");
			_disposables?.Dispose();
		}

		public bool IsOn => _isOn.Value;
		
		public void Toggle(bool isOn)
		{
			_isOn.Value = isOn;
		}

		public void PushRaw( int rawMaterials, out int outOfCapacity)
		{
			_raw.SetAmount( rawMaterials );
			
			outOfCapacity = 1;
		}
		
		public int PullConverted(int amount)
		{
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
			
			int inProgressAmount	= _config.InputAmount;
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
			_converted.SetAmount( _config.OutputAmount );
			
			_isConverting.Value = false;
		}

		bool IsTimeEnd() => _timeHelper.IsTimeEnd( ConvertTime );
		
		bool CanStartCycle( bool isOn, bool isAlreadyConverting, int rawMaterialsAmount, int amountTgt )
		{
			return isOn &&
			       !isAlreadyConverting &&
			       rawMaterialsAmount >= amountTgt;
		}
	}
}