using UnityEngine;

namespace Game.App
{
    public sealed class AudioRepository
    {
        private readonly string _musicPrefsKey;
        private readonly string _soundPrefsKey;

        public AudioRepository(string musicPrefsKey, string soundPrefsKey)
        {
            _musicPrefsKey = musicPrefsKey;
            _soundPrefsKey = soundPrefsKey;
        }

        public bool TryGetMusicVolume(out float volume)
        {
            if (PlayerPrefs.HasKey(_musicPrefsKey))
            {
                volume = PlayerPrefs.GetFloat(_musicPrefsKey);
                return true;
            }

            volume = default;
            return false;
        }

        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(_musicPrefsKey, volume);
        }

        public bool TryGetSoundVolume(out float volume)
        {
            if (PlayerPrefs.HasKey(_soundPrefsKey))
            {
                volume = PlayerPrefs.GetFloat(_soundPrefsKey);
                return true;
            }

            volume = default;
            return false;
        }

        public void SetSoundVolume(float volume)
        {
            PlayerPrefs.SetFloat(_soundPrefsKey, volume);
        }
    }
}