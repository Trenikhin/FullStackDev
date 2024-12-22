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
		UniTask Set(Dictionary<string, string> data);
		UniTask<Dictionary<string, string>> Get();
	}
	
	public class GameRepository : IGameRepository
	{
		byte[] _aesSalt = { 0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22 };
		string _pass = "1234";
		
		const string _lastSaveTime = "LastSaveTime";
		const string _dataName = "data";
		
		public async UniTask Set(Dictionary<string, string> data)
		{
			// Set save time
			data[_lastSaveTime] = Convert.ToBase64String(SerializationUtility.SerializeValue(DateTime.UtcNow, DataFormat.Binary));;
			
			// Encrypt
			string serializedData = Convert.ToBase64String(SerializationUtility.SerializeValue(data, DataFormat.Binary));
			string encryptedData =  AESEncryptor.Encrypt(serializedData, _aesSalt, _pass);
			
			// Save remote
			await Client.Set( encryptedData );
			
			// Save local
			PlayerPrefs.SetString( _dataName, encryptedData );
		}
		
		public async UniTask<Dictionary<string, string>> Get()
		{
			// Get remote
			var remoteJson = await Client.Get();
			var removeData = Deserialize(remoteJson);
			
			// Get local
			var localJson = PlayerPrefs.GetString(_dataName);
			var localData = Deserialize(localJson);
			
			return GetSaveTime(removeData) > GetSaveTime(localData) ? removeData : localData;
		}
		
		long GetSaveTime( Dictionary<string, string> data )
		{
			var bytes = Convert.FromBase64String( data[_lastSaveTime] );
			var dateTime = SerializationUtility.DeserializeValue<DateTime>(bytes, DataFormat.Binary);
			
			return dateTime.Ticks;
		}

		Dictionary<string, string> Deserialize( string json )
		{
			string decryptedData = AESEncryptor.Decrypt(json, _aesSalt, _pass);
			var bytes = Convert.FromBase64String(decryptedData);
			var saveData = SerializationUtility.DeserializeValue<Dictionary<string, string>>(bytes, DataFormat.Binary);

			return saveData;
		}
	}
}