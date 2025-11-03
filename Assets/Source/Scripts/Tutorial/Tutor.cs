using Assets.Source.Scripts.Utility;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Tutor : MonoBehaviour
{
    [SerializeField] private OxygenDrainTrigger _oxygenDrainTrigger;
    [SerializeField] private SwitchableElement _cleanKey;
    private PlayerOxygenManager _oxygenManager;
    private IPlayerInput _playerInput;
    private IInputStateManager _inputStateManager;
    private bool _isOxyDraining = false;

    private Tween _cleenKeyTween;

    [Inject]
    public void Construct(IPlayerInput playerInput, IInputStateManager inputStateManager)
    {
        _playerInput = playerInput;
        _inputStateManager = inputStateManager;

        _playerInput.OnClean += OnClean;
    }

    private void OnDestroy()
    {
        _playerInput.OnClean -= OnClean;
        _oxygenManager.OxygenRestored -= OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter -= OnOxygenRestored;
    }

    public void Init(PlayerOxygenManager oxygenManager)
    {
        _oxygenManager = oxygenManager;
        CreateCleenTween();

        _oxygenManager.OxygenRestored += OnOxygenRestored;
        _oxygenDrainTrigger.PlayerEnter += OnOxygenRestored;
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
