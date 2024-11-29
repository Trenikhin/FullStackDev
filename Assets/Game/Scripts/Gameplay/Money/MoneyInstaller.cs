using Modules.Money;
using Zenject;

namespace Game.Gameplay
{
    using UI;

    public sealed class MoneyInstaller : Installer<int, MoneyInstaller>
    {
        [Inject]
        private int _initialMoney;
        
        public override void InstallBindings()
        {
            this.Container
                .BindInterfacesAndSelfTo<MoneyStorage>()
                .AsSingle()
                .WithArguments(_initialMoney)
                .NonLazy();

            this.Container
                .BindInterfacesTo<MoneyAdapter>()
                .AsSingle()
                .NonLazy();
            
            this.Container
                .BindInterfacesTo<Coins>()
                .AsSingle()
                .NonLazy();
            
            this.Container
                .BindInterfacesTo<CoinsPresenter>()
                .AsSingle()
                .NonLazy();
            
            this.Container
                .BindInterfacesTo<CoinsView>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}