using Assets.Source.Scripts.DI.Services.Global;
using Assets.Source.Scripts.Utility;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerOxygenManager
{
    private const int DamageValue = 1;

    private readonly float _damageFrequency = 2f;
    private readonly Resource _oxygen;
    private readonly ICoroutineRunner _coroutineRunner;

    private WaitForSeconds _delay;

    public PlayerOxygenManager(ICoroutineRunner coroutineRunner)
    {
        _oxygen = new Resource(100);
        _coroutineRunner = coroutineRunner;
        _delay = new WaitForSeconds(_damageFrequency);

        _coroutineRunner.StartCoroutine(TakeDamage());
    }

    public event Action PlayerDied;

    public IResource Oxigen => _oxygen;

    public void RestoreOxygen()
    {
        _oxygen.Add(100);
    }

    private IEnumerator TakeDamage()
    {
        yield return _delay;

        while (_oxygen.TrySpent(DamageValue))
        {
            yield return _delay;
        }

        PlayerDied?.Invoke();
    }
}
