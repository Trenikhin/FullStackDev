namespace Game
{
	using System;
	using R3;

	public enum ETimerState
	{
		None,
		
		Stopped,
		Ready,
		Running,
	}
	
	public class RecyclingTimer: IDisposable
	{
		ReactiveProperty<ETimerState> _state = new (ETimerState.None);

		CompositeDisposable _disposables = new ();
		IDisposable _ticks;

		public RecyclingTimer() => State = _state;
		
		public void Dispose() => _disposables?.Dispose();
		
		public readonly ReadOnlyReactiveProperty<ETimerState> State;
		
		public void Start( TimeSpan time )
		{
			_state.Value = ETimerState.Running;
			
			_ticks = Observable
				.Timer( time )
				.Subscribe( _ => _state.Value = ETimerState.Ready )
				.AddTo(_disposables);
		}

		public void Stop()
		{
			_ticks?.Dispose();
			_state.Value = ETimerState.Stopped;
		}
	}
}