using System.Collections.Generic;
using Atomic.UI;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class ScreenNavigator : IInitializable
    {
        private readonly Transform _viewport;
        private readonly ScreenCatalog _catalog;
        private readonly IInstantiator _instantiator;
        private readonly Dictionary<ScreenName, Presenter> _screens = new();
        private readonly ScreenName _initialScreen;

        private ScreenName _currentName;
        private Presenter _currentPresenter;

        public ScreenNavigator(
            ScreenCatalog catalog,
            Transform viewport,
            IInstantiator instantiator,
            ScreenName initialScreen
        )
        {
            _viewport = viewport;
            _catalog = catalog;
            _instantiator = instantiator;
            _initialScreen = initialScreen;
        }

        public IPresenter Current => _currentPresenter;

        public bool IsCurrent(ScreenName name)
        {
            return _currentName == name;
        }

        void IInitializable.Initialize()
        {
            this.ShowScreen(_initialScreen);
        }

        public void ChangeScreen(ScreenName name)
        {
            if (_currentName == name)
                return;

            if (_currentPresenter != null)
                _currentPresenter.Hide();

            this.ShowScreen(name);
        }

        private void ShowScreen(ScreenName name)
        {
            if (!_screens.TryGetValue(name, out Presenter screen))
            {
                Presenter prefab = _catalog.GetScreenPrefab(name);
                GameObject view = _instantiator.InstantiatePrefab(prefab.gameObject, _viewport);
                screen = view.GetComponent<Presenter>();
                _screens.Add(name, screen);
            }

            _currentName = name;
            _currentPresenter = screen;

            screen.Show();
        }
    }
}