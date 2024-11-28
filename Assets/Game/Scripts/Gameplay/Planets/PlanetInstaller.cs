using Modules.Planets;
using Zenject;

namespace Game.Gameplay
{
    public sealed class PlanetInstaller : Installer<PlanetCatalog, PlanetInstaller>
    {
        [Inject]
        private PlanetCatalog _catalog;

        public override void InstallBindings()
        {
            foreach (PlanetConfig config in _catalog)
            {
                this.Container
                    .BindInterfacesAndSelfTo<Planet>()
                    .AsCached()
                    .WithArguments(config)
                    .NonLazy();
            }
        }
    }
}