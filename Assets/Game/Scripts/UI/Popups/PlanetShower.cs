namespace Game.UI
{
	using Modules.Planets;
	using Zenject;

	public interface IPlanetShower
	{
		void Show( IPlanet planet );
	}
	
	public class PlanetShower : IPlanetShower
	{
		[Inject] PlanetPopupPresenter _presenter;
		[Inject] PlanetPopupView _view;
		
		public void Show(IPlanet planet)
		{
			_presenter.Setup( planet );
			_view.Show();
		}
	}
}