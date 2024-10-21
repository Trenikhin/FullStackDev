namespace ShootEmUp
{
    using UnityEngine;
    using Random = UnityEngine.Random;
    
    public sealed class EnemyManager : EntityManager<Ship, EnemyParams>
    {
        [SerializeField] Transform[] _spawnPositions;
        [SerializeField] Transform[] _attackPositions;
        [SerializeField] Ship _player;

        void FixedUpdate() => HandleDied();

        public void SpawnEnemy()
        {
            Vector2 spawnPosition = RandomPoint(this._spawnPositions).position;
            Vector2 attackPosition = RandomPoint(this._attackPositions).position;

            var prms = new EnemyParams(_player, spawnPosition, attackPosition);

            Spawn(prms);
        }
        
        Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }
        
        void HandleDied()
        {
            foreach (Ship enemy in _activeObjs.ToArray()) // collection could be modified
            {
                if (enemy.Health.Value <= 0)
                {
                    Destroy( enemy );
                }
            }
        }
    }
}