namespace Installers
{
	using Data;
	using UnityEngine;
	using Zenject;

	public class ConfigInstaller : MonoInstaller
	{
		[SerializeField] PrefabsConfig _prefabsConfig;

		public override void InstallBindings()
		{
			Container.BindInstance(_prefabsConfig);
		}
	}
}