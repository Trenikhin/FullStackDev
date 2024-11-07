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
		
		public int RawMaterialsCapacity {get;}
		public int ConvertedMaterialsCapacity {get;}
		public int InputAmount {get;}
		public int OutputAmount {get;}
		public TimeSpan ConvertTime {get;}
		public float ConvertRemainingTime {get;}

		void Toggle(bool isOn);

		void PushRaw( ObjStack obj, out int outOfCapacity);
		ObjStack PullConverted();
	}
	
	public class ObjConverter : IDisposable, IObjConverter
	{
		Stack<ObjType> _rowItems = new Stack<ObjType>();
		Stack<ObjType> _inProgress = new Stack<ObjType>();
		Stack<ObjType> _convertedItems = new Stack<ObjType>();
		
		float _time;
		bool _isConverting;

		public bool IsOn { get; private set; }
		public int RawMaterialsCapacity { get; private set; }
		public int ConvertedMaterialsCapacity { get; private set; }
		public int InputAmount { get; }
		public int OutputAmount { get; private set; }
		public TimeSpan ConvertTime {get; private set;}
		public float ConvertRemainingTime { get; private set; }

		CompositeDisposable _disposables = new CompositeDisposable();
		IDisposable _ticks;

		public ObjConverter( int rawMaterialsCapacity, int convertedMaterialsCapacity, int outputAmount, int inputAmount, float convertTime )
		{
			if (rawMaterialsCapacity < 0)
				throw new ArgumentOutOfRangeException(nameof(rawMaterialsCapacity));
			if (convertedMaterialsCapacity < 0)
				throw new ArgumentOutOfRangeException(nameof(convertedMaterialsCapacity));
			if (convertTime < 0) throw new ArgumentOutOfRangeException(nameof(convertTime));

			InputAmount = inputAmount;
			OutputAmount = outputAmount;
			RawMaterialsCapacity = rawMaterialsCapacity;
			ConvertedMaterialsCapacity = convertedMaterialsCapacity;
			ConvertTime = TimeSpan.FromSeconds(convertTime); 
			ConvertRemainingTime = convertTime;
		}
		
		public void Dispose()
		{
			_disposables?.Dispose();
			_ticks?.Dispose();
		}

		public void Toggle(bool isOn)
		{
			return;
			if (isOn && !_isConverting)
				RestartConverting();
			else 
				StopConverting();
		}

		public void PushRaw( ObjStack obj, out int outOfCapacity)
		{
			outOfCapacity = 0;
		}

		public ObjStack PullConverted()
		{
			return new ObjStack();
		}

		void RestartConverting()
		{
			if (_isConverting)
				StopConverting();
			
			Enter_Convert();
			_ticks = Observable
				.EveryUpdate()
				.Subscribe( _ => Tick_Convert() )
				.AddTo(_disposables);
		}
		
		void StopConverting()
		{
			_ticks?.Dispose();
			Exit_Convert();	
		}
		
		void Enter_Convert()
		{
			_isConverting = true;
			
			for (int i = 0; i < OutputAmount; i++)
				_inProgress.Push( _rowItems.Pop() );	

			_time = 10;
		}
		
		void Tick_Convert()
		{
			_time -= Time.deltaTime;

			if (_time <= 0)
				RestartConverting();
		}

		void Exit_Convert()
		{
			for (int i = 0; i < _inProgress.Count; i++)
				_convertedItems.Push( _inProgress.Pop() );
			
			_isConverting = false;
		}
	}
}