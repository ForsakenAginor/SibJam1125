using Assets.Source.Scripts.Utility;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class RadaraController : MonoBehaviour
{
    [SerializeField] private SwitchableElement _radar;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private AudioSource audioDisableSource;
    [SerializeField] private AudioSource audioEnableSource; //сурсы для аудио
    [SerializeField] private AudioSource _radarBeep;
    private float _showY = -0.5f;
    private float _hideY = -0.678f;

    private IPlayerInput _input;
    private bool _isEnable = false;
    private Tween _tween;
    

    [Inject]
    public void Construct(IPlayerInput input)
    {
        _input = input;
        _radar.Disable();

        _input.OnRadarPressed += OnRadarPressed;
    }
    private void Update()
    {
        if (_isEnable == false && _radarBeep.isPlaying)
        {
            _radarBeep.Stop();
        }

        if (_isEnable == false)
            return;


    }

    private void OnDestroy()
    {
        _input.OnRadarPressed -= OnRadarPressed;
    }

    private void OnRadarPressed()
    {
        _tween?.Kill();

        _isEnable = !_isEnable;

        float targetY = _isEnable ? _showY : _hideY;

        if (_isEnable)
        {
            _radar.Enable();
            audioEnableSource.Play(); 
        }
        else audioDisableSource.Play();//если при убирании, то проигрываем выключение

        _tween = _radar.transform.DOLocalMoveY(targetY, _animationDuration)
                .SetEase(_isEnable ? Ease.OutBack : Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (!_isEnable)
                    {
                        _radar.Disable();
                    }
                    else
                    {
                        if (_radarBeep.isPlaying == false)
                            _radarBeep.Play();
                    }
                });
    }
}
