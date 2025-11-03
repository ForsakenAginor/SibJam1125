using UnityEngine;

public interface IPlayerTransform
{
    public Transform Head { get; }

    public Transform Player { get; }
}

public class PlayerTransform : MonoBehaviour, IPlayerTransform
{
    [SerializeField] private Transform _head;

    public Transform Player => transform;

    public Transform Head => _head;
}
