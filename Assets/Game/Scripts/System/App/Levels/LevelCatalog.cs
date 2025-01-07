using System.Collections.Generic;
using UnityEngine;

namespace Game.App
{
    [CreateAssetMenu(
        fileName = "LevelCatalog",
        menuName = "Game/App/New LevelCatalog"
    )]
    public sealed class LevelCatalog : ScriptableObject
    {
        [SerializeField]
        private LevelConfig[] _levels;

        public int LevelCount => _levels.Length;

        public LevelConfig FindLevel(int level)
        {
            for (int i = 0, count = _levels.Length; i < count; i++)
            {
                LevelConfig config = _levels[i];
                if (config.Number == level)
                    return config;
            }

            throw new KeyNotFoundException($"Level config {level} is not found!");
        }
    }
}