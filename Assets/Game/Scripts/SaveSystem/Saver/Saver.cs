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
		
		UniTask SaveAsync();
		UniTask LoadAsync( int ver );
	}
	
	public class Saver : ISaver
	{
		[Inject] ISerializer[] _serializers;
		[Inject] IGameRepository _gameRepository;
		
		const string _ver = "version";
		
		public int LastSaveVersion => PlayerPrefs.GetInt(_ver);
		
		public async UniTask SaveAsync()
		{
			var data = new Dictionary<string, string>();
			
			_serializers.ForEach(s => s.Serialize(data));
			
			// Set cur save ver
			PlayerPrefs.SetInt(_ver, PlayerPrefs.GetInt(_ver) + 1);
			
			await _gameRepository.Set( data, LastSaveVersion );
		}

		public async UniTask LoadAsync( int ver )
		{
			var data = await _gameRepository.Get( ver );
			
			_serializers.ForEach(s => s.Deserialize(data));
		}
	}
}