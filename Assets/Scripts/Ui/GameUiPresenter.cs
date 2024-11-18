namespace Ui
{
	using System;
	using Core;
	using Modules;
	using SnakeGame;
	using Zenject;

	public class GameUiPresenter : IInitializable, IDisposable
	{
		IGameUI _view;
		IDifficulty _difficulty;
		IGameOverHandler _gameOverHandler;
		IScore _score;
		
		public GameUiPresenter(IGameUI view, IScore score, IDifficulty difficulty, IGameOverHandler gameOverHandler)
		{
			_view = view;
			_score = score;
			_difficulty = difficulty;
			_gameOverHandler = gameOverHandler;
		}

		public void Initialize()
		{
			_view.SetScore( _score.Current.ToString() );
			_view.SetDifficulty( _difficulty.Current, _difficulty.Max );
			
			_score.OnStateChanged += OnScoreChanged;
			_difficulty.OnStateChanged += OnDifficultyChanged;
			_gameOverHandler.OnGameOver += OnGameOver;
		}

		public void Dispose()
		{
			_score.OnStateChanged -= OnScoreChanged;
			_difficulty.OnStateChanged -= OnDifficultyChanged;
			_gameOverHandler.OnGameOver -= OnGameOver;
		}
		
		void OnGameOver( bool isWin ) => _view.GameOver( isWin );
		void OnScoreChanged( int cur ) => _view.SetScore( cur.ToString() );
		void OnDifficultyChanged() => _view.SetDifficulty( _difficulty.Current, _difficulty.Max );
	}
}