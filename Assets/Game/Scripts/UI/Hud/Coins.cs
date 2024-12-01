namespace Game.UI
{
	using System;
	using Modules.Money;
	using UniRx;
	using Zenject;

	public interface ICoins
	{
		IReadOnlyReactiveProperty<int> Showing { get; } // Coins to show != current player coins
		
		void Hide(int coins);
	}
	
	public class Coins : ICoins , IInitializable, IDisposable
	{
		[Inject] IMoneyStorage _storage;
		
		ReactiveProperty<int> _show = new (0);
		ReactiveProperty<int> _hidden = new (0);
		
		public void Initialize()
		{
			Showing = Observable
				.CombineLatest( _show, _hidden, (c, h) => c - h )
				.ToReadOnlyReactiveProperty();
			
			_show.Value = _storage.Money;
			_storage.OnMoneyChanged += OnChanged;
		}

		public void Dispose()
		{
			_storage.OnMoneySpent -= OnChanged;
		}
		
		public IReadOnlyReactiveProperty<int> Showing { get; private set; }
		
		public void Hide( int coins )
		{
			_hidden.Value = coins;
		}

		void OnChanged(int newValue, int oldValue)
		{
			_show.Value = newValue;
		}
	}
}