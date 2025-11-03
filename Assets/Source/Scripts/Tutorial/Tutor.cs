using Assets.Source.Scripts.DI.Services.Global;
using Assets.Source.Scripts.Utility;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Tutor : MonoBehaviour
{
    [SerializeField] private OxygenDrainTrigger _oxygenDrainTrigger;
    [SerializeField] private SwitchableElement _cleanKey;
    [SerializeField] private FlashlightObject _flashlightObject;
    [SerializeField] private SwitchableElement _flashlightIcon;
    [SerializeField] private SwitchableElement _pickupFlashlightText;

    private CharacterController _characterController;
    private Flashlight _flashlight;
    private PlayerOxygenManager _oxygenManager;
    private IPlayerInput _playerInput;
    private IInputStateManager _inputStateManager;
    private bool _isOxyDraining = false;
    private bool _isFlashlighPickuped = false;

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
        _pickupFlashlightText.Disable();
        _isFlashlighPickuped = true;
    }

    private void CreateCleenTween()
    {
        _cleenKeyTween = _cleanKey.transform.DOScale(2, 0.5f).SetLoops(-1, LoopType.Yoyo);
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

        if (_isFlashlighPickuped == false)
            _pickupFlashlightText.Enable();

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

public class VoiceMessagePlayer : SerializedMonoBehaviour
{
    [ShowInInspector, OdinSerialize] private Dictionary<VoiceMessage, AudioClip> _clips;
    [SerializeField] private AudioSource _audioSource;

    private ICoroutineRunner _coroutineRunner;
    private CoroutineQueue _queue;

    [Inject]
    public void Construct(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
        _queue = _coroutineRunner.StartCorotineQueue();
        _queue.StartLoop();
    }

    private void OnDestroy()
    {
        _queue.StopLoop();
    }

    public void Play(VoiceMessage message)
    {
        _queue.EnqueueCoroutine(PlayVoiceMessage(message));
    }

    private IEnumerator PlayVoiceMessage(VoiceMessage message)
    {
        WaitForSeconds delay = new WaitForSeconds(_clips[message].length);
        WaitForSeconds pause = new WaitForSeconds(2f);
        yield return pause;

        _audioSource.clip = _clips[message];
        _audioSource.Play();

        yield return delay;
        _audioSource.Stop();
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
