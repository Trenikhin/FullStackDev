namespace Game.Scripts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Cysharp.Threading.Tasks;
	using UnityEngine;
	using R3;

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
		void Push(ObjStack obj, out int outOfCapacity);
		ObjStack Pull(int amount);
	}
	
	public class ObjConverter : IInitializeble, IDisposable, IObjConverter
	{
		ConvertConfig _config;
		ITimeHelper _timeHelper;
		
		public int RawCapacity { get; private set; }
		public int RawMaterialsAmount { get; private set; }
		public int ConvertedCapacity { get; private set; }
		public int ConvertedMaterialsAmount { get; private set; }
		public TimeSpan ConvertTime { get; private set; }
		public int CycleInput { get; private set; }
		public int CycleOutput { get; private set; }
		
		IDisposable _cycleTicks;
		CompositeDisposable _disposables = new CompositeDisposable();
		
		ReactiveProperty<bool> _isConverting = new ReactiveProperty<bool>(false);
		ReactiveProperty<bool> _isOn = new ReactiveProperty<bool>(false);
		
		ConverterZone RawMaterials;
		ConverterZone ReadyMaterials;
		
		public ObjConverter( ConvertConfig config, ITimeHelper timeHelper )
		{
			_config = config;
			_timeHelper = timeHelper;
		}
		
		public void Initialize()
		{
			// Start new Cycle
			Observable
				.Merge
				(
					_isOn.AsUnitObservable(),
					_isConverting.AsUnitObservable(),
					RawMaterials.AmountRx.AsUnitObservable()
				) // isOn + not converting + enough materials
				.Where(_ =>
					_isOn.Value &&
					RawMaterials.AmountRx.Value >= CycleInput &&
					!_isConverting.Value 
				 )
				.Subscribe( _ => StartCycle() )
				.AddTo(_disposables);
			
			// Stop Cycle
			_isOn
				.Where(_ => !_isOn.Value)
				.Subscribe( _ => StopCycle() )
				.AddTo(_disposables);
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		public bool IsOn => _isOn.Value;
		
		public void Toggle(bool isOn)
		{
			throw new NotImplementedException();
		}

		public void Push(ObjStack obj, out int outOfCapacity)
		{
			outOfCapacity = 1;
		}
		
		public ObjStack Pull(int amount)
		{
			return new ObjStack();
		}
		
		void StartCycle()
		{
			EnterCycle();
			
			// Start ticks
			_cycleTicks?.Dispose();
			_cycleTicks = Observable
				.EveryUpdate()
				.Subscribe( _ => TickCycle() )
				.AddTo( _disposables );
		}

		void StopCycle()
		{
			
		}

		void EnterCycle()
		{
			_isConverting.Value = true;
			
			// Get materials
		}
		
		void TickCycle()
		{
			if (IsTimeEnd())
				ExitCycle();
		}

		void ExitCycle()
		{
			// Push new materials
			
			_isConverting.Value = false;
		}

		bool IsTimeEnd() => true;
	}

	public class ConverterZone
	{
		public int Capacity { get; private set; }
		public int MaterialsAmount { get; private set; }
		
		public ReactiveProperty<int> AmountRx { get; private set; }
		
		public ObjStack Pull(int amount)
		{
			throw new NotImplementedException();
		}

		public void Push(ObjStack obj, out int outOfCapacity)
		{
			throw new NotImplementedException();
		}
	}
	
	public interface IInitializeble
	{
		void Initialize();
	}
}