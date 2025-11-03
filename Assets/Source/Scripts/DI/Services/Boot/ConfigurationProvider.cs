using UnityEngine;

namespace Assets.Source.Scripts.DI.Services.Boot
{
    [CreateAssetMenu(fileName = "ConfigurationsProvider", menuName = "Services/ConfigurationsProvider")]
    public class ConfigurationProvider : ScriptableObject, IGameSettings
    {
        [SerializeField] private GameSettings _gameSettings;

        public float OxygenDrainFrequency => _gameSettings.OxygenDrainFrequency;

        public int OxygenBasicDrain => _gameSettings.OxygenBasicDrain;

        public int OxygenMaximum => _gameSettings.OxygenMaximum;

        public float SprintSpeed => _gameSettings.SprintSpeed;

        public float WalkSpeed => _gameSettings.WalkSpeed;

        public float SprintOxygenMultiplier => _gameSettings.SprintOxygenMultiplier;

        public float OxygenReloadTime => _gameSettings.OxygenReloadTime;

        public float StartedOxygenPercent => _gameSettings.StartedOxygenPercent;
    }
}
