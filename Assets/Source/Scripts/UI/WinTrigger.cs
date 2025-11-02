using System;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    public event Action PlayerWon;

    private void OnTriggerEnter(Collider other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerWon?.Invoke();
        }
    }
}
