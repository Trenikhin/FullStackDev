using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.App
{
    public sealed class SceneNavigator
    {
        private const string MenuScene = "MenuScene";
        private const string GameScene = "GameScene";

        private string _currentScene = MenuScene;

        public async UniTask OpenGame()
        {
            if (_currentScene != GameScene)
            {
                _currentScene = GameScene;
                await SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Single);
            }
        }

        public async UniTask OpenMenu()
        {
            if (_currentScene != MenuScene)
            {
                _currentScene = MenuScene;
                await SceneManager.LoadSceneAsync(MenuScene, LoadSceneMode.Single);
            }
        }
    }
}