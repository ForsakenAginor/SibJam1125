using Assets.Source.Scripts.DI.Services.Game;
using Assets.Source.Scripts.DI.Services.Global;
using System;
using UnityEngine;
using Zenject;

public class PlayerFacade : MonoBehaviour
{
    [SerializeField] private PlayerController _characterController;
    [SerializeField] private PlayerSensitivityChanger _sensitivityChanger;
    [SerializeField] private PlayerDamageTaker _damageTaker;

    private PlayerOxygenManager _oxygenManager;
    private IZenjectInstantiateWrapper _instantiateWrapper;

    [Inject]
    public void Construct(IZenjectInstantiateWrapper instantiateWrapper, INoiseVignetteEffect noiseVignetteEffect, IColorizationFSEffect colorizationFSEffect) 
    {
        _instantiateWrapper = instantiateWrapper;
        _oxygenManager = _instantiateWrapper.Create<PlayerOxygenManager>();
        OxygenFSEffectsAdapter oxygenFSEffectsAdapter = new OxygenFSEffectsAdapter(noiseVignetteEffect, colorizationFSEffect ,_oxygenManager.Oxigen);
    }

    public PlayerSensitivityChanger SensitivityChanger => _sensitivityChanger;

    public PlayerOxygenManager Oxygen => _oxygenManager;

    public PlayerDamageTaker DamageTaker => _damageTaker;
}
