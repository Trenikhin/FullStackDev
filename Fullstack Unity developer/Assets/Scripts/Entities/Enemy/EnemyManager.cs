namespace ShootEmUp
{
    using UnityEngine;
    using Random = UnityEngine.Random;
    
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] Transform[] _spawnPositions;
        [SerializeField] Transform[] _attackPositions;
        [SerializeField] Ship _player;
        [SerializeField] EnemyPool _pool;
        
        ActivePool<Ship> _activePool;
        
        void Awake() => _activePool = new ActivePool<Ship>(_pool);

        void FixedUpdate() => _activePool.DoWithFiltered(isDead, _activePool.Return);

        public void SpawnEnemy()
        {
            Vector2 spawnPosition = RandomPoint(_spawnPositions).position;
            Vector2 attackPosition = RandomPoint(_attackPositions).position;

            var ship = _activePool.Rent();
            
            ship.transform.position = spawnPosition;
            ship.GetComponent< EnemyBrain >().Init( _player, attackPosition );
        }
        
        Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }

        bool isDead(Ship ship) => ship.Health.Value <= 0;
    }
}