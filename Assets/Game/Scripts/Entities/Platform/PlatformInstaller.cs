namespace Game.Entities
{
	using System.Collections.Generic;
	using Components;
	using UnityEngine;

	public class PlatformInstaller : EntityInstaller
	{
		[SerializeField] Transform _platform;
		[SerializeField] Transform _point1;
		[SerializeField] Transform _point2;
		
		[SerializeField] float _speed = 5;
		
		public override void InstallBindings()
		{
			base.InstallBindings();

			Container
				.BindInterfacesTo<PointMover>()
				.AsSingle()
				.WithArguments( _platform, new List<Vector3>() {_point1.position, _point2.position}, _speed);
			
			Container
				.Bind<PlatformTag>()
				.AsSingle();
		}
	}
}