namespace Input
{
	using System;
	using Modules;
	using Zenject;
	using UnityEngine;

	public interface IInputHandler
	{
		public event Action<Vector2Int> OnDirectionChange;
	}
	
	public class InputHandler : ITickable, IInputHandler
	{
		public event Action<Vector2Int> OnDirectionChange;
		
		public void Tick()
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				OnDirectionChange?.Invoke(Vector2Int.left);
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				OnDirectionChange?.Invoke(Vector2Int.right);
			}
			else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftArrow))
			{
				OnDirectionChange?.Invoke(Vector2Int.up);
			}
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow))
			{
				OnDirectionChange?.Invoke(Vector2Int.down);
			}
		}
	}
}