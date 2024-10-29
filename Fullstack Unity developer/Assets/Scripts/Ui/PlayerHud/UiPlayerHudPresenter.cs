namespace Ui.PlayerHud
{
    using System;
    using ShootEmUp;

    public class UiPlayerHudPresenter: IInitializable, IDisposable
    {
        readonly UiPlayerHudView _view;
        readonly Ship _playerShip;

        public UiPlayerHudPresenter( UiPlayerHudView view, Ship ship )
        {
            _view           = view;
            _playerShip     = ship;
        }
        
        public void Init()
        {
            SetHealth( _playerShip.Health.Value );
            
            _playerShip.Health.OnHealthChanged += SetHealth;
        }

        public void Dispose()
        {
            _playerShip.Health.OnHealthChanged -= SetHealth;
        }

        void SetHealth( int health )
        {
            var playerHealth = health.ToString();
            _view.SetHealth( playerHealth );
        }
    }
}