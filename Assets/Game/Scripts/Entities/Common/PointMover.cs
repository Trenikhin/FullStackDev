namespace Game.Components
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using Cysharp.Threading.Tasks;
	using DG.Tweening;
	using UnityEngine;
	using Zenject;

	public class PointMover : IInitializable, IDisposable
	{
		List<Vector3> _points;
		float _speed;
		readonly Transform _platform;
		
		CancellationTokenSource _cts = new ();

		public PointMover( Transform platform, List<Vector3> points, float speed )
		{
			_platform = platform;
			_points = points;
			_speed = speed;
		}
		
		public void Initialize()
		{
			RunMover( _cts.Token ).Forget();
		}
		
		public void Dispose()
		{
			_cts.Cancel();
			_cts.Dispose();
		}
		
		async UniTaskVoid RunMover( CancellationToken ct )
		{
			while (true)
			{
				foreach (var p in _points)
				{
					await MoveToTarget(p, ct);
				}
			}
		}
		
		async UniTask MoveToTarget(Vector3 target, CancellationToken ct)
		{
			var distance = Vector2.Distance(_platform.position, target);
			var dur = distance / _speed;
			
			await _platform
				.DOMove(target, dur)
				.SetEase(Ease.Linear)
				.ToUniTask( cancellationToken: ct );
		}
	}
}