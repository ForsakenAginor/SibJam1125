using Assets.Source.Scripts.DI.Services.Game;
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
    [SerializeField] private SwitchableElement _pickupFlashlightText;
    [SerializeField] private VoiceMessagePlayer _voiceMessagePlayer;

    [SerializeField] private SpiderNavigator[] _spiders;

    private CharacterController _characterController;
    private Flashlight _flashlight;
    private PlayerOxygenManager _oxygenManager;
    private IPlayerInput _playerInput;
    private IInputStateManager _inputStateManager;
    private bool _isOxyDraining = false;
    private bool _isFlashlighPickuped = false;
    private AudioPlayer _audioPlayer;

    private Tween _cleenKeyTween;

    [Inject]
    public void Construct(IPlayerInput playerInput, IInputStateManager inputStateManager, PlayerFacade playerFacade, PlayerOxygenManager playerOxygenManager, AudioPlayer audioPlayer)
    {
        _playerInput = playerInput;
        _inputStateManager = inputStateManager;
        _characterController = playerFacade.Colider;
        _flashlight = playerFacade.Flashlight;
        _oxygenManager = playerOxygenManager;
        _audioPlayer = audioPlayer;

        _playerInput.OnClean += OnClean;

        foreach(var spider in _spiders)
        {
            spider.AttackPlayer += OnPlayerAttacked;
        }    
    }

    private void OnDestroy()
    {
        _playerInput.OnClean -= OnClean;
        _flashlightObject.Pickuped -= OnPickuped;
        _oxygenManager.OxygenRestored -= OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter -= OnOxygenRestored;

        foreach (var spider in _spiders)
        {
            spider.AttackPlayer -= OnPlayerAttacked;
        }
    }

    public void Init()
    {
        _flashlight.Init();
        CreateCleenTween();

        _flashlightObject.Pickuped += OnPickuped;
        _oxygenManager.OxygenRestored += OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter += OnOxygenRestored;
    }

    private void OnPlayerAttacked()
    {
        foreach (var spider in _spiders)
        {
            spider.AttackPlayer -= OnPlayerAttacked;
        }

        _voiceMessagePlayer.Play(VoiceMessage.SpiderScare);
    }

    private void OnPickuped()
    {
        _flashlight.Enable();
        _flashlightIcon.Enable();
        _pickupFlashlightText.Disable();
        _isFlashlighPickuped = true;
        _audioPlayer.PlayTorchPickup();
    }

    private void CreateCleenTween()
    {
        _cleenKeyTween = _cleanKey.transform.DOScale(2, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnClean()
    {
        _playerInput.OnClean -= OnClean;
        _inputStateManager.ToWorldState();
        _cleenKeyTween?.Kill();
        _cleanKey.Disable();
        DOTween.To(
            () => _characterController.height,
            newHeight => _characterController.height = newHeight,
            2,
            1f).SetEase(Ease.Linear);

        _pickupFlashlightText.Enable();
        _voiceMessagePlayer.Play(VoiceMessage.YouAlive);
    }

    private void OnOxygenRestored()
    {
        if (_isOxyDraining == false)
        {
            _isOxyDraining = true;
            _oxygenManager.StartOxygenDrain();
            _voiceMessagePlayer.Play(VoiceMessage.ISeeYou);
        }
    }

}

public enum VoiceMessage
{
    YouAlive,
    ISeeYou,
    SpiderScare,
    StopMove,
    KeepMove,
    SoClose,
}
