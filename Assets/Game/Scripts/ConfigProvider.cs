namespace Game.Scripts
{
	using UnityEngine;

	public class ConfigProvider : MonoBehaviour
	{
		public static ConfigProvider Instance => _instance ??= new ();
		
		static ConfigProvider _instance;

		// Core
		public ConvertConfig ConvertConfig = new ConvertConfig();
		public ObjsConfig ObjsConfig = new ObjsConfig();
		
		// Objs
		public ObjConfig Wood = new ObjConfig()
		{
			Name = "Wood",
		};
		public ObjConfig Plank = new ObjConfig()
		{
			Name = "Plank",
		};
	}
}