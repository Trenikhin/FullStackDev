namespace Game.UI
{
	using Modules.Planets;
	using Zenject;

	public interface IPlanetShower
	{
		void Show( IPlanet planet );
	}

	public interface IPlanetHider
	{
		void Hide();
	}
	
	public class PlanetShower : IPlanetShower, IPlanetHider
	{
		[Inject] PlanetPopupPresenter _presenter;
		[Inject] PlanetPopupView _view;
		
		public void Show(IPlanet planet)
		{
			_presenter.Setup( planet );
			_presenter.OnShow();
			_view.Show();
		}

		public void Hide()
		{
			_presenter.OnHide();
			_view.Hide();
		}
	}
}