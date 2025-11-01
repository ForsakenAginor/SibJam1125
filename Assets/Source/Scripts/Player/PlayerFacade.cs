using Assets.Source.Scripts.DI.Services.Global;
using UnityEngine;
using Zenject;

public class PlayerFacade : MonoBehaviour
{
    [SerializeField] private PlayerController _characterController;
    [SerializeField] private PlayerSensitivityChanger _sensitivityChanger;

    private PlayerOxygenManager _oxygenManager;
    private IZenjectInstantiateWrapper _instantiateWrapper;

    [Inject]
    public void Construct(IZenjectInstantiateWrapper instantiateWrapper)
    {
        _instantiateWrapper = instantiateWrapper;
        _oxygenManager = _instantiateWrapper.Create<PlayerOxygenManager>();
    }

    public PlayerSensitivityChanger SensitivityChanger => _sensitivityChanger;

    public PlayerOxygenManager Oxygen => _oxygenManager;
}
