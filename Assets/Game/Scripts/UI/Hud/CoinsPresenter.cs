namespace Game.UI
{
	using System;
	using Modules.Money;
	using UniRx;
	using Zenject;
	
	public class CoinsPresenter : IInitializable, IDisposable
	{
		[Inject] ICoinsView _coinsView;
		[Inject] IMoneyStorage _storage;
		
		CompositeDisposable _disposables = new ();
		
		public void Initialize()
		{
			_coinsView.Text = _storage.Money.ToString();
			_storage.OnMoneyChanged += Spend;
		}

		public void Dispose()
		{
			_storage.OnMoneyChanged -= Spend;
			_disposables?.Dispose();
		}

		void Spend(int newValue, int oldValue)
		{
			_coinsView.Text = newValue.ToString();
		}
	}
}