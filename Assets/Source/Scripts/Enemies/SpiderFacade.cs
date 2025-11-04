using System.Collections;
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
        StartCoroutine(Routine());
    }
    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(10f);
        _stateFactory.CreateStateMachine(_navigator, _playerDetector);        
    }
}


