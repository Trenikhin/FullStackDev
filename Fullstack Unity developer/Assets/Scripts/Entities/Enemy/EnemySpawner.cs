namespace ShootEmUp
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;
    
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Transform[] spawnPositions;
        [SerializeField] Transform[] attackPositions;
        [SerializeField] Player      character;
        [SerializeField] EnemyPool   _enemyPool;

        IEnumerator Start()
        {
            // Spawn enemies
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                Enemy enemy = _enemyPool.Rent();
                SetupEnemy( enemy );
            }
        }

        void FixedUpdate()
        {
            // Handle Enemy Die
            foreach (Enemy enemy in _enemyPool.ActiveObjs.ToArray())
            {
                if (enemy.Health <= 0)
                {
                    _enemyPool.Return( enemy );
                }
            }
        }

        
        void SetupEnemy( Enemy enemy )
        {
            Transform spawnPosition = this.RandomPoint(this.spawnPositions);
            enemy.transform.position = spawnPosition.position;

            Transform attackPosition = this.RandomPoint(this.attackPositions);
               
            enemy.Init( character, attackPosition.position );
        }

        
        Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}