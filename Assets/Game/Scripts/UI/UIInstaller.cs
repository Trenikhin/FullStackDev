using Zenject;

namespace Game.Gameplay
{
    using SaveSystem;

    //Don't modify
    public sealed class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<ControlsView>().FromComponentInHierarchy().AsSingle();
            this.Container.BindInterfacesTo<ControlsPresenter>().AsSingle();
        }
    }
}