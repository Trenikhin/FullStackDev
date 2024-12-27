namespace Game.UI
{
	using System;
	using Modules.Money;
	using Modules.UI;
	using UniRx;
	using UnityEngine;
	using Zenject;

	public class EarnData
	{
		public Vector3 From;
		public int Delta;
	}
	
	public class CoinsPresenter : IInitializable, IDisposable
	{
		[Inject] ICoinsView _view;
		[Inject] IMoneyStorage _storage;
		
		[Inject] IMessageReceiver _receiver;
		[Inject] ParticleAnimator _particleAnimator;
		
		CompositeDisposable _disposables = new ();
		
		ReactiveProperty<int> _coins = new(); // Cur in 'Storage'
		ReactiveProperty<int> _hidden = new(); // Temp hidden
		
		public void Initialize()
		{
			_storage.OnMoneyChanged += OnCoinsChange;
			_coins.Value = _storage.Money;
			
			// Coins Earned
			_receiver
				.Receive<EarnData>()
				.Subscribe( v => OnCoinsEarn(v.From, v.Delta) )
				.AddTo(_disposables);
			
			// Show Coins
			Observable
				.CombineLatest
				( 
					_coins,
					_hidden,
					( c, h ) => c - h
				)
				.Subscribe( sc => _view.Text = sc.ToString() )
				.AddTo(_disposables);
		}

		public void Dispose()
		{
			_storage.OnMoneyChanged -= OnCoinsChange;
			_disposables?.Dispose();
		}
		
		void OnCoinsEarn(Vector3 from, int amount)
		{
			_hidden.Value = amount;
			
			_particleAnimator.Emit( from, _view.Position, 1, ()
			=>
				_view.Add(amount, c => _hidden.Value = c));
		}
		
		void OnCoinsChange(int newValue, int oldValue)
		{
			_coins.Value = newValue;
		}
	}
}