using UnityEngine;
using Zenject;

public class SpiderAttackParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private particleAttractorLinear _attractor;

    [Inject]
    public void Construct(IPlayerTransform playerTransform)
    {
        _attractor.target = playerTransform.Head;
    }

    public void Play()
    {
        _particleSystem.Play();
    }
}


