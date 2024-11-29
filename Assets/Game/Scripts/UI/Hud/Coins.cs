namespace Game.UI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Modules.Money;
	using Obj;
	using Sirenix.Utilities;
	using UniRx;
	using Zenject;

	public interface ICoins
	{
		IReadOnlyReactiveProperty<int> Showing { get; } // Coins to show != current player coins
		
		void Hide(int coins);
	}
	
	public class Coins : ICoins , IInitializable, IDisposable
	{
		[Inject] IFlyIcons _flyIcons;
		[Inject] IMoneyStorage _storage;
		[Inject] List<IPlanetFacade> _planets;
		
		ReactiveProperty<int> _show = new (0);
		ReactiveProperty<int> _hidden = new (0);

		List<Action<int>> _gatheredHandlers = new ();
		
		public void Initialize()
		{
			Showing = Observable
				.CombineLatest( _show, _hidden, (c, h) => c - h )
				.ToReadOnlyReactiveProperty();
			
			_show.Value = _storage.Money;
			_storage.OnMoneyChanged += OnChanged;
			
			// TODO: Workaround
			_planets
				.Select( p => p )
				.ForEach(p =>
				{
					Action<int> handler = v => OnGathered(v, p);
					p.Planet.OnGathered += handler;
					_gatheredHandlers.Add(handler);
				});
		}

		public void Dispose()
		{
			_planets
				.Select(p => p)
				.Where( (_, i) => i < _gatheredHandlers.Count )
				.ForEach((p, index) => p.Planet.OnGathered -= _gatheredHandlers[index] );
			_gatheredHandlers.Clear();
			
			_storage.OnMoneySpent -= OnChanged;
		}

#region ICoins
		
		public IReadOnlyReactiveProperty<int> Showing { get; private set; }
		
		public void Hide( int coins ) => _hidden.Value = coins;
		
#endregion 		
		
		void OnGathered(int delta, IPlanetFacade planet)
		{
			_flyIcons.Fly(planet.View.Coin, delta);
		}

		void OnChanged(int newValue, int oldValue)
		{
			_show.Value = newValue;
		}
	}
}