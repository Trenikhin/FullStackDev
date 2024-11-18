namespace Core
{
	using System;
	using Coins;
	using Modules;
	using SnakeGame;
	using UnityEngine;
	using Zenject;

	public interface IGameOverHandler
	{
		event Action<bool> OnGameOver;
	}
	
	public class GameOverHandler : IInitializable, IDisposable, IGameOverHandler
	{
		IDifficulty _difficulty;
		IWorldBounds _bounds;
		ICoins _coins;
		ISnake _snake;
		
		public GameOverHandler( IDifficulty difficulty, ICoins coins, IWorldBounds bounds, ISnake snake )
		{
			_difficulty = difficulty;
			_coins = coins;
			_snake = snake;
			_bounds = bounds;
		}
		
		public void Initialize()
		{
			_snake.OnMoved += OnMoved;
			_snake.OnSelfCollided += LoseGame;
			_coins.CountChanged += OnCoinsCollected;
		}

		public void Dispose()
		{
			_snake.OnMoved -= OnMoved;
			_snake.OnSelfCollided -= LoseGame;
			_coins.CountChanged -= OnCoinsCollected;

		}
		
		public event Action<bool> OnGameOver;

		void OnMoved( Vector2Int head )
		{
			if (!_bounds.IsInBounds(head))
				LoseGame();
		}

		void OnCoinsCollected( int cur )
		{
			if (_difficulty.Current >= _difficulty.Max)
				OnGameOver?.Invoke( true );
		}
		
		void LoseGame() => OnGameOver?.Invoke( false );
	}
}