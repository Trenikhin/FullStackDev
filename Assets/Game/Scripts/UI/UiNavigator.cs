namespace Game.UI
{
	using System;
	using System.Collections.Generic;
	using UniRx;

	public enum EUi
	{
		None = 0,
		
		Screen = 1,
		Popup = 2,
	}
	
	public interface IUi
	{
		EUi Ui { get; }
	}
	
	public interface IUiNavigator
	{
		IObservable<T> OnOpen<T>();
		IObservable<T> OnClose<T>();
		
		void Show<T>( T ui ) where T : IUi;
		void Hide<T>() where T : IUi;
	}
	
	public class UiNavigator : IUiNavigator
	{
		Dictionary<Type, object> _open = new ();
		
		ReactiveCommand<Type> _onOpen = new();
		ReactiveCommand<Type> _onClose = new();
		
		public IObservable<T> OnOpen<T>() 
		{
			return _onOpen.Select (c => (T)_open[c] );
		}

		public IObservable<T> OnClose<T>()
		{
			return _onClose.Select ( c => (T)_open[c] );
		}

		public void Show<T>(T ui) where T : IUi
		{
			_open.Add( ui.GetType(), ui );
			_onOpen.Execute(ui.GetType());
		}

		public void Hide<T>() where T : IUi
		{
			if (!_open.ContainsKey(typeof(T)))
				return;

			_onClose.Execute(typeof(T));
			_open.Remove( typeof(T) );
		}
	}
}