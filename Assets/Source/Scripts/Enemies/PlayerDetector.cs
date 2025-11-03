using Assets.Source.Scripts.DI.Services.Game;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    private AudioPlayer _audio;
    private WaitForSeconds _delay = new WaitForSeconds(4f);
    private bool _canScare = true;

    public event Action PlayerDetected;
    public event Action Scared;


    [Inject]
    public void Construct(AudioPlayer audio)
    {
        _audio = audio;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerDetected?.Invoke();
        }
    }

    public void Scare()
    {
        if(_canScare)
            StartCoroutine(ScareRoutine());
    }

    private IEnumerator ScareRoutine()
    { 
        _canScare = false;  
        _audio.PlaySpiderFlee(transform.parent);
        Scared?.Invoke();
        yield return _delay;
        _canScare = true;
    }
}


