namespace Game.UI
{
	using System;
	using UniRx;
	using Zenject;

	public interface IUi {}
	
	public interface IUiNavigator
	{
		IObservable<T> OnOpen<T>();
		IObservable<T> OnHide<T>();
		
		void Show<T>( T ui ) where T : IUi;
		void Hide<T>() where T : IUi;
	}
	
	public class UiNavigator : IUiNavigator, IInitializable
	{
		ReactiveDictionary<Type, object> _open = new ();
		
		public void Initialize()
		{
			Show( new CoinsUi() );
		}
		
		public IObservable<T> OnOpen<T>() 
		{
			return _open
				.ObserveAdd()
				.Where( c => c.Key == typeof(T) )
				.Select (c => (T)_open[c.Key] );
		}

		public IObservable<T> OnHide<T>()
		{
			return _open
				.ObserveRemove()
				.Where( c => c.Key == typeof(T) )
				.Select (c => (T)c.Value );
		}

		public void Show<T>(T ui) where T : IUi
		{
			if (_open.ContainsKey(typeof(T)))
				return;
			
			_open.Add( ui.GetType(), ui );
		}

		public void Hide<T>() where T : IUi
		{
			if (!_open.ContainsKey(typeof(T)))
				return;
			
			_open.Remove( typeof(T) );
		}
	}
}