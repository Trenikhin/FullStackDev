namespace ShootEmUp
{
    using Entities;
    using UnityEngine;
    using Random = UnityEngine.Random;
    
    public sealed class EnemyManager : EntityManager<Ship>
    {
        [SerializeField] Transform[] _spawnPositions;
        [SerializeField] Transform[] _attackPositions;
        [SerializeField] Ship _player;

        IFactory<Ship, EnemyParams> _cache;
        
        IFactory<Ship, EnemyParams> _enemyFactory
        {
            get
            {
                if (_cache != null)
                    return _cache;
                return _cache = ServiceLocator.Instance.Get<EnemyFactory>();
            }
        }

        void FixedUpdate() => HandleDied();

        public void SpawnEnemy()
        {
            Vector2 spawnPosition = RandomPoint(this._spawnPositions).position;
            Vector2 attackPosition = RandomPoint(this._attackPositions).position;

            var prms = new EnemyParams(_player, spawnPosition, attackPosition);
            
            Ship enemy = _enemyFactory.Create( prms );
            OnSpawn( enemy );
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
                    _enemyFactory.Destroy( enemy );
                    OnObjDestroy( enemy );
                }
            }
        }
    }
}