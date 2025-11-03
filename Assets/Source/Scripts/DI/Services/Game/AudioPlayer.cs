using Assets.Source.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Source.Scripts.DI.Services.Game
{
    public class AudioPlayer : MonoBehaviour
    {
        [Header("Queue logic")]
        private readonly int _maxSimultaneousSounds = 10;
        private readonly Queue<AudioSource> _audioSources = new Queue<AudioSource>();
        private readonly Dictionary<AudioSource, WaitWhileCached> _cachedWaitWhiles = new Dictionary<AudioSource, WaitWhileCached>();
        [SerializeField] private AudioSource[] _audioSourcesArray;

        [SerializeField] private AudioSource _oxyRecharge;
        [SerializeField] private AudioSource _oxyRollback;
        [SerializeField] private AudioSource _spiderAttack;
        [SerializeField] private AudioSource _spiderFlee;
        [SerializeField] private AudioSource _takeDamage;
        [SerializeField] private AudioSource _cleanScreen;

        private void Awake()
        {
            if (_audioSourcesArray.Length != _maxSimultaneousSounds)
                throw new Exception("audioSources amount is not correct");

            for (int i = 0; i < _maxSimultaneousSounds; i++)
            {
                _audioSources.Enqueue(_audioSourcesArray[i]);
                _cachedWaitWhiles.Add(_audioSourcesArray[i], new WaitWhileCached(() => false));
            }
        }

        public void PlayCleanScreen()
        {
            _cleanScreen.Play();
        }

        public void PlayTakeDamage()
        {
            _takeDamage.Play();
        }

        public void PlayOxyRecharge()
        {
            _oxyRecharge.Play();
        }

        public void PlayOxyRollback()
        {
            _oxyRecharge.Stop();
            _oxyRollback.Play();
        }

        public void PlaySpiderAttack(Transform spider)
        {
            _spiderAttack.transform.SetParent(spider);
            _spiderAttack.transform.localPosition = Vector3.zero;
            _spiderAttack.Play();
        }

        public void PlaySpiderFlee(Transform spider)
        {
            _spiderFlee.transform.SetParent(spider);
            _spiderFlee.transform.localPosition = Vector3.zero;
            _spiderFlee.Play();
        }

        private void PlaySound(AudioClip clip, float volumeMultiplier = 1f, bool isRandomPitch = false)
        {
            if (volumeMultiplier > 1f)
                throw new NotImplementedException();
            if (_audioSources.Count == 0)
                return;

            AudioSource source = _audioSources.Dequeue();
            source.clip = clip;

            if (isRandomPitch)
                source.pitch = Random.Range(0.75f, 1.15f);

            if (volumeMultiplier != 1f)
                source.volume = source.volume * volumeMultiplier;

            source.Play();
            StartCoroutine(ReturnToPool(source, volumeMultiplier));
        }

        private IEnumerator ReturnToPool(AudioSource source, float volumeMultiplier)
        {
            _cachedWaitWhiles[source].UpdateCondition(() => source.isPlaying);
            yield return _cachedWaitWhiles[source];
            source.pitch = 1f;
            source.volume = source.volume / volumeMultiplier;
            _audioSources.Enqueue(source);
        }
    }
}