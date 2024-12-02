namespace Game.UI
{
	using UniRx;
	using Zenject;

	public class CoinsUi : IUi {}
	
	public class UiCoinsPresenter : IShowingPresenter<CoinsUi>
	{
		[Inject] ICoins _coins;
		[Inject] ICoinsView _coinsView;
		
		CompositeDisposable _disposables = new ();

		protected override void OnShow(CoinsUi arg)
		{
			_disposables = new ();
			
			_coinsView.ShowHide( true );
			
			// Update coins
			_coins.Showing
				.Subscribe( v => _coinsView.Text = v.ToString() )
				.AddTo(_disposables);
		}
		
		protected override void OnHide()
		{
			_coinsView.ShowHide( false );
			
			_disposables?.Dispose();
		}
	}
}