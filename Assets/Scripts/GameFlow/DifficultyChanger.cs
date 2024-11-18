namespace Core
{
	using System;
	using CoinManager;
	using Modules;
	using SnakeGame;
	using UnityEngine;
	using Zenject;
	
	public class DifficultyChanger : IInitializable, IDisposable
	{
		IDifficulty _difficulty;
		ICoins _coins;
		
		public DifficultyChanger(IDifficulty difficulty, ICoins coins, IWorldBounds bounds, ISnake snake )
		{
			_difficulty = difficulty;
			_coins = coins;
		}
		
		public void Initialize()
		{
			// Start Game
			NextDifficulty();
			_coins.CountChanged += OnCoinCollected;
		}

		public void Dispose()
		{
			_coins.CountChanged -= OnCoinCollected;
		}

		void OnCoinCollected( int cur )
		{
			if (cur <= 0)
				NextDifficulty();
		}
 
		void NextDifficulty()
		{
			if (_difficulty.Next( out int difficulty ))
				_coins.Spawn( difficulty );
		}
	}
}