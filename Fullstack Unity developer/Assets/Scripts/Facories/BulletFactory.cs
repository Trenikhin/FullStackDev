namespace ShootEmUp
{
	using UnityEngine;

	public struct BulletParams
	{
		public readonly Vector2 Position;
		public readonly Vector2 Velocity;
		public readonly Color Color;
		public readonly int PhysicsLayer;
		public readonly int Damage;

		public BulletParams(int damage, int physicsLayer, Color color, Vector2 velocity, Vector2 position)
		{
			Damage = damage;
			PhysicsLayer = physicsLayer;
			Color = color;
			Velocity = velocity;
			Position = position;
		}
	}
	
	public class BulletFactory : PoolFactory<Bullet>, IFactory<Bullet, BulletParams>
	{
		public Bullet Create( BulletParams @params )
		{
			Bullet bullet = Get();
				
			bullet.Init( @params.Damage, @params.Position, @params.Color, @params.PhysicsLayer, @params.Velocity );

			return bullet;
		}
	}
}