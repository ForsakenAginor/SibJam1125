using Sirenix.OdinInspector;
using UnityEngine;

public interface IGameSettings
{
    public float OxygenDrainFrequency { get; }

    public int OxygenBasicDrain { get; }

    public int OxygenMaximum { get; }

    public float SprintSpeed { get; }

    public float WalkSpeed { get; }

    public float SprintOxygenMultiplier { get; }
}

[CreateAssetMenu(fileName = "GameConfigurations", menuName = "Configurations/GameConfigurations")]
public class GameSettings : SerializedScriptableObject, IGameSettings
{
    [SerializeField] private float _walkSpeed = 2;
    [SerializeField] private float _sprintSpeed = 4;
    [SerializeField] private int _oxygenMaximum = 1000;
    [SerializeField] private int _oxygenBasicDrain = 10;
    [SerializeField] private float _drainFrequency = 1f;
    [SerializeField] private float _sprintOxygenMultiplier = 1.1f;

    public float WalkSpeed => _walkSpeed;

    public float SprintSpeed => _sprintSpeed;

    public int OxygenMaximum => _oxygenMaximum;

    public int OxygenBasicDrain => _oxygenBasicDrain;

    public float OxygenDrainFrequency => _drainFrequency;

    public float SprintOxygenMultiplier => _sprintOxygenMultiplier;
}
