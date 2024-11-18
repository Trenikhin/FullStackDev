namespace Snake
{
	using System;
	using Modules;
	using UnityEngine;
	using Zenject;

	public class SnakeController: IInitializable, IDisposable
	{
		ISnake _snake;
		IInputHandler _inputHandler;
		ICoinTrigger _coinTrigger;
		IDifficulty _difficulty;
		
		public SnakeController( ISnake snake, IInputHandler inputHandler, ICoinTrigger coinTrigger, IDifficulty difficulty )
		{
			_snake = snake;
			_inputHandler = inputHandler;
			_coinTrigger = coinTrigger;
			_difficulty = difficulty;
		}
		
		public void Initialize()
		{
			_inputHandler.OnDirectionChange += OnDirectionChanged;
			_coinTrigger.OnCoinTouch += OnCoinTouched;
			_difficulty.OnStateChanged += OnDifficultyChanged;
		}

		public void Dispose()
		{
			_inputHandler.OnDirectionChange -= OnDirectionChanged;
			_coinTrigger.OnCoinTouch -= OnCoinTouched;
			_difficulty.OnStateChanged -= OnDifficultyChanged;
		}

		void OnDirectionChanged(SnakeDirection direction) => _snake.Turn(direction);
		void OnCoinTouched( Coin coin ) => _snake.Expand( coin.Bones );
		void OnDifficultyChanged() => _snake.SetSpeed(_difficulty.Current);
	}
}