namespace Core
{
	using System;
	using Input;
	using Modules;
	using Zenject;
		
	public class ScoreCounter: IInitializable, IDisposable
	{
		ICoinTrigger _coinTrigger;
		IScore _score;
		
		public ScoreCounter( ICoinTrigger coinTrigger, IScore score )
		{
			_coinTrigger = coinTrigger;
			_score = score;
		}

		public void Initialize() => _coinTrigger.OnCoinTouch += OnCoinTouched;
		public void Dispose() => _coinTrigger.OnCoinTouch -= OnCoinTouched;
		
		void OnCoinTouched(Coin coin) => _score.Add(coin.Score);
	}
}