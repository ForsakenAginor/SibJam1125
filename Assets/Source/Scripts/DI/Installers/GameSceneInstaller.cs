using Assets.Source.Scripts.DI.Services.Game;
using Assets.Source.Scripts.DI.Services.Global;
using Assets.Source.Scripts.Utility;
using Assets.Source.Scripts.Utility.Pools;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Zenject;

namespace Assets.Source.Scripts.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Header("Audio")]
        [SerializeField] private UIAudioPlayer _uiAudioPlayerPrefab;
        [SerializeField] private MusicPlayer _musicPlayerPrefab;
        [SerializeField] private AudioPlayer _audioPlayerPrefab;

        [Header("Other")]
        [SerializeField] private PlayerInteractor _playerInteractorPrefab;
        [SerializeField] private Image _flashlightImage;

        [Header("Player")]
        [SerializeField] private PlayerFacade _playerPrefab;

        [Header("Vignettes")]
        [SerializeField] private ScriptableRendererFeature _healthVignetteEffect;
        [SerializeField] private Material _healthVignetteMaterial;
        [SerializeField] private ScriptableRendererFeature _noiceVignetteEffect;
        [SerializeField] private Material _noiceVignetteMaterial;
        [SerializeField] private ScriptableRendererFeature _colorizationFSEffect;
        [SerializeField] private Material _colorizationFSMaterial;

        private ZenjectInstantiateWrapper _instantiateWrapper;

        public override void InstallBindings()
        {
            BindInstantiateWrapper();
            BindPoolFactory();
            BindCoroutineRunner();
            BindAudio();
            BindTimeIncrement();
            BindHealthVignetteEffect();
            BindNoiceVignetteEffect();
            BindColorizationEffect();
            BindPlayer();
            BindInteractor();
            BindSpidersFactory();
        }

        private void BindSpidersFactory()
        {
            SpiderStateMachineFactory factory = _instantiateWrapper.Create<SpiderStateMachineFactory>();

            Container
                .Bind<SpiderStateMachineFactory>()
                .To<SpiderStateMachineFactory>()
                .FromInstance(factory)
                .AsSingle()
                .NonLazy();
        }

        private void BindInteractor()
        {
            PlayerInteractor interactor = _instantiateWrapper.Instantiate(_playerInteractorPrefab, Vector3.zero, Quaternion.identity);

            Container
                .Bind<PlayerInteractor>()
                .To<PlayerInteractor>()
                .FromInstance(interactor)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IPlayerInteractor>()
                .To<PlayerInteractor>()
                .FromInstance(interactor)
                .AsCached();
        }

        private void BindPlayer()
        {
            PlayerFacade player = _instantiateWrapper.Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            PlayerTransform playerTransform = player.GetComponentInChildren<PlayerTransform>();
            var flashlight = player.GetComponentInChildren<Flashlight>();
            flashlight.fillImage = _flashlightImage;

            Container
                .Bind<PlayerFacade>()
                .To<PlayerFacade>()
                .FromInstance(player)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IPlayerTransform>()
                .To<PlayerTransform>()
                .FromInstance(playerTransform)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PlayerSensitivityChanger>()
                .To<PlayerSensitivityChanger>()
                .FromInstance(player.SensitivityChanger)
                .AsSingle()
                .NonLazy();
        }

        private void BindColorizationEffect()
        {
            ColorizationFSEffect effect = new ColorizationFSEffect(_colorizationFSEffect, _colorizationFSMaterial);

            Container
                .Bind<ColorizationFSEffect>()
                .To<ColorizationFSEffect>()
                .FromInstance(effect)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IColorizationFSEffect>()
                .To<ColorizationFSEffect>()
                .FromInstance(effect)
                .AsCached();
        }

        private void BindNoiceVignetteEffect()
        {
            NoiceVignetteEffect effect = new NoiceVignetteEffect(_noiceVignetteEffect, _noiceVignetteMaterial);

            Container
                .Bind<NoiceVignetteEffect>()
                .To<NoiceVignetteEffect>()
                .FromInstance(effect)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<INoiseVignetteEffect>()
                .To<NoiceVignetteEffect>()
                .FromInstance(effect)
                .AsCached();
        }

        private void BindHealthVignetteEffect()
        {
            HealthVignetteEffect effect = new HealthVignetteEffect(_healthVignetteEffect, _healthVignetteMaterial);

            Container
                .Bind<HealthVignetteEffect>()
                .To<HealthVignetteEffect>()
                .FromInstance(effect)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IHealthDamageEffect>()
                .To<HealthVignetteEffect>()
                .FromInstance(effect)
                .AsCached();
        }

        private void BindPoolFactory()
        {
            PoolableFactory poolableFactory = new (Container);
            PoolFactory factory = new(poolableFactory, transform);

            Container
                .Bind<IPoolFactory>()
                .To<PoolFactory>()
                .FromInstance(factory)
                .AsSingle()
                .NonLazy();
        }

        private void BindTimeIncrement()
        {
            GameTimeService timeService = gameObject.AddComponent<GameTimeService>();

            Container
                .Bind<IGameTimeService>()
                .To<GameTimeService>()
                .FromInstance(timeService)
                .AsSingle();
        }

        private void BindCoroutineRunner()
        {
            ZenjectCoroutineRunner runner = new(this);

            Container
                .Bind<ICoroutineRunner>()
                .To<ZenjectCoroutineRunner>()
                .FromInstance(runner)
                .AsSingle()
                .NonLazy();
        }

        private void BindInstantiateWrapper()
        {
            _instantiateWrapper = new ZenjectInstantiateWrapper(Container);

            Container.Bind<IZenjectInstantiateWrapper>()
                .To<ZenjectInstantiateWrapper>()
                .FromInstance(_instantiateWrapper)
                .AsSingle()
                .NonLazy();
        }

        private void BindAudio()
        {
            UIAudioPlayer uIAudioPlayer = Container.InstantiatePrefabForComponent<UIAudioPlayer>(_uiAudioPlayerPrefab);
            MusicPlayer musicPlayer = Container.InstantiatePrefabForComponent<MusicPlayer>(_musicPlayerPrefab);
            AudioPlayer audioPlayer = Container.InstantiatePrefabForComponent<AudioPlayer>(_audioPlayerPrefab);

            Container
                .Bind<IUIAudioPlayer>()
                .To<UIAudioPlayer>()
                .FromInstance(uIAudioPlayer)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IMusicPlayer>()
                .To<MusicPlayer>()
                .FromInstance(musicPlayer)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<AudioPlayer>()
                .To<AudioPlayer>()
                .FromInstance(audioPlayer)
                .AsSingle()
                .NonLazy();
        }
    }
}