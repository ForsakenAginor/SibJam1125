using UnityEngine;
using Zenject;

public class SpiderFacade : MonoBehaviour
{
    [SerializeField] private SpiderNavigator _navigator;
    [SerializeField] private PlayerDetector _playerDetector;

    private SpiderStateMachineFactory _stateFactory;

    [Inject]
    public void Construct(SpiderStateMachineFactory factory)
    {
        _stateFactory = factory;
    }

    private void Start()
    {
        _stateFactory.CreateStateMachine(_navigator, _playerDetector);        
    }
}

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


