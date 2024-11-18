namespace Input
{
	using System;
	using CoinManager;
	using Modules;
	using UnityEngine;
	using Zenject;

	public class SnakeController: IInitializable, IDisposable
	{
		ISnake _snake;
		IInputHandler _inputHandler;
		ICoins _coins;
		IScore _score;
		IDifficulty _difficulty;
		
		public SnakeController( ISnake snake, IInputHandler inputHandler, ICoins coins, IScore score, IDifficulty difficulty )
		{
			_snake = snake;
			_inputHandler = inputHandler;
			_coins = coins;
			_score = score;
			_difficulty = difficulty;
		}
		
		public void Initialize()
		{
			_inputHandler.OnDirectionChange += OnDirectionChanged;
			_snake.OnMoved += OnMove;
			_difficulty.OnStateChanged += OnDifficultyChanged;
		}

		public void Dispose()
		{
			_inputHandler.OnDirectionChange -= OnDirectionChanged;
			_snake.OnMoved -= OnMove;
			_difficulty.OnStateChanged -= OnDifficultyChanged;
		}

		void OnDirectionChanged(Vector2Int direction)
		{
			_snake.Turn(direction.ToEnum());
		}

		void OnMove( Vector2Int head )
		{
			if (_coins.TryGet(head, out Coin coin))
			{
				_snake.Expand( coin.Bones );
				_score.Add( coin.Score );
				_coins.Destroy( head );
			}
		}
		
		void OnDifficultyChanged()
		{
			_snake.SetSpeed(_difficulty.Current);
		}
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