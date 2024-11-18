namespace Snake
{
	using System;
	using Coins;
	using Modules;
	using UnityEngine;
	using Zenject;

	public interface ICoinTrigger
	{
		event Action<Coin> OnCoinTouch;
	}
	
	public class CoinCollector: IInitializable, IDisposable, ICoinTrigger
	{
		ICoins _coins;
		ISnake _snake;
		
		public CoinCollector( ICoins coins, ISnake snake )
		{
			_coins = coins;
			_snake = snake;
		}
		
		public void Initialize() => _snake.OnMoved += OnMove;
		public void Dispose() => _snake.OnMoved -= OnMove;
		
		public event Action<Coin> OnCoinTouch;
		
		void OnMove(Vector2Int headPos)
		{
			if (_coins.TryGet(headPos, out Coin coin))
			{
				_coins.Destroy( headPos );
				OnCoinTouch?.Invoke( coin );
			}
		}
	}
}