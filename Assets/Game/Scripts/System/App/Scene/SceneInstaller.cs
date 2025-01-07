using UnityEngine;
using Zenject;

namespace Game.App
{
    [CreateAssetMenu(
        fileName = "SceneInstaller",
        menuName = "Zenject/App/New SceneInstaller"
    )]
    public sealed class SceneInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<SceneNavigator>().AsSingle();
        }
    }
}