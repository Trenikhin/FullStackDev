namespace Game.UI
{
	using System;
	using UniRx;
	using Zenject;

	public abstract class IShowingPresenter<T> : IInitializable, IDisposable where T : IUi
	{
		[Inject] IUiNavigator _uiNavigator;
		
		CompositeDisposable _lifetimeDisposables = new ();
		
		public void Initialize()
		{
			// Show
			_uiNavigator
				.OnShow<T>()
				.Subscribe(OnShow )
				.AddTo(_lifetimeDisposables);
			
			// Hide
			_uiNavigator
				.OnHide<T>()
				.Subscribe(m => OnHide() )
				.AddTo(_lifetimeDisposables);
		}

		public void Hide() => _uiNavigator.Hide<T>();
		
		protected abstract void OnShow(T arg);
		protected abstract void OnHide();

		public void Dispose() => _lifetimeDisposables?.Dispose();
	}
}