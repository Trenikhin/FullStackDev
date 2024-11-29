using Modules.Planets;
using Zenject;
using Game.Obj;

namespace Game.Gameplay
{

    public sealed class PlanetInstaller : Installer<PlanetCatalog, PlanetInstaller>
    {
        [Inject]
        private PlanetCatalog _catalog;

        public override void InstallBindings()
        {
            this.Container
                .BindInterfacesAndSelfTo<PlanetFacade>()
                .FromComponentsInHierarchy()
                .AsSingle();
        }
    }
}