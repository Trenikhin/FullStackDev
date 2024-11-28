namespace Game.UI
{
	using Modules.Planets;
	using UniRx;
	using Zenject;

	public interface IPlanetPopup
	{
		void Show(IPlanet planet);
		void Hide();
	}
	
	public class PlanetPopupModel
	{
		public ReactiveProperty<bool> IsShowing  = new (false);
		public IPlanet Planet;
	}
	
	public class PlanetPopup : IPlanetPopup
	{
		[Inject] PlanetPopupModel _model;
		
		public void Show( IPlanet planet )
		{
			_model.Planet			= planet;
			_model.IsShowing.Value	= true;
		}
		
		public void Hide()
		{
			_model.IsShowing.Value		= false;
			_model.Planet				= null;
		}
	}
}