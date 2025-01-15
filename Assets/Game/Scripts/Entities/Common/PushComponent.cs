namespace Game.Components
{
	using System;
	using System.Collections.Generic;
	using Entities;
	using UnityEngine;
	using Zenject;

	public class PushComponent : ITriggerEnter2D, ITriggerExit2D, ITickable
	{
		Func<bool> _pushCondition;
		Vector3 _windDirection;
		float _force;

		List<Rigidbody2D> _rigidbodies = new ();
		
		public PushComponent( Vector3 windDirection, float force )
		{
			_windDirection = windDirection;
			_force = force;
		}

		public void OnTriggerEnter2D(Entity e)
		{
			if (e.TryGet(out Rigidbody2D rb))
			{
				_rigidbodies.Add( rb );
			}
		}

		public void OnTriggerExit2D(Entity e)
		{
			if (e.TryGet(out Rigidbody2D rb))
			{
				_rigidbodies.Remove( rb );
			}
		}
		
		public void AddPushCondition( Func<bool> pushCondition )
		{
			_pushCondition = pushCondition;
		}
		
		public void Tick()
		{
			if (_pushCondition == null)
				return;
			
			if (!_pushCondition())
				return;
			
			foreach (var rb in _rigidbodies)
			{
				rb.AddForce(_windDirection * _force);
			}
		}
	}
}