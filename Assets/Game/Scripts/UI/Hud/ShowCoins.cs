namespace Game.UI
{
	using System;
	using DG.Tweening;
	using Modules.Money;
	using UniRx;
	using Zenject;

	public interface IShowCoins
	{
		IReadOnlyReactiveProperty<int> Current { get; }

		void Set(int coins);
	}
	
	public class ShowCoins : IShowCoins , IInitializable, IDisposable
	{
		ReactiveProperty<int> _show = new (0);

		[Inject] IMoneyStorage _storage;
		
		public void Initialize()
		{
			_show.Value = _storage.Money;
		
			_storage.OnMoneySpent += Spend;
		}

		public void Dispose()
		{
			_storage.OnMoneySpent -= Spend;
		}
		
		public IReadOnlyReactiveProperty<int> Current	=> _show;

		public void Set( int coins ) => _show.Value = coins;

		void Spend(int newValue, int oldValue) => _show.Value = newValue;
	}
}