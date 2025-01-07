using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.App
{
    [CreateAssetMenu(
        fileName = "MusicCatalog",
        menuName = "Game/App/New MusicCatalog"
    )]
    public sealed class MusicCatalog : ScriptableObject
    {
        [SerializeField]
        private Music[] _tracks;

        public AudioClip GetTrack(MusicName name)
        {
            for (int i = 0, count = _tracks.Length; i < count; i++)
            {
                Music music = _tracks[i];
                if (music.name == name)
                    return music.clip;
            }

            throw new KeyNotFoundException($"Music with name {name} is not found!");
        } 
        
        [Serializable]
        private struct Music
        {
            public MusicName name;
            public AudioClip clip;
        }
    }
}