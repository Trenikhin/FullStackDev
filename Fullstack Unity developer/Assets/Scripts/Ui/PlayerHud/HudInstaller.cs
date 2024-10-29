namespace Ui.PlayerHud
{
    using ShootEmUp;
    using UnityEngine;

    public class HudInstaller : MonoBehaviour
    {
        [SerializeField] UiPlayerHudView _view;
        [SerializeField] Ship _ship;
         
        UiPlayerHudPresenter _presenter;
        
        void Awake()
        {
            _presenter = new UiPlayerHudPresenter(_view, _ship);
            
            _presenter.Init();
        }

        void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}