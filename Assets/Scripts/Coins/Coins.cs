namespace CoinManager
{
	using System;
	using System.Collections.Generic;
	using Data;
	using Modules;
	using SnakeGame;
	using UnityEngine;
	using Zenject;

	public interface ICoins
	{
		event Action<int> CountChanged;
		
		void Spawn(int count);
		void Destroy(Vector2Int pos);
		bool TryGet(Vector2Int pos, out Coin coin);
	}
	
	public class Coins : ICoins
	{
		PrefabsConfig _prefabsConfig;
		IWorldBounds _worldBounds;
	
		HashSet<Vector2Int> _coinsPositions = new ();
		Dictionary<Vector2Int, Coin> _coins = new ();
		
		public Coins( PrefabsConfig prefabConfig, IWorldBounds worldBounds )
		{
			_prefabsConfig = prefabConfig;
			_worldBounds = worldBounds;
		}
		
		public event Action<int> CountChanged;
		
		public void Spawn(int count)
		{
			for (int i = 0; i < count; i++)
				CreateNewCoin();
		}

		public bool TryGet( Vector2Int pos, out Coin coin)
		{
			return _coins.TryGetValue(pos, out coin);
		}
		
		public void Destroy( Vector2Int pos )
		{
			var coin = _coins[pos];
			
			_coinsPositions.Remove(pos);
			_coins.Remove(pos);
			GameObject.Destroy( coin.gameObject );
			
			CountChanged?.Invoke(_coins.Count);
		}
		
		void CreateNewCoin()
		{
			var coinPrefab = GameObject.Instantiate(_prefabsConfig.CoinTemplate);

			Vector2Int rand = _worldBounds.GetRandomPosition();
			while (_coinsPositions.Contains(rand))
				rand = _worldBounds.GetRandomPosition();	
			
			coinPrefab.Position = rand;
			_coinsPositions.Add(rand);
			_coins.Add(rand, coinPrefab);
			coinPrefab.Generate();
			
			CountChanged?.Invoke(_coins.Count);
		}
	}
}