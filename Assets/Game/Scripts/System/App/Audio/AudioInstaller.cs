using Game.Audio;
using UnityEngine;
using Zenject;

namespace Game.App
{
    public sealed class AudioInstaller : MonoInstaller
    {
        [Header("Sound")]
        [SerializeField]
        private SoundCatalog _soundCatalog;

        [SerializeField]
        private AudioSource _soundSource;

        [SerializeField]
        private float _initialSoundVolume = 0.5f;

        [SerializeField]
        private string _soundPrefsKey = "SoundVolume";

        [Header("Music")]
        [SerializeField]
        private MusicCatalog _musicCatalog;

        [SerializeField]
        private float _initialMusicVolume = 0.5f;

        [SerializeField]
        private AudioSource _musicSource;

        [SerializeField]
        private string _musicPrefsKey = "MusicVolume";

        public override void InstallBindings()
        {
            this.Container
                .Bind<SoundPlayer>()
                .FromInstance(new SoundPlayer(_soundCatalog, _soundSource))
                .AsSingle()
                .OnInstantiated<SoundPlayer>((_, it) => it.SetVolume(_initialSoundVolume))
                .NonLazy();

            this.Container
                .Bind<MusicPlayer>()
                .FromInstance(new MusicPlayer(_musicCatalog, _musicSource))
                .AsSingle()
                .OnInstantiated<MusicPlayer>((_, it) => it.SetVolume(_initialMusicVolume))
                .NonLazy();

            this.Container
                .BindInterfacesAndSelfTo<AudioSaveLoader>()
                .AsSingle();

            this.Container
                .Bind<AudioRepository>()
                .FromInstance(new AudioRepository(_musicPrefsKey, _soundPrefsKey))
                .AsSingle();
        }
    }
}