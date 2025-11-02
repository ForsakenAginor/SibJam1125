using System;
using UnityEngine;
using VolumetricFogAndMist2;

namespace Assets.Source.Scripts.Utility
{
    [SelectionBase]
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private Light light;
        [SerializeField] private float fadeSpeed = 1f;

        [SerializeField] private FogPointLight fogPoint;
        [SerializeField] private FogVoid fogVoid;
        [SerializeField] private VolumetricFog fog;

        [SerializeField] private AudioSource audioSource;

        public bool isDisabled = false;

        public bool isRecharge = false;

        //private bool _isOn = false;
        private float intensity;
        private float intensityStart;

        private float falloff;
        private float falloffStart;

        private float fogStrength;
        private float fogStrengthStart;

        public float audioVolumeStart;

        private void Start()
        {
            fog = FindFirstObjectByType<VolumetricFog>();

            fogPoint = GetComponent<FogPointLight>();
            fogVoid = GetComponentInChildren<FogVoid>();

            audioSource = GetComponent<AudioSource>();
            audioVolumeStart = audioSource.volume;

            intensityStart = light.intensity;
            intensity = light.intensity;

            falloff = fogVoid.falloff;
            falloffStart = fogVoid.falloff;

            fogStrength = fog.settings.noiseStrength;
            fogStrengthStart = fog.settings.noiseStrength;
        }

        private void Update()
        {
            if (isDisabled)
                return;

            intensity -= fadeSpeed * Time.deltaTime;
            intensity = Mathf.Clamp(intensity, 0, intensityStart);
            light.intensity = intensity;

            var intensityKoef = (float)intensity / intensityStart;

            fogVoid.falloff = Mathf.Lerp(1, falloffStart, intensityKoef);
            fog.settings.noiseStrength = Mathf.Lerp(fogStrengthStart, 0, intensityKoef);

            audioSource.volume = Mathf.Lerp(audioVolumeStart, audioVolumeStart, intensityKoef);
        }

        public void Recharge()
        {
            intensity = intensityStart;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject?.SetActive(false);
        }
    }
}