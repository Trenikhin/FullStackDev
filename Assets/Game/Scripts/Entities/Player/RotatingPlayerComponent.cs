namespace Game.Components
{
	using System;
	using Unity.Mathematics;
	using UnityEngine;
	using Zenject;

	public class RotatingPlayerComponent : ITickable
	{
		Transform _transform;
		
		public RotatingPlayerComponent( Transform parentTransform )
		{
			_transform = parentTransform;
		}
		
		public void Tick()
		{
			var direction = Input.GetAxis("Horizontal");
			
			if ( math.abs(direction) > 0 )
				_transform.localScale = direction < 0 ? new Vector3(-1, 1, 1) : new Vector3( 1 , 1, 1);
		}
	}
}