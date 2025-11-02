using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Source.Scripts.DI.Services.Game
{
    public interface IColorizationFSEffect
    {
        public void Disable();

        public void Enable();

        public void SetStrength(float strength);
    }

    public class ColorizationFSEffect : IColorizationFSEffect
    {
        private const string BlendParameter = "_BlendValue";
        private const float MaxColorize = 1f;
        private const float MinColorize = 0f;

        private readonly ScriptableRendererFeature _feature;
        private readonly Material _material;

        public ColorizationFSEffect(ScriptableRendererFeature feature, Material material)
        {
            _feature = feature != null ? feature : throw new System.ArgumentNullException(nameof(feature));
            _material = material != null ? material : throw new System.ArgumentNullException(nameof(material));

            _feature.SetActive(false);
        }

        public void Enable()
        {
            _material.SetFloat(BlendParameter, MaxColorize);
            _feature.SetActive(true);
        }

        public void Disable()
        {
            _material.SetFloat(BlendParameter, MaxColorize);
            _feature.SetActive(false);
        }

        public void SetStrength(float strength)
        {
            strength = Mathf.Clamp(strength, MinColorize, MaxColorize);
            _material.SetFloat(BlendParameter, strength);
        }
    }
}