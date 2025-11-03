using Assets.Source.Scripts.DI.Services.Game;
using System;
using UnityEngine;
using Zenject;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    private AudioPlayer _audio;

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
        _audio.PlaySpiderFlee(transform.parent);
        Scared?.Invoke();
    }
}


