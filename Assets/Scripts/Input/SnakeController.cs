namespace DefaultNamespace.Input
{
	using System;
	using Modules;
	using UnityEngine;
	using Zenject;

	public class SnakeController: IInitializable, IDisposable
	{
		ISnake _snake;
		IInputHandler _inputHandler;

		public SnakeController( ISnake snake, IInputHandler inputHandler )
		{
			_snake = snake;
			_inputHandler = inputHandler;
		}
		
		public void Initialize()
		{
			_inputHandler.OnDirectionChange += OnDirectionChanged;
		}

		public void Dispose()
		{
			_inputHandler.OnDirectionChange -= OnDirectionChanged;
		}

		void OnDirectionChanged(Vector2Int direction) => _snake.Turn(direction.ToEnum());
	}
	
	public static class DirectionHelper
	{
		public static SnakeDirection ToEnum(this Vector2Int vector)
		{
			if (vector == Vector2Int.up)
				return SnakeDirection.UP;
			else if (vector == Vector2Int.down)
				return SnakeDirection.DOWN;
			else if (vector == Vector2Int.left)
				return SnakeDirection.LEFT;
			else if (vector == Vector2Int.right)
				return SnakeDirection.RIGHT;
			
			return SnakeDirection.NONE;
		}
	}
}