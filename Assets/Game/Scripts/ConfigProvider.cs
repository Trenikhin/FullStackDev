namespace Game.Scripts
{
	using UnityEngine;

	public class ConfigProvider : MonoBehaviour
	{
		public static ConfigProvider Instance => _instance ??= new ();
		
		static ConfigProvider _instance;

		// Core
		public ConvertConfig ConvertConfig;
		public ObjsConfig ObjsConfig;
		
		// Objs
		public ObjConfig Wood;
		public ObjConfig Plank;
	}
}