using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    public event Action PlayerDetected;
    public event Action Scared;

    private void OnTriggerEnter(Collider other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerDetected?.Invoke();
        }
    }

    public void Scare()
    {
        Debug.Log("Scared");
        Scared?.Invoke();
    }
}


