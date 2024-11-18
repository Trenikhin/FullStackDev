namespace Snake
{
	using System;
	using System.Collections.Generic;
	using Data;
	using Modules;
	using Zenject;
	using UnityEngine;

	public interface IInputHandler
	{
		event Action<SnakeDirection> OnDirectionChange;
	}
	
	public class InputHandler : ITickable, IInputHandler
	{
		InputMapConfig _cfg;
		
		public InputHandler( InputMapConfig inputMap )
		{
			_cfg = inputMap;
		}
		
		public event Action<SnakeDirection> OnDirectionChange;
		
		public void Tick()
		{
			foreach (var kb in _cfg.KeyBindings)
			{
				if (IsKeyPressed(kb.Keys))
					OnDirectionChange?.Invoke(kb.Direction);
			}
		}
		
		bool IsKeyPressed(List<KeyCode> keys)
		{
			foreach (var key in keys)
				if (Input.GetKeyDown(key))
					return true;
			
			return false;
		}
	}
}