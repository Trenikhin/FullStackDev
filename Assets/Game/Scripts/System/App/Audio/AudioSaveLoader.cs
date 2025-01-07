using System;
using Zenject;

namespace Game.App
{
    public sealed class AudioSaveLoader : IInitializable, IDisposable
    {
        private readonly MusicPlayer _musicPlayer;
        private readonly SoundPlayer _soundPlayer;
        private readonly AudioRepository _repository;

        public AudioSaveLoader(MusicPlayer musicPlayer, SoundPlayer soundPlayer, AudioRepository repository)
        {
            _musicPlayer = musicPlayer;
            _soundPlayer = soundPlayer;
            _repository = repository;
        }

        void IInitializable.Initialize()
        {
            this.SetupPlayers();
            
            _musicPlayer.OnVolumeChanged += _repository.SetMusicVolume;
            _soundPlayer.OnVolumeChanged += _repository.SetSoundVolume;
        }

        void IDisposable.Dispose()
        {
            _musicPlayer.OnVolumeChanged -= _repository.SetMusicVolume;
            _soundPlayer.OnVolumeChanged -= _repository.SetSoundVolume;
        }

        private void SetupPlayers()
        {
            if (_repository.TryGetMusicVolume(out float musicVolume))
                _musicPlayer.SetVolume(musicVolume);

            if (_repository.TryGetSoundVolume(out float soundVolume))
                _soundPlayer.SetVolume(soundVolume);
        }
    }
}