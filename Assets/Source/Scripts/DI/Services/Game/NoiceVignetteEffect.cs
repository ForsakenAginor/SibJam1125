using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Source.Scripts.DI.Services.Game
{
    public interface INoiseVignetteEffect
    {
        public void Disable();

        public void Enable();

        public void SetEffectStrength(float value);
    }

    public class NoiceVignetteEffect : INoiseVignetteEffect
    {
        private const string RadiusParameter = "_Radius";
        private const string SoftnessParameter = "_Softness";
        private const string ColorParameter = "_Color";

        private const float MinRadius = 0.3f;
        private const float MinSoftness = 0.6f;
        private const float MaxRadius = 0.8f;
        private const float MaxSoftness = 0.7f;
        private const float TweenDuration = 1f;

        private readonly ScriptableRendererFeature _feature;
        private readonly Material _material;

        private Tweener _radiusTweener;
        private Tweener _softnessTweener;
        private Tweener _colorTweener;

        public NoiceVignetteEffect(ScriptableRendererFeature feature, Material material)
        {
            _feature = feature != null ? feature : throw new System.ArgumentNullException(nameof(feature));
            _material = material != null ? material : throw new System.ArgumentNullException(nameof(material));
            _feature.SetActive(false);
        }

        public void Enable()
        {
            _material.SetFloat(RadiusParameter, MaxRadius);
            _material.SetFloat(SoftnessParameter, MaxSoftness);
            _feature.SetActive(true);
        }

        public void Disable()
        {
            _feature.SetActive(false);
        }

        public void SetEffectStrength(float value)
        {
            if (value < 0 || value > 1f)
                throw new System.ArgumentOutOfRangeException(nameof(value));

            _radiusTweener?.Kill();
            _softnessTweener?.Kill();
            _colorTweener?.Kill();

            float remappedRadius = Unity.Mathematics.math.remap(0, 1, MinRadius, MaxRadius, value);
            float remappedSoftness = Unity.Mathematics.math.remap(0, 1, MinSoftness, MaxSoftness, value);
            Color targetColor = Color.Lerp(Color.white, Color.red, value);

            _colorTweener = _material.DOColor(targetColor, ColorParameter, TweenDuration);
            _radiusTweener = _material.DOFloat(remappedRadius, RadiusParameter, TweenDuration);
            _softnessTweener = _material.DOFloat(remappedSoftness, SoftnessParameter, TweenDuration);
        }
    }
}