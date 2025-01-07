using Zenject;

namespace Game.UI
{
    public sealed class BackgroundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container
                .Bind<BackgroundView>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}