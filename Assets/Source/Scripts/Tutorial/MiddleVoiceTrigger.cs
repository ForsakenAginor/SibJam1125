using System;
using UnityEngine;

public class MiddleVoiceTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    public event Action PlayerEnter;

    private void OnTriggerEnter(Collider other)
    {

        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerEnter?.Invoke();
        }
    }
}
