using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FillableImage : MonoBehaviour
{
    [SerializeField] private Image _fillableImage;
    [SerializeField] private float _fillTime = 2f;
    [SerializeField] private float _unfillTime = 0.5f;

    private Tween _fillTween;

    public event Action FillComplete;

    private void OnEnable()
    {
        _fillableImage.fillAmount = 0;
    }

    public void Fill()
    {
        if (_fillTween != null && _fillTween.IsActive())
            _fillTween.Kill();

        _fillableImage.fillAmount = 0;
        _fillTween = _fillableImage
            .DOFillAmount(1f, _fillTime)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() => FillComplete?.Invoke());
    }

    public void Unfill()
    {
        if (_fillTween != null && _fillTween.IsActive())
            _fillTween.Kill();

        _fillTween = _fillableImage
            .DOFillAmount(0f, _unfillTime)
            .SetEase(Ease.Linear)
            .SetUpdate(true);

    }
}