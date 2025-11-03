using Assets.Source.Scripts.DI.Services.Global;
using Assets.Source.Scripts.Utility;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerOxygenManager
{
    private readonly int _damageValue = 1;
    private readonly float _damageFrequency = 2f;
    private readonly float _sprintMultiplier = 1.1f;

    private readonly Resource _oxygen;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IPlayerInput _input;

    private WaitForSeconds _delay;

    public PlayerOxygenManager(ICoroutineRunner coroutineRunner, IGameSettings gameSettings, IPlayerInput input)
    {
        int starterOxygen = (int)(gameSettings.StartedOxygenPercent * gameSettings.OxygenMaximum);
        _oxygen = new Resource(starterOxygen, gameSettings.OxygenMaximum);
        _coroutineRunner = coroutineRunner;
        _input = input;

        _damageValue = gameSettings.OxygenBasicDrain;
        _damageFrequency = gameSettings.OxygenDrainFrequency;
        _sprintMultiplier = gameSettings.SprintOxygenMultiplier;

        _delay = new WaitForSeconds(_damageFrequency);
    }

    public event Action PlayerDied;
    public event Action OxygenRestored;

    public IResource Oxigen => _oxygen;

    public void RestoreOxygen()
    {
        _oxygen.Add(_oxygen.Maximum);
        OxygenRestored?.Invoke();
    }

    public void StartOxygenDrain()
    {
        _coroutineRunner.StartCoroutine(TakeDamage());
    }

    private IEnumerator TakeDamage()
    {
        yield return _delay;

        while (_oxygen.Amount > 0)
        {
            int totalValue = _damageValue;
            totalValue = _input.IsSprinted ? (int)(_damageValue * _sprintMultiplier) : _damageValue;
            _oxygen.Spent(totalValue);
            yield return _delay;
        }

        PlayerDied?.Invoke();
    }
}
