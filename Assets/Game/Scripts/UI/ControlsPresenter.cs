namespace Game.Gameplay
{
    using System;
    using Cysharp.Threading.Tasks;
    using SaveSystem;
    using UnityEngine;
    using Zenject;
    
    public sealed class ControlsPresenter : IControlsPresenter
    {
        [Inject] ISaver _saver;
        
        public void Save(Action<bool, int> callback)
        {
            SaveAsync(callback).Forget();
        }

        public void Load(string versionText, Action<bool, int> callback)
        {
            LoadAsync(versionText, callback).Forget();
        }
        
        async UniTaskVoid SaveAsync( Action<bool, int> callback )
        {
            await _saver.SaveAsync();
            callback.Invoke(true, _saver.LastSaveVersion);
        }
        
        async UniTaskVoid LoadAsync( string ver, Action<bool, int> callback )
        {
            int verNum = int.TryParse(ver, out verNum) ? verNum : 1;
            await _saver.LoadAsync( verNum );
            callback.Invoke(true, verNum);
        }
    }
}