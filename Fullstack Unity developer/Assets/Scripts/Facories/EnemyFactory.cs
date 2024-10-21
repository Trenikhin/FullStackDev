namespace ShootEmUp
{
	using UnityEngine;
	
	public struct EnemyParams
	{
		public readonly Vector2 Pos;
		public readonly Vector2 AttackPos;
		public readonly Ship Enemy;

		public EnemyParams(Ship enemy, Vector2 pos, Vector2 attackPos)
		{
			Enemy = enemy;
			Pos = pos;
			AttackPos = attackPos;
		}
	}
	
	public class EnemyFactory : PoolFactory<Ship, EnemyParams>
	{
		public override Ship Create( EnemyParams @params )
		{
			Ship ship = Get();
			SetupEnemy(ship, @params.Pos, @params.AttackPos, @params.Enemy );

			return ship;
		}
		
		void SetupEnemy( Ship ship, Vector2 pos, Vector2 attackPos, Ship enemy )
		{
			ship.transform.position = pos;
			ship.GetComponent< EnemyBrain >().Init( enemy, attackPos );
		}
	}
}