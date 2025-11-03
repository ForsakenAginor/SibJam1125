using UnityEngine;

public interface IPlayerTransform
{
    public Transform Player { get; }
}

public class PlayerTransform : MonoBehaviour, IPlayerTransform
{
    public Transform Player => transform;
}
