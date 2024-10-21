namespace ShootEmUp
{
    using System.Collections;
    using UnityEngine;
    
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyManager _enemyManager;
        
        [SerializeField] int _maxEnemyCount = 5;
        [SerializeField] float _mixInterval = 1;
        [SerializeField] float _maxInterval = 2;
        
        IEnumerator Start()
        {
            int enemyCount = 0;
            
            // Spawn enemies
            while (enemyCount <= _maxEnemyCount)
            {
                yield return new WaitForSeconds(GetInterval());

               _enemyManager.SpawnEnemy();
                
                enemyCount++;
            }
        }
        
        float GetInterval() => Random.Range(_mixInterval, _maxInterval);
    }
}