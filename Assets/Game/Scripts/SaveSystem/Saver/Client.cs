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
		
		public static async UniTask< (bool isCucceed, string json) > Get( int ver )
		{
			// Get remote data
			UnityWebRequest request = UnityWebRequest.Get($"{Uri}/load?version={ver}");
			
			try
			{
				await request.SendWebRequest();
				if (request.result != UnityWebRequest.Result.Success)
					return (false, null);
			}
			catch
			{
				return (false, null);
			}

			string json = request.downloadHandler.text;
			return json == null ? (false, null) : (true, json);
		}
		
		public static async UniTask Set(string json, int ver)
		{
			// Send
			UnityWebRequest request = UnityWebRequest.Put($"{Uri}/save?version={ver}", json);
			
			try
			{
				await request.SendWebRequest();
			}
			catch
			{
				await UniTask.CompletedTask;
			}
		}
	}
}