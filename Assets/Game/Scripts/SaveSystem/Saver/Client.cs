namespace Game.SaveSystem
{
	using System;
	using System.Collections.Generic;
	using Cysharp.Threading.Tasks;
	using Sirenix.Serialization;
	using UnityEngine;
	using UnityEngine.Networking;
	using Zenject;

	public static class Client
	{
		public static string Uri = "http://127.0.0.1:8888";
		
		public static async UniTask<string> Get()
		{
			// Get remote data
			UnityWebRequest request = UnityWebRequest.Get($"{Uri}/load?version=1");
			await request.SendWebRequest();
			string json = request.downloadHandler.text;
			//var bytes = Convert.FromBase64String(json);
			//var saveData = SerializationUtility.DeserializeValue<Dictionary<string, string>>(bytes, DataFormat.Binary);
			return  json;
		}
		
		public static async UniTask Set(string json)
		{
			// Serialize
			//var json = Convert.ToBase64String(SerializationUtility.SerializeValue(data, DataFormat.Binary));
			
			// Send
			UnityWebRequest request = UnityWebRequest.Put($"{Uri}/save?version=1", json);
			await request.SendWebRequest();
		}
	}
}