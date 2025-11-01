using Assets.Source.Scripts.DI.Services.Game;
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

public class OxygenFSEffectsAdapter
{
    private readonly INoiseVignetteEffect _noiseEffect;
    private readonly IColorizationFSEffect _colorizationEffect;
    private readonly IResource _oxygen;

    public OxygenFSEffectsAdapter(INoiseVignetteEffect noiseEffect, IColorizationFSEffect colorizationFSEffect, IResource oxygen)
    {
        _noiseEffect = noiseEffect;
        _colorizationEffect = colorizationFSEffect;
        _oxygen = oxygen;

        _oxygen.ResourcesAmountChanged += OnOxyChanged;
    }

    ~OxygenFSEffectsAdapter()
    {
        _oxygen.ResourcesAmountChanged -= OnOxyChanged;
    }

    private void OnOxyChanged()
    {
        _noiseEffect.SetEffectStrength(_oxygen.Percent);
        _colorizationEffect.SetStrength(_oxygen.Percent);
    }
}