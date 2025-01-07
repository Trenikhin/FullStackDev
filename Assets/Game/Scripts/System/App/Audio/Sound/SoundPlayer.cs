using System;
using Game.Audio;
using UnityEngine;

namespace Game.App
{
    public sealed class SoundPlayer
    {
        private const float VOLUME_TOLERANCE = 0.01f;

        public event Action<float> OnVolumeChanged;
        public event Action<bool> OnMuteChanged;
        public event Action OnStateChanged;

        private readonly AudioSource _source;
        private readonly SoundCatalog _catalog;

        public SoundPlayer(SoundCatalog catalog, AudioSource source)
        {
            _source = source;
            _catalog = catalog;
        }

        public void Play(SoundName name)
        {
            AudioClip clip = _catalog.GetSound(name);
            _source.PlayOneShot(clip);
        }

        public float GetVolume()
        {
            return _source.volume;
        }

        public void SetVolume(float volume)
        {
            if (Math.Abs(_source.volume - volume) > VOLUME_TOLERANCE)
            {
                _source.volume = volume;
                this.OnVolumeChanged?.Invoke(volume);
                this.OnStateChanged?.Invoke();
            }
        }

        public bool IsMute()
        {
            return _source.mute;
        }

        public void SetMute(bool mute)
        {
            if (_source.mute != mute)
            {
                _source.mute = mute;
                this.OnMuteChanged?.Invoke(mute);
                this.OnStateChanged?.Invoke();
            }
        }
    }
}