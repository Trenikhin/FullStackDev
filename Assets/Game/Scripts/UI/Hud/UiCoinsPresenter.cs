namespace Game.UI
{
	using System;
	using UniRx;
	using Zenject;

	public class CoinsUi : IUi {}
	
	public class UiCoinsPresenter : IInitializable, IDisposable
	{
		[Inject] ICoins _coins;
		[Inject] ICoinsView _coinsView;
		[Inject] IUiNavigator _uiNavigator;
		
		CompositeDisposable _disposables = new ();
		
		public void Initialize()
		{
			// Open
			_uiNavigator
				.OnShow<CoinsUi>()
				.Subscribe(m => _coinsView.ShowHide( true ) )
				.AddTo(_disposables);
			
			// Close
			_uiNavigator
				.OnHide<CoinsUi>()
				.Subscribe(m => _coinsView.ShowHide( false )  )
				.AddTo(_disposables);
			
			// Update coins
			_coins.Showing
				.Subscribe( v => _coinsView.Text = v.ToString() )
				.AddTo(_disposables);
		}
		
		public void Dispose() => _disposables?.Dispose();
	}
}