namespace ShootEmUp
{
    using System.Collections;
    using UnityEngine;
    
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyManager _enemyManager;
        
        IEnumerator Start()
        {
            const int maxEnemyCount = 5;
            int enemyCount = 0;
            
            // Spawn enemies
            while (enemyCount <= maxEnemyCount)
            {
                yield return new WaitForSeconds(GetInterval());

               _enemyManager.SpawnEnemy();
                
                enemyCount++;
            }
        }
        
        float GetInterval() => Random.Range(_mixInterval, _maxInterval);
        
        // Conts
        const float _mixInterval = 1;
        const float _maxInterval = 2;
    }
}