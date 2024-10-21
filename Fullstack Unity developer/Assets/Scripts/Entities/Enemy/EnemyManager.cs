namespace ShootEmUp
{
    using Entities;
    using System.Collections;
    using UnityEngine;
    using Random = UnityEngine.Random;
    
    public sealed class EnemyManager : EntityManager<Ship>
    {
        [SerializeField] Transform[] _spawnPositions;
        [SerializeField] Transform[] _attackPositions;
        [SerializeField] Ship _player;
        
        IFactory<Ship, EnemyParams> _enemyFactory;
        
        IEnumerator Start()
        {
            _enemyFactory = ServiceLocator.Instance.Get<EnemyFactory>();
            
            const int maxEnemyCount = 5;
            int enemyCount = 0;
            
            // Spawn enemies
            while (enemyCount <= maxEnemyCount)
            {
                yield return new WaitForSeconds(GetInterval());

                Vector2 spawnPosition = RandomPoint(this._spawnPositions).position;
                Vector2 attackPosition = RandomPoint(this._attackPositions).position;

                var prms = new EnemyParams(_player, spawnPosition, attackPosition);
                
                Ship enemy = _enemyFactory.Create( prms );
                OnSpawn( enemy );
                
                enemyCount++;
            }
        }

        void FixedUpdate() => HandleDied();

        
        Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }


        float GetInterval() => Random.Range(_mixInterval, _maxInterval);


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
        
        
        // Conts
        const float _mixInterval = 1;
        const float _maxInterval = 2;
    }
}