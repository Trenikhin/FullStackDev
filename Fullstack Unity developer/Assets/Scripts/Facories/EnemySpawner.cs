namespace ShootEmUp
{
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;
    
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Transform[] spawnPositions;
        [SerializeField] Transform[] attackPositions;
        [SerializeField] Ship        character;
        [SerializeField] EnemyPool   _enemyPool;

        IEnumerator Start()
        {
            const int maxEnemyCount = 5;
            int       enemyCount = 0;
            
            // Spawn enemies
            while (enemyCount <= maxEnemyCount)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                Ship ship = _enemyPool.Rent();
                SetupEnemy( ship );

                enemyCount++;
            }
        }

        void FixedUpdate() => HandleDied();

        
        void SetupEnemy( Ship ship )
        {
            Transform spawnPosition = this.RandomPoint(this.spawnPositions);
            ship.transform.position = spawnPosition.position;

            Transform attackPosition = this.RandomPoint(this.attackPositions);
            
            ship.GetComponent< EnemyBrain >().Init( character, attackPosition.position );
        }

        
        Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }


        void HandleDied()
        {
            foreach (Ship enemy in _enemyPool.ActiveObjs.ToArray()) // collection could be modified
            {
                if (enemy.Health.Value <= 0)
                {
                    _enemyPool.Return( enemy );
                }
            }
        }
    }
}