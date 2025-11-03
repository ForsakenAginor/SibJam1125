using Assets.Source.Scripts.Utility;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class Tutor : MonoBehaviour
{
    [SerializeField] private OxygenDrainTrigger _oxygenDrainTrigger;
    [SerializeField] private SwitchableElement _cleanKey;
    [SerializeField] private FlashlightObject _flashlightObject;
    [SerializeField] private SwitchableElement _flashlightIcon;

    private CharacterController _characterController;
    private Flashlight _flashlight;
    private PlayerOxygenManager _oxygenManager;
    private IPlayerInput _playerInput;
    private IInputStateManager _inputStateManager;
    private bool _isOxyDraining = false;

    private Tween _cleenKeyTween;

    [Inject]
    public void Construct(IPlayerInput playerInput, IInputStateManager inputStateManager, PlayerFacade playerFacade)
    {
        _playerInput = playerInput;
        _inputStateManager = inputStateManager;
        _characterController = playerFacade.Colider;
        _flashlight = playerFacade.Flashlight;

        _playerInput.OnClean += OnClean;
    }

    private void OnDestroy()
    {
        _playerInput.OnClean -= OnClean;
        _flashlightObject.Pickuped -= OnPickuped;
        _oxygenManager.OxygenRestored -= OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter -= OnOxygenRestored;
    }

    public void Init(PlayerOxygenManager oxygenManager)
    {
        _oxygenManager = oxygenManager;
        _flashlight.Init();
        CreateCleenTween();

        _flashlightObject.Pickuped += OnPickuped;
        _oxygenManager.OxygenRestored += OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter += OnOxygenRestored;
    }

    private void OnPickuped()
    {
        _flashlight.Enable();
        _flashlightIcon.Enable();
    }

    private void CreateCleenTween()
    {
        _cleenKeyTween = _cleanKey.transform.DOScale(2, 0.5f).SetLoops(-1,LoopType.Yoyo);
    }

    private void OnClean()
    {
        _inputStateManager.ToWorldState();
        _cleenKeyTween?.Kill();
        _cleanKey.Disable();
        DOTween.To(
            () => _characterController.height,
            newHeight => _characterController.height = newHeight,
            2,
            1f).SetEase(Ease.Linear);
    }

    private void OnOxygenRestored()
    {
        if (_isOxyDraining == false)
        {
            _isOxyDraining = true;
            _oxygenManager.StartOxygenDrain();
        }
    }
}
