using System;
using System.Collections.Generic;
using Atomic.UI;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(
        fileName = "ScreenCatalog",
        menuName = "Game/UI/New ScreenCatalog"
    )]
    public sealed class ScreenCatalog : ScriptableObject
    {
        [SerializeField]
        private Screen[] screens;
        
        public Presenter GetScreenPrefab(ScreenName name)
        {
            foreach (Screen screen in this.screens)
                if (screen.name == name)
                    return screen.view;

            throw new KeyNotFoundException($"Screen with name {name} is not found!");
        }
        
        [Serializable]
        private struct Screen
        {
            public ScreenName name;
            public Presenter view;
        }  
    }
}