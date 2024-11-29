namespace Game.UI
{
	using System;
	using UniRx;
	using Zenject;

	public class CoinsPresenter : IInitializable, IDisposable
	{
		[Inject] ICoins _coins;
		[Inject] ICoinsView _coinsView;
		
		CompositeDisposable _disposables = new ();
		
		public void Initialize()
		{
			_coins.Showing
				.Subscribe( v => _coinsView.Text = v.ToString() )
				.AddTo(_disposables);
		}

		public void Dispose() => _disposables?.Dispose();
	}
}