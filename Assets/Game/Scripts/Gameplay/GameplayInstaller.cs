using Game.Gameplay;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public sealed class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private int _initialMoney = 300;

        [SerializeField]
        private PlanetCatalog _catalog;

        public override void InstallBindings()
        {
            MoneyInstaller.Install(this.Container, _initialMoney);
            PlanetInstaller.Install(this.Container, _catalog);
        }
    }
}