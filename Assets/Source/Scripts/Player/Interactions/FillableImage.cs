using Assets.Source.Scripts.DI.Services.Game;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FillableImage : MonoBehaviour
{
    [SerializeField] private Image _fillableImage;
    [SerializeField] private float _fillTime = 2f;
    [SerializeField] private float _unfillTime = 0.5f;

    private Tween _fillTween;
    private AudioPlayer _audioPlayer;

    public event Action FillComplete;

    [Inject]
    public void Construct(IGameSettings gameSettings, AudioPlayer audioPlayer)
    {
        _audioPlayer = audioPlayer;
        _unfillTime = gameSettings.OxygenReloadTime;
    }

    private void OnEnable()
    {
        _fillableImage.fillAmount = 0;
    }

    public void Fill()
    {
        if (_fillTween != null && _fillTween.IsActive())
            _fillTween.Kill();

        _audioPlayer.PlayOxyRecharge();
        _fillableImage.fillAmount = 0;
        _fillTween = _fillableImage
            .DOFillAmount(1f, _fillTime)
            .SetEase(Ease.Linear)
            .OnComplete(() => FillComplete?.Invoke());
    }

    public void Unfill()
    {
        if (_fillTween != null && _fillTween.IsActive())
            _fillTween.Kill();

        _audioPlayer.PlayOxyRollback();
        _fillTween = _fillableImage
            .DOFillAmount(0f, _unfillTime)
            .SetEase(Ease.Linear);
    }
}