namespace Installers
{
	using Data;
	using UnityEngine;
	using Zenject;

	public class ConfigInstaller : MonoInstaller
	{
		[SerializeField] PrefabsConfig _prefabsConfig;
		[SerializeField] InputMapConfig _inputConfig;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_prefabsConfig);
			Container.BindInstance(_inputConfig);
		}
	}
}