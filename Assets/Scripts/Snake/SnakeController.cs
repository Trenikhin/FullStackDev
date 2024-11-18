namespace Input
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

		void OnDirectionChanged(Vector2Int direction) => _snake.Turn(direction.ToEnum());
		void OnCoinTouched( Coin coin ) => _snake.Expand( coin.Bones );
		void OnDifficultyChanged() => _snake.SetSpeed(_difficulty.Current);
	}
	
	public static class DirectionHelper
	{
		public static SnakeDirection ToEnum(this Vector2Int vector)
		{
			return vector switch
			{
				var v when v == Vector2Int.up => SnakeDirection.UP,
				var v when v == Vector2Int.down => SnakeDirection.DOWN,
				var v when v == Vector2Int.left => SnakeDirection.LEFT,
				var v when v == Vector2Int.right => SnakeDirection.RIGHT,
				_ => SnakeDirection.NONE
			};
		}
	}
}