using System;
using System.Collections;
using UnityEngine;
using VolumetricFogAndMist2;

namespace Assets.Source.Scripts.Utility
{
    [SelectionBase]
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private Light light;
        [SerializeField] private float fadeSpeed = 1f;
        private float currentFadeSpeed;

        [SerializeField] private float rechargeSpeedMax = 5f;
        [SerializeField] private float rechargeIncreaseSpeed = 1f;
        private float currentRechargeSpeed;

        [SerializeField] private FogPointLight fogPoint;
        [SerializeField] private FogVoid fogVoid;
        [SerializeField] private VolumetricFog fog;

        [SerializeField] private AudioSource audioSource;

        public bool isDisabled = false;

        public bool IsRecharge = false;

        [SerializeField] private float rechargeTime = 1f;
        private float rechargeTimeout;

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

            currentFadeSpeed = fadeSpeed;

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

            if (IsRecharge)
            {
                currentRechargeSpeed += rechargeIncreaseSpeed * Time.deltaTime;
                currentRechargeSpeed = Mathf.Clamp(currentRechargeSpeed, 0, rechargeSpeedMax);
                intensity += currentRechargeSpeed * Time.deltaTime;
            }
            else
            {
                currentRechargeSpeed -= rechargeIncreaseSpeed * Time.deltaTime;
                currentRechargeSpeed = Mathf.Clamp(currentRechargeSpeed, 0, rechargeSpeedMax);
                intensity -= currentFadeSpeed * Time.deltaTime;
            }

            intensity = Mathf.Clamp(intensity, 0, intensityStart);
            light.intensity = intensity;

            var intensityKoef = (float)intensity / intensityStart;

            fogVoid.falloff = Mathf.Lerp(1, falloffStart, intensityKoef);
            fog.settings.noiseStrength = Mathf.Lerp(fogStrengthStart, 0, intensityKoef);

            audioSource.volume = Mathf.Lerp(audioVolumeStart, audioVolumeStart, intensityKoef);
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