using Assets.Source.Scripts.AudioLogic;
using Assets.Source.Scripts.DI.Services.Boot;
using Assets.Source.Scripts.DI.Services.Game;
using Assets.Source.Scripts.General;
using Assets.Source.Scripts.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Source.Scripts.EntryPoint
{
    public class Root : MonoBehaviour
    {
        [Header("Other")]
        [SerializeField] private AudioSaveLoadService _soundInitializer;
        [SerializeField] private Button[] _closeButtons;
        [SerializeField] private Button _restartButton;
        [SerializeField] private InGameMenu _gameMenu;
        [SerializeField] private Tutor _tutor;

        private ISceneChanger _sceneChanger;
        private SaveDataProvider _saveDataProvider;
        private List<IDataSaveLoadService> _saveLoadServices = new();
        private HealthVignetteEffect _healthVignette;
        private NoiceVignetteEffect _noiceVignette;
        private IColorizationFSEffect _colorizationFSEffect;
        private PlayerFacade _playerFacade;

        [Inject]
        public void Construct(ISceneChanger sceneChanger, SaveDataProvider saveDataProvider, HealthVignetteEffect healthVignette, NoiceVignetteEffect noiceVignette,
            IColorizationFSEffect colorizationFSEffect, IInputStateManager input, PlayerFacade playerFacade)
        {
            _sceneChanger = sceneChanger;
            _saveDataProvider = saveDataProvider;
            _healthVignette = healthVignette;
            _noiceVignette = noiceVignette;
            _colorizationFSEffect = colorizationFSEffect;
            _playerFacade = playerFacade;

            input.ToWorldState();

            //_colorizationFSEffect.Enable();
            _healthVignette.Enable();
            //_noiceVignette.Enable();
            _saveLoadServices.Add(_soundInitializer);
        }

        private void Start()
        {
            _tutor.Init(_playerFacade.Oxygen);



            SensitivitySaveLoadService sensitivitySaveLoadService = new SensitivitySaveLoadService(_gameMenu);
            _saveLoadServices.Add(sensitivitySaveLoadService);
            LoadData();

            foreach(var button in _closeButtons)
                button.onClick.AddListener(OnCloseButtonClick);

            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _sceneChanger.FadeOut();
            Time.timeScale = 1f;
        }

        private void OnDestroy()
        {
            _colorizationFSEffect.Disable();
            _healthVignette.Disable();
            _noiceVignette.Disable();

            foreach (var button in _closeButtons)
                button.onClick.RemoveListener(OnCloseButtonClick);

            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }


        private void SaveData()
        {
            foreach (var service in _saveLoadServices)
            {
                service.Save();
            }

            _saveDataProvider.Save();
        }

        private void LoadData()
        {
            foreach (var service in _saveLoadServices)
            {
                service.Init(_saveDataProvider.PlayerSavedData);
            }

            foreach (var service in _saveLoadServices)
            {
                service.Load();
            }
        }

        private void OnRestartButtonClick()
        {
            SaveData();
            _sceneChanger.LoadSceneIgnoreTimeScale(Scenes.Game.ToString());
        }

        private void OnCloseButtonClick()
        {
            SaveData();
            _sceneChanger.LoadSceneIgnoreTimeScale(Scenes.Menu.ToString());
        }
    }
}