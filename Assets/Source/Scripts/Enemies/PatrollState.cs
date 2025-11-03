using Assets.Source.Scripts.DI.Services.Global;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PatrollState : State
{
    private readonly SpiderNavigator _navigator;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly PlayerDetector _playerDetector;

    private Coroutine _coroutine;

    public PatrollState(ICoroutineRunner coroutineRunner, SpiderNavigator navigator, PlayerDetector detector,
        Transition[] transitions) : base(transitions)
    {
        _navigator = navigator;
        _coroutineRunner = coroutineRunner;
        _playerDetector = detector;
    }

    ~PatrollState()
    {
        Unsubscribe();
    }

    public override void DoThing()
    {
        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.Scared += OnScared;
        _coroutine = _coroutineRunner.StartCoroutine(Patroll());
    }

    private void OnScared()
    {
        Unsubscribe();
        Transitions.First(o => o is ToFleeTransition).SetIsReady(true);
        CallBecomeReadyToTransit();
    }

    private void OnPlayerDetected()
    {
        Unsubscribe();
        Transitions.First(o => o is ToAttackTransition).SetIsReady(true);
        CallBecomeReadyToTransit();
    }

    private void Unsubscribe()
    {
        _playerDetector.Scared -= OnScared;
        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _coroutineRunner.StopCoroutine(_coroutine);
    }

    private IEnumerator Patroll()
    {
        bool isWorking = true;

        while (isWorking)
        {
            _navigator.MoveToRandomPosition();
            yield return new WaitUntil(() => _navigator.IsCloseToPoint());
        }
    }
}


