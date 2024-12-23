namespace Game.SaveSystem
{
	using Cysharp.Threading.Tasks;
	using UnityEngine.Networking;

	public static class Client
	{
		const string _uri = "http://127.0.0.1:8888";

		public static async UniTask<(bool isSucceed, string json)> Get( int ver )
		{
			// Get remote data
			var request = UnityWebRequest.Get($"{_uri}/load?version={ver}");
			
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
		
		public static async UniTask<bool> Set(string json, int ver)
		{
			// Send
			var request = UnityWebRequest.Put($"{_uri}/save?version={ver}", json);
			
			try
			{
				await request.SendWebRequest();
				return true;
			}
			catch
			{
				await UniTask.CompletedTask;
				return false;
			}
		}
	}
}