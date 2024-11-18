namespace Coins
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
		CoinPool _pool;
		IWorldBounds _worldBounds;
		
		Dictionary<Vector2Int, Coin> _coins = new ();
		
		public Coins(  IWorldBounds worldBounds, CoinPool pool )
		{
			_pool = pool;
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
			_coins.Remove(pos);
			coin.gameObject.SetActive(false);
			_pool.Despawn(coin);
			
			CountChanged?.Invoke(_coins.Count);
		}
		
		void CreateNewCoin()
		{
			var coin = _pool.Spawn();

			Vector2Int rand = _worldBounds.GetRandomPosition();
			while (_coins.ContainsKey(rand))
				rand = _worldBounds.GetRandomPosition();	
			
			coin.gameObject.SetActive(true);
			coin.Position = rand;
			coin.Generate();
			
			_coins.Add(rand, coin);
			
			CountChanged?.Invoke(_coins.Count);
		}

		public class CoinPool : MonoMemoryPool<Coin> { }
	}
}