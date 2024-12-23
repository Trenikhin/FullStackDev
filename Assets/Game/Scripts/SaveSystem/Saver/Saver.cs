namespace Game.SaveSystem
{
	using System.Collections.Generic;
	using Cysharp.Threading.Tasks;
	using Sirenix.Utilities;
	using UnityEngine;
	using Zenject;

	public interface ISaver
	{
		int LastSaveVersion { get; }
		
		UniTask<bool> SaveAsync();
		UniTask<bool> LoadAsync( int ver );
	}
	
	public class Saver : ISaver
	{
		[Inject] ISerializer[] _serializers;
		[Inject] IGameRepository _gameRepository;
		
		const string _ver = "version";
		
		public int LastSaveVersion => PlayerPrefs.GetInt(_ver);
		
		public async UniTask<bool> SaveAsync()
		{
			var data = new Dictionary<string, string>();
			
			_serializers.ForEach(s => s.Serialize(data));
			
			// Set cur save ver
			PlayerPrefs.SetInt(_ver, PlayerPrefs.GetInt(_ver) + 1);
			
			return await _gameRepository.Set( data, LastSaveVersion );
		}

		public async UniTask<bool> LoadAsync( int ver )
		{
			if (ver > LastSaveVersion)
				return false;
				
			var data = await _gameRepository.Get( ver );

			if (!data.isSucceed)
				return false;
			
			_serializers.ForEach(s => s.Deserialize(data.data));
			return true;
		}
	}
}