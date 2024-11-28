namespace Game.UI
{
	using Modules.Planets;
	using Zenject;

	public interface IUiNavigator
	{
		void Show(IPlanet planet);
		void Hide();
	}
	
	public interface IPopupHandler
	{
		void OnHide();
	}
	
	public class UiNavigator : IUiNavigator
	{
		[Inject] IPlanetPopupHandler _planetPopupHandler;
		
		IPopupHandler _popupHandler;
		
		public void Show( IPlanet planet )
		{
			_popupHandler = _planetPopupHandler;
			_planetPopupHandler.OnShow( planet );
		}
		
		public void Hide()
		{
			_popupHandler.OnHide();
		}
	}
}