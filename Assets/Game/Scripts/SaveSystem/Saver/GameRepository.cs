namespace Game.SaveSystem
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Cysharp.Threading.Tasks;
	using Sirenix.OdinInspector.Editor;
	using Sirenix.Serialization;
	using UnityEngine;

	public interface IGameRepository
	{
		UniTask Set(Dictionary<string, string> data, int ver);
		UniTask<Dictionary<string, string>> Get( int ver );
	}
	
	public class GameRepository : IGameRepository
	{
		byte[] _aesSalt = { 0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22 };
		string _pass = "1234";
		
		const string _saveTime = "LastSaveTime";
		const string _dataName = "data";
		
		public async UniTask Set(Dictionary<string, string> data, int ver)
		{
			// Set save time
			data[_saveTime] = Convert.ToBase64String(SerializationUtility.SerializeValue(DateTime.UtcNow, DataFormat.Binary));;
			
			// Encrypt
			var encryptedData = Serialize(data);
			
			// Save remote
			//await Client.Set( encryptedData, ver );
			
			// Save local
			PlayerPrefs.SetString( GetLocalPath(ver), encryptedData );
		}
		
		public async UniTask<Dictionary<string, string>> Get( int ver )
		{
			// Get remote
			//var remoteResponse = await Client.Get(ver );
			//
			//if (remoteResponse.isCucceed)
			//	return Deserialize(remoteResponse.json);
			
			// Get local
			var localJson = PlayerPrefs.GetString( GetLocalPath(ver) );
			var localData = Deserialize(localJson);
			
			return localData;
		}

		string Serialize( Dictionary<string, string> data )
		{
			string serializedData = Convert.ToBase64String(SerializationUtility.SerializeValue(data, DataFormat.Binary));
			string encryptedData =  AESEncryptor.Encrypt(serializedData, _aesSalt, _pass);
			
			return encryptedData;
		}
		
		Dictionary<string, string> Deserialize( string json )
		{
			string decryptedData = AESEncryptor.Decrypt(json, _aesSalt, _pass);
			var bytes = Convert.FromBase64String(decryptedData);
			var saveData = SerializationUtility.DeserializeValue<Dictionary<string, string>>(
				bytes, DataFormat.Binary);
			
			return saveData;
		}

		string GetLocalPath( int ver )
		{
			return $"{_dataName}{ver}";
		}
	}
}