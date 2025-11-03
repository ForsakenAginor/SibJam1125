using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private float fogVoidKoefMin = 0.5f;
        private Vector3 fogVoidScaleStart;

        [SerializeField] private VolumetricFog fog;

        [SerializeField] private AudioSource audioSource;

        public Image fillImage;

        public bool isDisabled = false;

        public bool IsRecharge = false;

        public bool IsWorking => intensity > intensityStart * 0.5f;

        public float IntensityKoef { get; private set; }

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
            fogVoidScaleStart = fogVoid.transform.localScale;

            audioSource = GetComponent<AudioSource>();
            audioVolumeStart = audioSource.volume;

            currentFadeSpeed = fadeSpeed;

            intensityStart = light.intensity;
            intensity = light.intensity;

            falloff = fogVoid.falloff;
            falloffStart = fogVoid.falloff;

            if (fog != null)
            {
                fogStrength = fog.settings.noiseStrength;
                fogStrengthStart = fog.settings.noiseStrength;
            }
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

            IntensityKoef = intensity / intensityStart;

            fogVoid.falloff = Mathf.Lerp(1, falloffStart, IntensityKoef);
            var scaleLerp = Mathf.Lerp(0.5f, 1, IntensityKoef);
            fogVoid.transform.localScale = fogVoidScaleStart * scaleLerp;

            if (fog != null)
            {
                fog.settings.noiseStrength = Mathf.Lerp(fogStrengthStart, 0, IntensityKoef);
            }

            audioSource.volume = Mathf.Lerp(audioVolumeStart, audioVolumeStart, IntensityKoef);

            if (IsRecharge)
            {
                fillImage.enabled = true;
                fillImage.fillAmount = IntensityKoef;
            }
            else
            {
                fillImage.enabled = false;
            }
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