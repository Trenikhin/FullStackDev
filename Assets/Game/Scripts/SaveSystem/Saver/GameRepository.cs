namespace Game.SaveSystem
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Cysharp.Threading.Tasks;
	using Sirenix.OdinInspector.Editor;
	using Sirenix.Serialization;
	using UnityEngine;
	using Zenject;

	public interface IGameRepository
	{
		UniTask<bool> Set(Dictionary<string, string> data, int ver);
		UniTask< (bool isSucceed, Dictionary<string, string> data) > Get( int ver );
	}
	
	public class GameRepository : IGameRepository
	{
		[Inject] ISerializeHelper _serializeHelper;
		
		byte[] _aesSalt = { 0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22 };
		string _pass = "1234";
		
		const string _saveTime = "LastSaveTime";
		const string _dataName = "data";
		
		public async UniTask<bool> Set(Dictionary<string, string> data, int ver)
		{
			// Set save time
			data[_saveTime] = Convert.ToBase64String(SerializationUtility.SerializeValue(DateTime.UtcNow, DataFormat.Binary));;
			
			// Encrypt
			var encryptedData = Serialize(data);
			
			// Save remote
			var res = await Client.Set( encryptedData, ver );
			if (res == false)
				return false;
			
			// Save local
			PlayerPrefs.SetString( GetLocalPath(ver), encryptedData );
			return true;
		}
		
		public async UniTask< (bool, Dictionary<string, string> ) > Get( int ver )
		{
			// Get remote
			var remoteResponse = await Client.Get(ver );
			
			if(!remoteResponse.isSucceed)
				return (false, null);
			
			var remoteData = Deserialize(remoteResponse.json);
			
			// Get local
			var localJson = PlayerPrefs.GetString( GetLocalPath(ver) );
			var localData = Deserialize(localJson);

			var res = GetSaveTime(localData) > GetSaveTime(remoteData) ? localData : remoteData;

			return (true, res);
		}

		string Serialize( Dictionary<string, string> data )
		{
			string serializedData = _serializeHelper.Serialize(data);
			string encryptedData =  AESEncryptor.Encrypt(serializedData, _aesSalt, _pass);
			
			return encryptedData;
		}
		
		Dictionary<string, string> Deserialize( string json )
		{
			string decryptedData = AESEncryptor.Decrypt(json, _aesSalt, _pass);
			var saveData = _serializeHelper.Deserialize<Dictionary<string, string>>(decryptedData);
			
			return saveData;
		}
		
		long GetSaveTime(Dictionary<string, string> data)
		{
			return !data.TryGetValue(_saveTime, out string v)
				? default
				: _serializeHelper.Deserialize<DateTime>(v).Ticks;
		}

		static string GetLocalPath( int ver ) => $"{_dataName}{ver}";
	}
}