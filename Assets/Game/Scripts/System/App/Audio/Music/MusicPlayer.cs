using System;
using UnityEngine;

namespace Game.App
{
    public sealed class MusicPlayer
    {
        private const float VOLUME_TOLERANCE = 0.01f;

        public event Action OnStateChanged;
        public event Action<float> OnVolumeChanged;
        public event Action<bool> OnMuteChanged;

        private readonly MusicCatalog _catalog;
        private readonly AudioSource _source;

        public MusicPlayer(MusicCatalog catalog, AudioSource source)
        {
            _catalog = catalog;
            _source = source;
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

        public void Play(MusicName music, bool force = false)
        {
            AudioClip clip = _catalog.GetTrack(music);

            if (force || _source.clip != clip)
            {
                _source.clip = clip;
                _source.Play();
            }
        }

        public void Pause()
        {
            _source.Pause();
        }

        public void Stop()
        {
            _source.Stop();
        }

        public void Resume()
        {
            _source.UnPause();
        }
    }
}